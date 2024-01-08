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

        document.getElementById(message["address"]).setAttribute("data-timeup", time); //message["time"]);
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
    
    // console.log(dateNow);
    // console.log(message["timeUp"]);
    // console.log(dateNow.substring(11, 13));
    // console.log(message["timeUp"].substring(11, 13));


    // Checks if new hour if so set activationCount to 0 
    if (dateNow.substring(11, 13) > message["timeUp"].substring(11, 13)) {
        document.getElementById("activationCount").textContent = "0";
       // console.log("hej");
    } else {
        document.getElementById("activationCount").textContent = message["hourlyCount"];
    }
    
    
    document.getElementById("activationDailyCount").textContent = message["dailyCount"];
    document.getElementById("activationAverage").textContent = secondsToTime(message["averageActivity"]);
    document.getElementById("deActivationRestAverage").textContent = secondsToTime(message["averageRest"]);


    if (message["isActive"] == true) {
        aTime.style.color = "green";
        deATime.style.color = "black";

        aTime.textContent = moment(message["timeUp"]).format('ddd D/M HH:mm.ss');
       // deATime.textContent = moment(message["timeDown"]).add(-2, 'hours').format('ddd D/M HH:mm.ss ');
        deATime.textContent = moment(message["timeDown"]).format('ddd D/M HH:mm.ss ');

        //document.getElementById("activationRunningTime").textContent = secondsToTime(moment(dateNow).diff(moment(message["timeUp"]).add(0, 'hours'), "seconds", false));

        var activationDiff = moment(dateNow).diff(moment(message["timeUp"]));
        document.getElementById("activationRunningTime").setAttribute("data-timediff", activationDiff);
        document.getElementById("activationRunningTime").textContent = durationAsString(activationDiff);


        //document.getElementById("deActivationRunningTime").textContent = secondsToTime(moment(message["timeUp"]).diff(moment(message["timeDown"]).add(-2, 'hours'), "seconds", false));
        var deactivationDiff = moment(message["timeUp"]).diff(moment(message["timeDown"]));
        document.getElementById("deActivationRunningTime").setAttribute("data-timediff", deactivationDiff);
        document.getElementById("deActivationRunningTime").textContent = durationAsString(deactivationDiff);
       

        myVarActive = setInterval(myTimerActive, 1000);
    } else {
        aTime.style.color = "black";
        deATime.style.color = "red";
        

        aTime.textContent = moment(message["timeUp"]).format('ddd D/M HH:mm.ss');
       // deATime.textContent = moment(message["timeDown"]).add(-2, 'hours').format('ddd D/M HH:mm.ss');
        deATime.textContent = moment(message["timeDown"]).format('ddd D/M HH:mm.ss');

       // document.getElementById("activationRunningTime").textContent = secondsToTime(moment(message["timeDown"]).diff(moment(message["timeUp"]).add(2, 'hours'), "seconds", false));
        var activationDiff = moment(message["timeDown"]).diff(moment(message["timeUp"]));
        document.getElementById("activationRunningTime").setAttribute("data-timediff", activationDiff);
        document.getElementById("activationRunningTime").textContent = durationAsString(activationDiff);

        //document.getElementById("deActivationRunningTime").textContent = secondsToTime(moment(dateNow).diff(moment(message["timeDown"]), "seconds", false));
        var deactivationDiff = moment(dateNow).diff(moment(message["timeDown"]));
        document.getElementById("deActivationRunningTime").setAttribute("data-timediff", deactivationDiff);
        document.getElementById("deActivationRunningTime").textContent = durationAsString(deactivationDiff);


        myVarDeActive = setInterval(myTimerDeActive, 1000);
    }


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
    
    var t = document.getElementById("activationRunningTime").dataset.timediff;
    var diff = parseInt(t, 10) + 1000;
    document.getElementById("activationRunningTime").innerHTML = durationAsString(diff);
    document.getElementById("activationRunningTime").setAttribute("data-timediff", diff);
    
}

function myStopFunctionActive() {
    clearInterval(myVarActive);
}

function myTimerDeActive() {

    var t = document.getElementById("deActivationRunningTime").dataset.timediff;
    var diff = parseInt(t, 10) + 1000;
    document.getElementById("deActivationRunningTime").textContent = durationAsString(diff);
    document.getElementById("deActivationRunningTime").setAttribute("data-timediff", diff);

}

function myStopFunctionDeActive() {
    clearInterval(myVarDeActive);
}


function secondsToTime(seconds) {


    return moment.utc(moment.duration(seconds, "seconds").asMilliseconds()).format("HH:mm:ss");
    
}





function durationAsString(diff) {
    var duration = moment.duration(diff);



    //Get Days
    var days = Math.floor(duration.asDays()); // .asDays returns float but we are interested in full days only
    var daysFormatted = days ? `${days}d ` : ''; // if no full days then do not display it at all

    //Get Hours
    var hours = duration.hours();
    var hoursFormatted = `${hours}h `;

    //Get Minutes
    var minutes = duration.minutes();
    var minutesFormatted = `${minutes}m `;

    //Get Seconds
    var seconds = duration.seconds();
    var secondsFormatted = `${seconds}s`;

    var str = [daysFormatted, hoursFormatted, minutesFormatted, secondsFormatted].join('');

    return str;
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

