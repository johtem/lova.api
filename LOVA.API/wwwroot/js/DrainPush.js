"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/activationHub").build();

  

connection.on("ReceiveMessage", function (user, message) {
    //console.log(message);
    //console.log(message["address"]);
    // var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var msg = message["address"] + " " + message["active"];
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");

//   li.setAttribute('style', 'color: red;');


    if (message["active"] == true) {
        li.setAttribute('style', 'color: #fc9005;');
        document.getElementById(message["address"]).style.backgroundColor = '#fc9005';
    } else {
        li.setAttribute('style', 'color: #5ab25e;');
        document.getElementById(message["address"]).style.backgroundColor = '#5ab25e';
    }

    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
   
}).catch(function (err) {
    return console.error(err.toString());
});

