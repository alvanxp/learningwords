function saveWord() {
    var wordModel = {
        Word: $("#englishWordText").val(),
        Language: 'EN',
        Description: $("#englishDescriptionText").val(),
        ToWord: $("#traslatedText").val(),
        ToLanguage: 'ES',
        ToDescription: $("#translatedDescriptionText").val()
    };

    
    $.ajax({
        url: 'api/words/',
        type: 'post',
        dataType: 'json',
        success: function(data) {
            alert('guardado');
            clear();
        },
        data: wordModel
    });
}

function clear() {
    $("#englishWordText").val('');
    $("#englishDescriptionText").val('');
    $("#traslatedText").val('');
    $("#translatedDescriptionText").val('');
}

$("body").on("keypress", function (event) {
    if (event.which == 13) {
        event.preventDefault();
        saveWord();
    }
});
//$("#traslatedText").on("change", function () {
//    changePreview(this.value);
//});
//function changePreview(enlatadoText) {
//    var div = $("#preview"); //document.createElement('div');
//    div.html(enlatadoText);
//    //div.append(enlatadoText);
//    //document.getElementById('content').appendChild(div);
//}
//function removeRow(input) {
//    document.getElementById('content').removeChild(input.parentNode);
//}
