var map;
var poly;
var continers = [];
var lastFlight;
var flag = 0;

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
function initMap() {
    var temp = document.getElementById('map');
    temp.onclick = function () { removeLastFlight(); };
    var options = {
        zoom: 2,
        center: { lat: 32.4187, lng: 34.647 }
    }
    lastFlight = null;
    map = new google.maps.Map(document.getElementById('map'), options);
    getAllFlightPlans();
    setInterval(function () { getAllFlightPlans(); }, 5000);
}

function removeLastFlight() {
    if (!lastFlight) {
        return;
    }
    poly.setMap(null);
    var textBox = document.getElementById('tdCompany_name');
    if (textBox) {
        textBox.innerHTML = "";
    }
    textBox = document.getElementById('tdpassengers');
    if (textBox) {
        textBox.innerHTML = "";
    }
    textBox = document.getElementById('tdTImeOfFlight');
    if (textBox) {
        textBox.innerHTML = "";
    }    for (index in continers) {
        if (continers[index].id == lastFlight) {
            var div = continers[index].divOfButton.div;
            div.setAttribute("style", "");
            var mar = continers[index].marker;
            var icon1 = {
                url: 'aeroplane.png',
                size: new google.maps.Size(50, 50),
                scaledSize: new google.maps.Size(50, 50),
            };
            mar.setIcon(icon1);
        }
    }
    lastFlight = null;


}

function addMarker(props) {
    //  { lat: 32.5117, lng: 34.5005 }
    console.log(props.coord);
    var marker = new google.maps.Marker({
        position: props.coord,
        map: map
    });
    var icon1 = {
        url: 'aeroplane.png',
        size: new google.maps.Size(50, 50),
        scaledSize: new google.maps.Size(50, 50),
    };
    marker.setIcon(icon1);
    /*
    if (props.func) {
        marker.addListener('click', func);
    }*/
    return marker;
   
}
function makeFlightButton(id, company_name) {
    var tr = document.createElement("tr");
    var tdId = document.createElement("td");
    var text = document.createTextNode(id);
    var buttonShow = document.createElement("button");
    buttonShow.setAttribute("class", "btn btn-default");
    buttonShow.onclick = function () { flightIsPress(id); };
    buttonShow.appendChild(text);
    tdId.appendChild(buttonShow);
    tr.appendChild(tdId);

    var tdCompany = document.createElement("td");
    var text = document.createTextNode(company_name);
    var buttonShow = document.createElement("button");
    buttonShow.setAttribute("class", "btn btn-default");
    buttonShow.onclick = function () { flightIsPress(id); };
    buttonShow.appendChild(text);
    tdCompany.appendChild(buttonShow);
    tr.appendChild(tdCompany);

    var tdDelete = document.createElement("td");
    var deletebutton = document.createElement("button");
    deletebutton.setAttribute("class", "btn btn-default");
    deletebutton.onclick = function () { deleteFlight(id); };
    var icon = document.createElement("i");
    icon.setAttribute("class", "glyphicon glyphicon-remove");
    deletebutton.appendChild(icon);
    tdDelete.appendChild(deletebutton);
    tr.appendChild(tdDelete);
    var list = document.getElementById('listOfMyFlight');
    list.appendChild(tr);
    return { div: tr, button: buttonShow, deleteButton: null };}
function makeExternalFlightButton(id, company_name){
    
    var tr = document.createElement("tr");
    var tdId = document.createElement("td");
    var text = document.createTextNode(id);
    var buttonShow = document.createElement("button");
    buttonShow.setAttribute("class", "btn btn-default");
   // buttonShow.setAttribute('onclick', "flightIsPress(" + id + ")");
    buttonShow.onclick = function () { flightIsPress(id); };
    buttonShow.appendChild(text);
    tdId.appendChild(buttonShow);
    tr.appendChild(tdId);

    var tdCompany = document.createElement("td");
    var text = document.createTextNode(company_name);
    var buttonShow = document.createElement("button");
    buttonShow.setAttribute("class", "btn btn-default");
   // buttonShow.setAttribute('onclick', "flightIsPress(" + id + ")");
    buttonShow.onclick = function () { flightIsPress(id); };
    buttonShow.appendChild(text);
    tdCompany.appendChild(buttonShow);
    tr.appendChild(tdCompany);

    var list = document.getElementById('listOfExternalFlight');
    list.appendChild(tr);
        
    return { div: tr, button: buttonShow, deleteButton: null };
}


