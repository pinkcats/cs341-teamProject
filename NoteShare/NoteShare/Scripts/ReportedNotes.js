$(document).ready(function () {
    $('.deleteNote').on('click', function () {
        var $input = $(this);
        var id = $input.parent().siblings('.noteId').text();
        var title = $input.parent().siblings('.noteType').text();
        var $ele = $input.parent().parent().parent();

        $('#DeleteNote').dialog({
            modal: true,
            title: "Delete " + title,
            width: 400,
            buttons: {
                "Delete Note": function () {
                    $.post('/Home/DeleteNote', { noteId: id }, function () {
                        $ele[0].outerHTML = "";
                    });
                    $(this).dialog("close");
                },
                Cancel: function () { $(this).dialog("close"); }
            }
        });
    });
});