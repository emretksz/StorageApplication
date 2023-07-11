
$(function () {
    $('#model').val("arazi");
});
var formDatas = new FormData();
var infer = function () {

    return new Promise(function (resolve, reject) {
        $('#output').html("Hesaplanıyor...");
      /*  $("#resultContainer").show();*/
    /*    $('html').scrollTop(100000);*/
        getSettingsFromForm(function (settings) {
            settings.error = function (xhr) {
                $('#output').html("").append([
                    "Error loading response.",
                    "",
                    "Check your API key, model, version,",
                    "and other parameters",
                    "then try again."
                ].join("\n"));
            };

            $.ajax(settings).then(function (response) {
                if (settings.format == "json") {
                    var pretty = $('<pre>');
                    var formatted = JSON.stringify(response, null, 4)
                    pretty.html(formatted);
                    formDatas.append("jsonData", formatted);
                    console.log("datasss", formDatas);
                    $('#output').html("");
                    resolve(formatted); ;
                    //$('#output').html("").append(pretty);
                    //$('html').scrollTop(100000);
                } else {
                    reject("xx");
                }
            });
        });
             
       
    });


    
};
document.getElementById("ImageForm").addEventListener("submit", async function (event) {
    event.preventDefault(); // Formun otomatik olarak gönderilmesini engeller

    infer().then(function (xx) {

        var fileInput = document.getElementById('file');


        for (var i = 0; i < fileInput.files.length; i++) {
            formDatas.append('images', fileInput.files[i]);
        }


        fetch("/Home/AddPicture", {
            method: "POST",
            body: formDatas
        })
            .then(response => response.json())
            .then(data => {
                console.log("kontrol",data);
                // İşlem tamamlandığında yapılacak işlemler
                var label = "";
                label += "Ulaşım Durumu:" + " " + data.state.name+"-"+data.type.name +"/" +"Ulaşım kolaylığı:"+" "+ data.type.oran + "%" ;
                swal({
                    title: "Uyarı",
                    text: label,
                    icon: "info",
                    buttons: ["İptal", "Devam Et"],
                }).then(function (resultValue) {
                    console.log(resultValue);
                    if (resultValue) {
                        SendForm();
                    }
                    else {
                        swal({
                            title: "İptal Edildi",
                            icon: "info",
                            button:"Tamam"
                        })
                    }
                })
                //if (data.property != null) {
                //    //if (data.property.height != null) {
                //    //    label += "Yüksekliği: " + data.property.height + ",";
                //    //}
                //    //if (data.property.length != null) {
                //    //    label += "Uzunluğu: " + data.property.length + ",";
                //    //}
                //    //if (data.property.width != null) {
                //    //    label += "Genişliği: " + data.property.width;
                //    //}
                //}
                // SendForm();

                $('#yazdir').html(label);

            })
            .catch(error => {
                // Hata durumunda yapılacak işlemler
                console.error(error);
            });

        return false;
    });
});
//async function sonuc() {
  
    

//    });


  
//}

var getSettingsFromForm = function (cb) {
    var settings = {
        method: "POST",
    };

    var parts = [
        "https://detect.roboflow.com/",
        "arazi",
        "/",
        1,
        "?api_key=" + ""
    ];


    parts.push("&confidence=" + 50);
    parts.push("&overlap=" + 50);
    parts.push("&format=" + "json");
    settings.format = "json";

    var file = $('#file').get(0).files && $('#file').get(0).files.item(0);
    if (!file) return alert("Please select a file.");

    getBase64fromFile(file).then(function (base64image) {
        settings.url = parts.join("");
        settings.data = base64image;

        console.log(settings);
        cb(settings);
    });

};

var getBase64fromFile = function (file) {
    return new Promise(function (resolve, reject) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            resizeImage(reader.result).then(function (resizedImage) {
                resolve(resizedImage);
            });
        };
        reader.onerror = function (error) {
            reject(error);
        };
    });
};

var resizeImage = function (base64Str) {
    return new Promise(function (resolve, reject) {
        var img = new Image();
        img.src = base64Str;
        img.onload = function () {
            var canvas = document.createElement("canvas");
            var MAX_WIDTH = 1500;
            var MAX_HEIGHT = 1500;
            var width = img.width;
            var height = img.height;
            if (width > height) {
                if (width > MAX_WIDTH) {
                    height *= MAX_WIDTH / width;
                    width = MAX_WIDTH;
                }
            } else {
                if (height > MAX_HEIGHT) {
                    width *= MAX_HEIGHT / height;
                    height = MAX_HEIGHT;
                }
            }
            canvas.width = width;
            canvas.height = height;
            var ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0, width, height);
            resolve(canvas.toDataURL('image/jpeg', 1.0));
        };

    });
};
