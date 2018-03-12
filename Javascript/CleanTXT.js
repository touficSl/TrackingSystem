$(document).ready(function () {

    $('input[type=text], textarea').each(function () {

        //        $(this).keyup(function (e) {
        //            if (e.which !== 0) {
        //                if ($(this).val().indexOf(getSelectedText()) != -1 && getSelectedText() != "" ) {
        //                    $(this).val().replace(getSelectedText(), "");
        //                } else {
        //                    var cleanedValue = $(this).val().replace(/<([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace(/([a-z])>/i, "");
        //                    cleanedValue = cleanedValue.replace(/>([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace(/([a-z])</i, "");
        //                    cleanedValue = cleanedValue.replace(/<\s+([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace(/>\s+([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace("<>", "");
        //                    cleanedValue = cleanedValue.replace("/>", "");
        //                    cleanedValue = cleanedValue.replace("</", "");
        //                    cleanedValue = cleanedValue.replace("<<", "");
        //                    cleanedValue = cleanedValue.replace(">>", "");
        //                    $(this).val(cleanedValue);
        //                }

        //            }
        //        });




        $(this).change(function () {
            var cleanedValue = $(this).val().replace(/<([a-zA-Z])/i, "");
            cleanedValue = cleanedValue.replace(/([a-zA-Z])>/g, "");
            cleanedValue = cleanedValue.replace(/>([a-zA-Z])/g, "");
            cleanedValue = cleanedValue.replace(/([a-zA-Z])</g, "");
            cleanedValue = cleanedValue.replace(/<\s+([a-zA-Z])/g, "");
            cleanedValue = cleanedValue.replace(/>\s+([a-zA-Z])/g, "");
            cleanedValue = cleanedValue.replace(/<>/g, "");
            cleanedValue = cleanedValue.replace(/\/>/g, "");
            cleanedValue = cleanedValue.replace(/<\//g, "");
            cleanedValue = cleanedValue.replace(/<</g, "");
            cleanedValue = cleanedValue.replace(/>>/g, "");
            $(this).val(cleanedValue);


        });

    });
});

var pbControl = null;
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_beginRequest(BeginRequestHandler);
prm.add_endRequest(EndRequestHandler);
function BeginRequestHandler(sender, args) {

}
function EndRequestHandler(sender, args) {
    $('input[type=text], textarea').each(function () {
        //        $(this).keyup(function (e) {
        //            if (e.which !== 0) {
        //                if ($(this).val().indexOf(getSelectedText()) != -1 && getSelectedText() != "") {
        //                    $(this).val().replace(getSelectedText(), "");
        //                } else {
        //                    var cleanedValue = $(this).val().replace(/<([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace(/([a-z])>/i, "");
        //                    cleanedValue = cleanedValue.replace(/>([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace(/([a-z])</i, "");
        //                    cleanedValue = cleanedValue.replace(/<\s+([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace(/>\s+([a-z])/i, "");
        //                    cleanedValue = cleanedValue.replace("<>", "");
        //                    cleanedValue = cleanedValue.replace("/>", "");
        //                    cleanedValue = cleanedValue.replace("</", "");
        //                    cleanedValue = cleanedValue.replace("<<", "");
        //                    cleanedValue = cleanedValue.replace(">>", "");
        //                    $(this).val(cleanedValue);
        //                }

        //            }
        //        });

        $(this).change(function () {
            var cleanedValue = $(this).val().replace(/<(\D)/i, "");
            cleanedValue = cleanedValue.replace(/([a-zA-Z])>/g, "");
            cleanedValue = cleanedValue.replace(/>([a-zA-Z])/g, "");
            cleanedValue = cleanedValue.replace(/([a-zA-Z])</g, "");
            cleanedValue = cleanedValue.replace(/<\s+([a-zA-Z])/g, "");
            cleanedValue = cleanedValue.replace(/>\s+([a-zA-Z])/g, "");
            cleanedValue = cleanedValue.replace(/<>/g, "");
            cleanedValue = cleanedValue.replace(/\/>/g, "");
            cleanedValue = cleanedValue.replace(/<\//g, "");
            cleanedValue = cleanedValue.replace(/<</g, "");
            cleanedValue = cleanedValue.replace(/>>/g, "");
            $(this).val(cleanedValue);


        });

    });

}



function getSelectedText() {
    var text = "";
    if (typeof window.getSelection != "undefined") {
        text = window.getSelection().toString();
    } else if (typeof document.selection != "undefined" && document.selection.type == "Text") {
        text = document.selection.createRange().text;
    }
    return text;
}
