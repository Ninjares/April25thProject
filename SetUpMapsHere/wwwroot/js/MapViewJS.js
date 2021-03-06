﻿var mymap = L.map('mapid');
navigator.geolocation.getCurrentPosition((position) => mymap.setView([position.coords.latitude, position.coords.longitude], 14));

L.tileLayer
    ('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw', {
        maxZoom: 18,
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        id: 'mapbox/streets-v11',
        accessToken: 'your.mapbox.access.token'
    }).addTo(mymap);

L.marker([42.686, 23.319]).bindPopup(`<input id="create-btn" type="button" class="btn btn-primary" value="Create"/>`).addTo(mymap);


fetch('/Map/GetRoutes').then(x => x.json()).then(routes => routes.forEach((route) => { console.log(route); L.polyline(route.coordinates, { color: route.colorHex }).addTo(mymap); }));
fetch('/Map/GetStops').then(x => x.json()).then(stops => stops.forEach((stop) => { console.log(stop); L.marker(stop.coordinates, { icon: StopIcon }).bindPopup(stop.stopList).addTo(mymap);}));


var StopIcon = L.icon({
    iconUrl: "https://cdn3.iconfinder.com/data/icons/transport-29/100/22-512.png",

    //iconUrl: "/js/busStop.png",       //this works

    iconSize: [40, 40], // size of the icon
    iconAnchor: [20, 40], // point of the icon which will correspond to marker's location
    popupAnchor: [0, -35]
});

var UserIcon = L.icon({
    iconUrl: "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fmeridianapps.com%2Fimages%2Ficon_bludot%402x.png&f=1&nofb=1",

    iconSize: [40, 40],
    iconAnchor: [20, 20]
})

var BusIcon = L.icon({
    iconUrl: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fcdn3.iconfinder.com%2Fdata%2Ficons%2Ftransportation%2F100%2Ftransportation__bus-512.png&f=1&nofb=1",

    iconSize: [30, 30],
    iconAnchor: [15, 15]
});

var userlocation = L.marker({ icon: UserIcon });

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
        navigator.geolocation.getCurrentPosition((position) => { userlocation.setLatLng([position.coords.latitude, position.coords.longitude]).setIcon(UserIcon).addTo(mymap);});
    }
}

var connection = new signalR.HubConnectionBuilder().withUrl("/gps").build();
var busarray = {};
connection.on("DisplayDrivers", function (buses) {
    //alert("Received drivers");
    buses.forEach(allbuses);
});
connection.on("RemoveDriver", function (driverid) {
    busarray[driverid].remove();
    delete busarray[driverid];
});
function allbuses(bus, index, arr) {
    //alert(bus.location);
    if (busarray[bus.id] == null) busarray[bus.id] = L.marker();
    busarray[bus.id].setLatLng(bus.location).setIcon(BusIcon).bindPopup(bus.busLine).addTo(mymap);
}

connection.on("Confirm", function () {
    alert("Confirmed");
});

function EngageBroadcast() {
    connection.invoke("GetAllDrivers");
}
connection.start().catch(function (err) {
    return console.error(err.toString());
});
setTimeout(EngageBroadcast, 1000);
