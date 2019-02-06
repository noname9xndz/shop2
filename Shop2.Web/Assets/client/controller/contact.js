
// cấu hình để gọi api gg map,setting tìm trên gg map api

var contact = {

    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {
        contact.initMap();
    },
    initMap: function ()
    {
        var uluru = {
            lat: parseFloat($('#hidLat').val()), // lấy kinh độ
            lng: parseFloat($('#hidLng').val())  // lấy vĩ độ
        };
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 17,
            center: uluru
        });

        var contentString = $('#hidAddress').val();

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: uluru,
            map: map,
            title: $('#hidName').val()
        });
        infowindow.open(map, marker);
    }

}

contact.init();