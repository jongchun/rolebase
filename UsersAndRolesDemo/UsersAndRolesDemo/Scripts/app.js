var serviceUrl = './api/_OwnerPropertyContoller';

function sendRequest() {
    $("#properties").replaceWith("<span id='value1'></span>");
    var method = $('#method').val();
    $.ajax({
        type: method,
        url: serviceUrl
    }).done(function (data) {
        data.forEach(function (val) {
            callback(val)
        });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $('#value1').text(jqXHR.responseText || textStatus);
    });
}
function callback(val) {
    $("#value1").replaceWith("<ul id='properties' />");
    var str = "Property: " + val.summary + " PropertyType: " + val.propertyType + " UserId: " + val.UserId + " Nummer Bedrooms: " + val.numBedrooms + " Nummer Washrooms: " + val.numWashrooms + " Kitchen: " + val.kitchen + " Baserate: " + val.baseRate + " Address: " + val.address + " Built Year: " + val.builtYear + " Smoking Allowed: " + val.smokingAllowed + " max Number Guests" + val.maxNumberGuests + "Available Dates: " + val.availableDates + " Dimensions:" + val.dimensions;
    $('<li/>', { text: str }).appendTo($('#properties'));
}

// Deletes and refreshes list.
function updateList() {
    $("#properties").replaceWith("<span id='value1'>(Result)<br /></span>");
    sendRequest();
}
function find() {
    var id = $('#propertyIdFind').val();
    $.getJSON(serviceUrl,
        function (data) {
            if (data == null) {
                $('#propertyFind').text('Property not found.');
            }
            //var str = data.summary + ': ' + data.propertyType + ", " + data.UserId + ", " + data.numBedrooms + ", " + data.numWashrooms + ", " + data.kitchen + ", " + data.baseRate + ", " + data.builtYear + ", " + data.smokingAllowed + ", " + data.maxNumberGuests + ", " + data.availableDates + ", " + data.dimensions + '%';
            $('#propertytitle').text("<a href='/Home/Details/" + data.Id + "'" + data.title + "</a>");
            $('#propertysummary').text(data.summary);
        })
    .fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#propertyFind').text('Find error: ' + err);
        });
}