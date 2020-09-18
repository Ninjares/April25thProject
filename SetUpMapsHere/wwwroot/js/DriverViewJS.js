var mymap = L.map('mapid');
navigator.geolocation.getCurrentPosition((position) => mymap.setView([position.coords.latitude, position.coords.longitude], 14));

L.tileLayer
    ('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw', {
        maxZoom: 18,
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        id: 'mapbox/streets-v11',
        accessToken: 'your.mapbox.access.token'
    }).addTo(mymap);

var StopIcon = L.icon({
    iconUrl: "https://cdn3.iconfinder.com/data/icons/transport-29/100/22-512.png",

    iconSize: [40, 40], // size of the icon
    iconAnchor: [20, 40], // point of the icon which will correspond to marker's location
    popupAnchor: [0, -35]
});

var UserIcon = L.icon({
    iconUrl: "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fmeridianapps.com%2Fimages%2Ficon_bludot%402x.png&f=1&nofb=1",

    iconSize: [40, 40],
    iconAnchor: [20, 20]
})

function looper() {
    getLocation();
    if (true) {
        setTimeout(looper, 1000)
    }
}
looper();


fetch('/Driver/Route').then(x => x.json()).then(route => { console.log(route); L.polyline(route.coordinates, { color: route.colorHex }).addTo(mymap); });
fetch('/Driver/Stops').then(x => x.json()).then(stops => { stops.forEach((stop) => { L.marker(stop.point, { icon: StopIcon }).bindPopup(stop.address).addTo(mymap); })});



var driverlocation = L.marker();
var popup = L.popup();
function onMapClick(e) {
    alert(e.latlng)
    popup
        .setLatLng(e.latlng)
        .setContent(e.latlng.toString())
        .openOn(mymap);
}

mymap.on('click', onMapClick);
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(SuccessfulPosition);
        navigator.geolocation.getCurrentPosition(Sendlocation);
    }
}
function showPosition(positionn) {
    alert(positionn.coords.latitude + " ");
    L.marker([positionn.coords.latitude, positionn.coords.longitude], { icon: UserIcon }).addTo(mymap);
}
function SuccessfulPosition(position) {
    driverlocation.setLatLng([position.coords.latitude + h, position.coords.longitude]).setIcon(UserIcon).addTo(mymap);
}










let h = 0;
var connection = new signalR.HubConnectionBuilder().withUrl("/gps").build();
function Sendlocation(position) {
    connection.invoke("UpdateLocation", position.coords.latitude + h, position.coords.longitude);
    h = h + 0.001;
}
function EndDriving() {
    connection.invoke("RemoveBus");
}
connection.on("AllGood", function (user, x, y) {
    alert("All good! " + user + " " + x + " " + y);
})
connection.start().catch(function (err) {
    return console.error(err.toString());
});

//document.GetElementById("sendButton").addEventListener("click", e => {
//    e.preventDefault();

//});
window.addEventListener('beforeunload', () => {
    EndDriving();
});