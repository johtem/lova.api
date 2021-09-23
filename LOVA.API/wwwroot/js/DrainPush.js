"use strict";


var connection = new signalR.HubConnectionBuilder()
    .withUrl("/activationHub")
    .build();

var myVarActive;
var myVarDeActive;

var connectionStatus = document.getElementById("connectionStatus");

moment.locale('sv');  // Set moment to Swedish date and time


connection.on("DrainActivity", function (user, message) {
    //console.log(message);
    //console.log(message["address"]);
    var time = moment(message["time"]).format('ddd HH:mm.ss ');
    var msg = message["active"];
    var encodedMsg = time + " - " + user + "  aktiv: " + msg;
    var li = document.createElement("li");
    var childLi = document.getElementsByTagName("li");  


    if (message["active"] == true) {
        li.setAttribute('style', 'color: #fc9005;');

        document.getElementById(message["address"]).classList.remove("tableLiveTdNonActive", "tableLiveTdActive");

        document.getElementById(message["address"]).setAttribute("data-timeup", message["time"]);
        document.getElementById(message["address"]).classList.add("active", "tableLiveTdActive");
       
    } else {
        li.setAttribute('style', 'color: #5ab25e;');
        document.getElementById(message["address"]).classList.remove("tableLiveTdNonActive", "tableLiveTdActive", "active", 'tableLiveTdLongActive');

        document.getElementById(message["address"]).classList.add("tableLiveTdNonActive");
    }

    li.textContent = encodedMsg;
    
    //document.getElementById("messagesList").appendChild(li);
    document.querySelector("#messagesList").prepend(li);

    var drainFromPage = document.getElementById("address").innerHTML;
   

    if (drainFromPage == user) {
        drainUpdate(user);
    }

});

connection.on("Drain", function (user, message, dateNow) {
    
    var aTime = document.getElementById("activationTime");
    var deATime = document.getElementById("deActivationTime");

    aTime.setAttribute("data-time", message["timeUp"]);
    deATime.setAttribute("data-time", message["timeDown"]);
    
    document.getElementById("activationCount").textContent = message["hourlyCount"];
    document.getElementById("activationDailyCount").textContent = message["dailyCount"];
    document.getElementById("activationAverage").textContent = secondsToTime(message["averageActivity"]);
    document.getElementById("deActivationRestAverage").textContent = secondsToTime(message["averageRest"]);


    if (message["isActive"] == true) {
        aTime.style.color = "green";
        deATime.style.color = "black";

        aTime.textContent = moment(message["timeUp"]).format('ddd D/M HH:mm.ss');
        deATime.textContent = moment(message["timeDown"]).add(-2, 'hours').format('ddd D/M HH:mm.ss ');

        document.getElementById("activationRunningTime").textContent = secondsToTime(moment(dateNow).diff(moment(message["timeUp"]).add(0, 'hours'), "seconds", false));
        document.getElementById("deActivationRunningTime").textContent = secondsToTime(moment(message["timeUp"]).diff(moment(message["timeDown"]).add(-2, 'hours'), "seconds", false));
       

        myVarActive = setInterval(myTimerActive, 1000);
    } else {
        aTime.style.color = "black";
        deATime.style.color = "red";
        

        aTime.textContent = moment(message["timeUp"]).format('ddd D/M HH:mm.ss');
        deATime.textContent = moment(message["timeDown"]).add(-2, 'hours').format('ddd D/M HH:mm.ss');

        document.getElementById("activationRunningTime").textContent = secondsToTime(moment(message["timeDown"]).diff(moment(message["timeUp"]).add(2, 'hours'), "seconds", false));
        document.getElementById("deActivationRunningTime").textContent = secondsToTime(moment(dateNow).diff(moment(message["timeDown"]).add(-2, 'hours'), "seconds", false));

        myVarDeActive = setInterval(myTimerDeActive, 1000);
    }

    //console.log(moment(message["timeUp"]).format('ddd HH:mm.SS '));
});




connection.onclose(function () {
    onDisconnected();
    console.log("Reconnect in 5 seconds...");
    setTimeout(startConnection, 5000);
})

function startConnection() {
    connection.start()
        .then(onConnected)
        .catch(function (err) {
            console.error(err);
        });
}

function onConnected() {
    connectionStatus.textContent = "Ansluten";
}

function onDisconnected() {
    connectionStatus.textContent = "Försöker att återansluta...";
}




startConnection();


function drainUpdate(drain) {
    
    myStopFunctionActive();
    myStopFunctionDeActive();
    connection.invoke("Drain", drain);
    
};


function myTimerActive() {
    
    var t = moment.duration(document.getElementById("activationRunningTime").innerHTML).asSeconds();
    document.getElementById("activationRunningTime").innerHTML = secondsToTime(parseInt(t, 10) + 1);

    
}

function myStopFunctionActive() {
    clearInterval(myVarActive);
}

function myTimerDeActive() {

    var t = moment.duration(document.getElementById("deActivationRunningTime").innerHTML).asSeconds();
    document.getElementById("deActivationRunningTime").innerHTML = secondsToTime(parseInt(t, 10) + 1); 

}

function myStopFunctionDeActive() {
    clearInterval(myVarDeActive);
}


function secondsToTime(seconds) {


    return moment.utc(moment.duration(seconds, "seconds").asMilliseconds()).format("HH:mm:ss");

    
}





// Check for long avtivation time every 5 seconds

var longActivations = setInterval(checkLongActivationTime, 5000);

function checkLongActivationTime() {

    var dateNow = new Date();

    var allActive = document.getElementsByClassName("active");
    //console.log(allActive);

    var i;
    for (i = 0; i < allActive.length; i++) {
        var secondsBetween = moment(dateNow).diff(moment(allActive[i].getAttribute("data-timeup")), "seconds", false)
        //console.log(secondsBetween);

        if (parseInt(secondsBetween, 10) > 100) {
            var id = allActive[i]["id"];
            //console.log(id);
            document.getElementById(id).classList.add('tableLiveTdLongActive', 'active');

            document.getElementById(id).classList.remove("tableLiveTdActive", "tableLiveTdNonActive");
            
        }

        if (parseInt(secondsBetween, 10) > 600) {

            document.getElementById(id).classList.add('redSolidBorder');
        }
    }
}

