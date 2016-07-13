$(document).ready(function () {
    var results = $('.notes');
    var resultCount = 0;

    $('#ShowMore').on('click', function () {
        showMoreResults(10);

        if (resultCount > results.length) {
            $('#ShowMore').hide();
        }
    })

    $('.makePubOrPri').on('click', function () {
        var $input = $(this);
        var isPrivate = ($input.parent().siblings('.isPrivate').text() == 'True')

        var id = $input.parent().siblings('.noteId').text();
        var title = $input.parent().siblings('.noteType').text();

        $('#PubOrPriText').text((isPrivate) ? "public" : "private");
        var textForButton = (isPrivate) ? "Make " + title + " public" : "Make " + title + " private";

        $('#MakePublicOrPrivate').dialog({
            modal: true,
            title: textForButton,
            width: 400,
            buttons: {
                "Change Privacy": function () {
                    if (isPrivate) {
                        $.post('/Home/MakePublic', { noteId: id }, function () {
                            $input.text("Make Private");
                            $input.parent().siblings('.isPrivate').text('False');
                        });
                    } else {
                        $.post('/Home/MakePrivate', { noteId: id }, function () {
                            $input.text("Make Public");
                            $input.parent().siblings('.isPrivate').text('True');
                        });
                    }
                    $(this).dialog("close");
                },
                Cancel: function () { $(this).dialog("close"); }
            }
        });
    });

    $('.deleteNote').on('click', function () {
        var $input = $(this);
        var id = $input.parent().siblings('.noteId').text();
        var title = $input.parent().siblings('.noteType').text();
        var $ele = $input.parent().parent();

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

    function showMoreResults(howMany) {
        resultCount += howMany;
        $.each(results, function (i, e) {
            if (i < resultCount) {
                $(e).show();
            }
        })
    };

    showMoreResults(10);
});