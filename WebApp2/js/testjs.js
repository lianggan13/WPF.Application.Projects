"use strict";


function GetJSON_Simple() {
    var resp = [];
    return resp;
}

debugger;
var simpleData = GetJSON_Simple();

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();