function myFlightControler(value) {
    var divOfButton = makeFlightButton(value.flight_id, value.company_name);
    var marker = addMarker({ coord: { lat: value.latitude, lng: value.longitude } });
    continers.push({ id: value.flight_id, divOfButton: divOfButton, marker: marker, is_external: false })
    marker.addListener('click', async function () {
        await sleep(100);
        flightIsPress(value.flight_id);
    })
   
}
function externalFlightControler(value) {
    var divOfButton = makeExternalFlightButton(value.flight_id, value.company_name);
    var marker = addMarker({ coord: { lat: value.latitude, lng: value.longitude } });
    continers.push({ id: value.flight_id, divOfButton: divOfButton, marker: marker, is_external: true })
    marker.addListener('click', async function () {
        await sleep(100);
        flightIsPress(value.flight_id);
    })
}



function showFlight(data) {
    //// we need to convert the data from json to var
    var seg = data.segments;
    if (!seg) {
        console.log("error in data, data contins:"+data);
        return;
    }
    if (poly) {
        poly.setMap(null);
    }
    poly = new google.maps.Polyline({
        strokeColor: '#15ECCF',
        strokeOpacity: 2.0,
        strokeWeight: 30
    });
    poly.setMap(map);
    var path = poly.getPath();
    path.push(new google.maps.LatLng(data.initial_location.latitude, data.initial_location.longitude));
    for (segmen in seg) {
        path.push(new google.maps.LatLng(seg[segmen].latitude, seg[segmen].longitude ));
        
    }

}
function showFlightPlan(data) {
    /*var textBox = document.getElementById('textBoxFlight_Details');
    var tr = document.createElement("tr");
    tr.setAttribute("id", "trFlightPlan");*/
    var company = document.getElementById("tdCompany_name");
    var text = document.createTextNode(data.company_name);
    
    company.appendChild(text);

    var passenger = document.getElementById("tdpassengers");
    var text = document.createTextNode(data.passengers);
    
    passenger.appendChild(text);

    var location = document.getElementById("tdTImeOfFlight");
    var text = document.createTextNode(data.initial_location.date_time);
    
    location.appendChild(text);
    }
function flightIsPress(flight_id) {
    if (lastFlight) {
        removeLastFlight();
    }
    lastFlight = flight_id;
    getFlightPlan(flight_id);
    var icon1 = {
        url: 'airplane1.png',
        size: new google.maps.Size(50, 50),
        scaledSize: new google.maps.Size(50, 50),
    };
    for (index in continers) {
        if (continers[index].id == flight_id) {
            var div = continers[index].divOfButton.div;
            div.setAttribute("style", "background-color:#71F8FD");
            var mar = continers[index].marker;
            var temp =  new google.maps.Marker({
                position: mar.getPosition(),
                map: map
            });
            temp.addListener('click', async function () {
                await sleep(100);
                flightIsPress(flight_id);
            })
            mar.setMap(null);
            temp.setIcon(icon1);
            continers[index].marker = temp;
            break;
            console.log("the image is changed");
        }
    }

}



