/// <reference path="jquery-3.1.1.js" />
//var serviceUrl = 'http://localhost:58046/api/Properties';
var serviceUrl = './api/Properties'

function sendRequest() {
    //   jQuery.support.cors = true;
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
    $("#value1").replaceWith("<dl  id='properties' />");
    //var str =  "propertyTitle: " + val.title  + "propertySummary: " + val.summary + " Nummer Bedrooms: " + val.numBedrooms + " Nummer Washrooms: " + val.numWashrooms + " Kitchen: " + val.kitchen + val.smokingAllowed + " max Number Guests" + val.maxNumberGuests + " Dimensions:" + val.dimensions;
    var str = "propertyTitle: " + val.title
    var strSummary = "propertySummary: " + val.summary
    var strBedroom = " Nummer Bedrooms: " + val.numBedrooms
    var strWashroom = val.numBedrooms + " Nummer Washrooms: " + val.numWashrooms
    var strKitchen = " Kitchen: " + val.kitchen
    var strSmoking = "smokingAllowed" + val.smokingAllowed
    var strMaxGuests = "max Number Guests" + val.maxNumberGuests
    var strDimensions = " Dimensions:" + val.dimensions
    $('<div>').appendTo($('#properties'));
    $('<li/>', { text: str }).appendTo($('#properties'));
    $('<li/>', { text: strSummary }).appendTo($('#properties'));
    $('<li/>', { text: strBedroom }).appendTo($('#properties'));
    $('<li/>', { text: strWashroom }).appendTo($('#properties'));
    $('<li/>', { text: strKitchen }).appendTo($('#properties'));
    $('<li/>', { text: strSmoking }).appendTo($('#properties'));
    $('<li/>', { text: strMaxGuests }).appendTo($('#properties'));
    $('<li/>', { text: strDimensions }).appendTo($('#properties'));
    $('<li/>').text("<a href='/Home/Details/" + data.Id + "'" + data.title + "</a>");
    $('<br/>').appendTo($('#properties'));
    $('<div/>').appendTo($('#properties'));
}
