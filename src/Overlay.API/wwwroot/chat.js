"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/overlay")
    .withAutomaticReconnect()
    .build();

connection.on("ChangeScene", function (scene) {
    console.info(`Switched to scene ${scene}`);
});

connection.on("EnterGame", function (game) {
    console.info('EnterGame fired.');

    document.getElementById("details").innerHTML = `
        [${game.players[0].mmr}] 
        ${game.players[0].name}  
        (${game.players[0].race}) 
            vs 
        (${game.players[1].race})
        ${game.players[1].name} 
        [${game.players[1].mmr}]
    `;
});

connection.on("LeaveGame", function (game) {
    console.info(`LeaveGame fired.`);
});

connection.start().then(function () {
    console.info("Connected to SignalR Hub.");
}).catch(function (err) {
    return console.error(err.toString());
});
