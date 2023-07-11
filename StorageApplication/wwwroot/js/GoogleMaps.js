


///kordinatı almak için çalışan script 
// tarayıcının konumunu alır
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    } else {
        console.log("Tarayıcınızda konum hizmetleri desteklenmiyor.");
    }
}
var myLatitude = "";
var myLongitude = ";"

function showPosition(position) {
    myLatitude = position.coords.latitude;
    myLongitude = position.coords.longitude;

    // Konum bilgilerini kullanarak yapmak istediğiniz işlemleri gerçekleştirin
}

function showError(error) {
    switch (error.code) {
        case error.PERMISSION_DENIED:
            console.log("Konum erişimi reddedildi.");
            break;
        case error.POSITION_UNAVAILABLE:
            console.log("Konum bilgilerine erişilemiyor.");
            break;
        case error.TIMEOUT:
            console.log("İstek zaman aşımına uğradı.");
            break;
        case error.UNKNOWN_ERROR:
            console.log("Bilinmeyen bir hata oluştu.");
            break;
    }
}


function initMap(datacount) {
    var currentLocation = { lat: myLatitude, lng: myLongitude };
    var destinationLocations = [];
    var duplicateCoordinateChecker = [];

    if (datacount.length > 0) {
        for (var i = 0; i < datacount.length; i++) {
            if (duplicateCoordinateChecker.indexOf(datacount[i].latitude) == -1) {
                destinationLocations.push({
                    lat: datacount[i].latitude,
                    lng: datacount[i].longitude
                });
                duplicateCoordinateChecker.push(datacount[i].latitude);
            }
        }
    }

    var mapOptions = {
        center: currentLocation,
        zoom: 12
    };

    var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    // Mevcut konumu mavi daire şeklinde gösteren marker
    var currentLocationMarker = new google.maps.Marker({
        position: currentLocation,
        map: map,
        icon: {
            path: google.maps.SymbolPath.CIRCLE,
            scale: 4,
            fillColor: "blue",
            fillOpacity: 1,
            strokeColor: "blue",
            strokeOpacity: 1,
            strokeWeight: 2
        }
    });

    var directionsService = new google.maps.DirectionsService();
    var directionsDisplay = new google.maps.DirectionsRenderer();
    directionsDisplay.setMap(map);

    var waypoints = [];
    for (var i = 0; i < destinationLocations.length; i++) {
        waypoints.push({
            location: destinationLocations[i],
            stopover: true
        });
    }

    var radioValue = document.querySelector('input[name="radioOption"]:checked').value;
    var request = {
        origin: currentLocation,
        destination: destinationLocations[destinationLocations.length - 1],
        waypoints: waypoints,
        optimizeWaypoints: true,
        travelMode: radioValue == "Yaya" ? google.maps.TravelMode.WALKING : google.maps.TravelMode.DRIVING
    };
    var distanceInKm;
    var durationInHours;
    directionsService.route(request, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(result);

            var routes = result.routes;
            var distance = 0;
            var duration = 0;
            for (var i = 0; i < routes.length; i++) {
                var legs = routes[i].legs;
                for (var j = 0; j < legs.length; j++) {
                    distance += legs[j].distance.value;
                    duration += legs[j].duration.value;
                }
            }

            // Mesafeyi ve süreyi metrik birimlere dönüştür
            distanceInKm = (distance / 1000).toFixed(1);
            durationInHours = (duration / 3600).toFixed(1);


            // Harita üzerinde mesafe ve süreyi görüntülemek için bir bilgi penceresi oluştur
            var infoWindow = new google.maps.InfoWindow({
                content: "Mesafe: " + distanceInKm + " km<br>Süre: " + durationInHours + " saat"
            });

            // Bilgi penceresini mevcut konumun üzerine yerleştir
            infoWindow.setPosition(currentLocation);
            infoWindow.open(map);
        }
    });
    setTimeout(() => {
        var requestData = {
            logId: logId,
            time: distanceInKm + 'Km' + ',' + durationInHours + ' saat/dakika'
        };

        $.ajax({
            url: '/Home/LogAddDistance',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function () {
                // Başarılı olduğunda yapılacak işlemler
            }
        });
    }, 1500)
}
