var serviceUrl = "http://localhost:30659/ExampleService.svc/Web";

$(document).ready(function () {
    $.support.cors = true;
    $(".testButtons").click(function(event) {
        CallWebService(event.target.id);
    });
});

function CallWebService(sourceId) {
    var $button = $("#" + sourceId);
    var $dataField = $("#" + $button.data("source"));
    var $optionField = $("#" + $button.data("option"));
    var $resultField = $("#" + $button.data("destination"));
    $resultField.val("Processing...");
    var contract = $button.data("method");
    var method = sourceId.toLowerCase().indexOf("get") > -1 ? "GET" : "POST";
    var testData = sourceId.indexOf("Fibonacci") > -1
        ? {
            Number: parseInt($dataField.val()),
            UseRecursiveAlgorithm: $optionField.val() === "true"
        }
        : $dataField.val();
    var urlAppendix = sourceId.indexOf("Fibonacci") > -1
        ? "/" + $dataField.val() + "/" + $optionField.val()
        : "/" + $dataField.val().replace(/\s/g, "%20");
    var contractUrl = serviceUrl + "/" + contract + (method === "GET" ? urlAppendix : "");
    //alert("Clicked " + sourceId + " to call the " + contract + " contract.");

    try {
        $.ajax({
            url: contractUrl,
            type: method,
            async: false,
            data: method === "GET" ? null : JSON.stringify(testData),
            contentType: "application/json; charset=utf-8",
            success: function(data) {
                $resultField.val("Request: " + contractUrl + (method === "POST" ? "\n\nData: " + JSON.stringify(testData) : "") + "\n\nResult: " + data + " - (via " + method + ")");
            },
            error: function(request, status, error) {
                $resultField.val("Web Service Error:\n\nStatus: " + status + "\nError: " + error);
            }
        });
    } catch (e) {
        alert("Error calling web service:\n\n" + e.message);
    } 
}