function getFlightPlans() {
    var key = "/api/Flights?relative_to";
    var l = new Date();
    var value = l.toISOString();
    key = key + "=" + value;
    key = encodeURI(key);
    $.ajax({
        url: key,
        type: 'GET',
        dataType: "json",
        success: function (data) {
            //console.log(data);
            var list = document.getElementById('listOfMyFlight');
            list.innerHTML = '';
            for (index in continers) {
                if (!continers[index].is_external) {
                    var marker = continers[index].marker;
                    marker.setMap(null);
                }
            }
            continers = [];
            $.each(data, function (index, value) {
                //console.log(value);
                if (value) {
                    myFlightControler(value);
                    if (value.flight_id == lastFlight) {
                        flightIsPress(lastFlight);
                    }
                }
            });
        }
    });
}
function getAllFlightPlans() {
    var key = "/api/Flights?relative_to";
    var l = new Date();
    var value = l.toISOString();
    key = key + "=" + value + "&sync_all";
    key = encodeURI(key);
    $.ajax({
        url: key,
        type: 'GET',
        dataType: "json",
        success: function (data) {
            var list = document.getElementById('listOfMyFlight');
            list.innerHTML = '';
            list = document.getElementById('listOfExternalFlight');
            list.innerHTML = '';
            for (index in continers) {
                var marker = continers[index].marker;
                marker.setMap(null);
            }
            continers = [];
            $.each(data, function (index, value) {
                //console.log(value);
                if (value) {
                    if (!value.is_external) {
                        myFlightControler(value);
                    } else {
                        externalFlightControler(value);
                    }
                    if (value.flight_id === lastFlight) {
                        flightIsPress(lastFlight);
                    }
                }
            });
        }
    })
}
function postFlightPlan(jsonPlan) {
    var key = "/api/FlightPlan";
    $.ajax({
        url: key,
        type: 'POST',
        data: JSON.stringify({flightPlan:jsonPlan }),
        contentType: 'application/json',
       
        success: function (result) {
            getAllFlightPlans();
        }
    })
    /*$.post(key, jsonPlan, function (data) {
        
    });*/
}
function getFlightPlan(id) {
    var key = "/api/FlightPlan/" + id;
    $.ajax({
        dataType: 'json',
        url: key,
        type: 'GET',
        success: function (result) {
            showFlight(result);
            showFlightPlan(result);
        }
    });
    /*$.getJSON(key, function (data) {
        //console.log("this is the data from get flight plan\n" + data.segments[0].latitude + "this is campany name:" + data.company_name);
        showFlight(data);
        showFlightPlan(data);

    });
    */
}
function deleteFlight(id) {
    var temp = "/api/Flights/" + id;
    $.ajax({
        url: temp,
        type: 'DELETE',
        success: function (result) {
            getAllFlightPlans();
        }
    });
    //clean the polyline from the map.
    for (i in continers) {
        if (continers[i].id == id) {
            var tempMarker = continers[i].marker;
            tempMarker.setMap(null);
            break;
        }
    }
    // if the last flight that click is the flight we want to delete.
    if (id == lastFlight) {
        poly.setMap(null);
        var textBox = document.getElementById('trFlightPlan');
        if (textBox) {
            textBox.remove();
        }
    }
    
   
}

function drop(ev) {
    document.getElementById("dragAndDrop").style.display = "none";
    document.getElementById("tableOfMyFlight").style.display = "inline";
      
    ev.preventDefault();
   // var data = ev.dataTransfer.getData("text");
    if (ev.dataTransfer.items[0].kind === 'file') {
        //postFlightPlan(ev.dataTransfer.items[0].getAsFile());
        
        var file = ev.dataTransfer.items[0].getAsFile();
        console.log(file);
        console.log("after convert");
        var key = "../api/FlightPlan";
        var xhr = new XMLHttpRequest();
        xhr.open("POST", key, true);
        xhr.setRequestHeader("Content-Type", "application/json");
        //console.log(JSON.stringify(file));
        xhr.send(file);
    }
    //postFlightPlan(data);
}
function allowDrop(ev) {
    document.getElementById("tableOfMyFlight").style.display = "none";
    document.getElementById("dragAndDrop").style.display = "inline";
    ev.preventDefault();
}

function addFlight() {
    //var para = document.createElement("li");
    for (var i = 0; i < 5; i++) {
        var temp = document.createElement("button");
        temp.setAttribute("id", "temperery" + i);
        temp.setAttribute("onclick", "addFlight()");
        var node = document.createTextNode("This is new." + i);
        temp.appendChild(node);
        var inline = document.createElement("button");
        inline.setAttribute("id", "inline" + i);
        inline.setAttribute("onclick", "deleteFlight()");
        inline.setAttribute("style", "{flout: right,width: 30%}");
        temp.appendChild(inline);
        var element = document.getElementById("listOfMyFlight");
        element.appendChild(temp);
    }


}

