using ASDDbContext.Models;
using ASPDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SetUpMapsHere.Services
{
    public class UserService : IUserService
    {
        private TransportDbContext db;
        public UserService(TransportDbContext db)
        {
            this.db = db;
        }
        public User GetUser(string name, string password)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == name && x.PasswordHash == Hash(password));
            if (user == null) return null;
            else return user;
        }

        public bool RegisterUser(string name, string email, string password)
        {
            var user = new User()
            {
                UserName = name,
                NormalizedUserName = name.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                PasswordHash = Hash(password)
            };
            if (db.Users.Any(x => x.NormalizedUserName == name.ToUpper() || x.Email == email.ToUpper())) return false;
            db.Users.Add(user);
            db.SaveChanges();
            return db.Users.Any(x => x.NormalizedUserName == name.ToUpper() && x.Email == email.ToUpper());
        }
        private string Hash(string input)
        {
            if (input == null)
                return null;

            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
