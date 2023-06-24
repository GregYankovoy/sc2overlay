"use strict";

let root = document.documentElement;

let hud = document.getElementById("hud");

let p1name = document.getElementById("p1name");
let p1mmr = document.getElementById("p1mmr");

let p2name = document.getElementById("p2name");
let p2mmr = document.getElementById("p2mmr");

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/overlay")
    .withAutomaticReconnect()
    .build();

connection.on("ChangeScene", function (scene) {
    console.info(`Switched to scene ${scene}`);
});

connection.on("EnterGame", function (game) {
    console.info('EnterGame fired.');

    hud.style.display = "block";

    p1name.innerHTML = game.players[0].name;
    p1mmr.innerHTML = game.players[0].mmr;
    p1race.innerHTML = game.players[0].race.charAt(0);

    p2name.innerHTML = game.players[1].name;
    p2mmr.innerHTML = game.players[1].mmr;
    p2race.innerHTML = game.players[1].race.charAt(0);
});

connection.on("LeaveGame", function (game) {
    console.info(`LeaveGame fired.`);

    hud.style.display = "none";

    p1name.innerHTML = null;
    p1mmr.innerHTML = null;
    p1race.innerHTML = null;

    p2name.innerHTML = null;
    p2mmr.innerHTML = null;
    p2race.innerHTML = null;
});

connection.start().then(function () {
    console.info("Connected to SignalR Hub.");
}).catch(function (err) {
    return console.error(err.toString());
});
