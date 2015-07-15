function saveEnlatado() {
    var client = new WindowsAzure.MobileServiceClient("https://gamealfa.azure-mobile.net/", "YVDcMAHRYdiquUdaQzYNOTNKcMLUFb50");
    var titleText = $("#englishWordTextText");
    var descriptionText = $("#traslatedText");
    var resumeText = $("#descriptionText");
    var enlatado = { title: titleText.val(), description: descriptionText.val(), resume: resumeText.val() };
    client.getTable("Enlatado").insert(enlatado);
    titleText.val("");
    descriptionText.val("");
    resumeText.val("");
    titleText.focus();
}
$("body").on("keypress", function (event) {
    if (event.which == 13) {
        event.preventDefault();
        saveEnlatado();
    }
});
$("#traslatedText").on("change", function () {
    changePreview(this.value);
});
function changePreview(enlatadoText) {
    var div = $("#preview"); //document.createElement('div');
    div.html(enlatadoText);
    //div.append(enlatadoText);
    //document.getElementById('content').appendChild(div);
}
function removeRow(input) {
    document.getElementById('content').removeChild(input.parentNode);
}
