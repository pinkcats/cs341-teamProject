
$(document).ready(function () {
    var results = [];
    var rate;

    $("#closeComments").click(function () {
        $(".Comments").animate({ width: 'toggle' }, "slow");
        $("#commentError").text("");
    });

    $("#commentbtn").click(function () {
        $(".Comments").animate({ width: 'toggle' }, "slow");
    });

    $('#reportbtn').on('click', function () {
        $('#ReportDialog').dialog({
            modal: true,
            title: "Report note",
            width: 400,
            buttons: {
                "Report note": function () {
                    $.post('/Home/ReportNote', { model: { noteId: $("#noteId").text(), text: $('#ReportDetails').val() } }, function () {
                        $('#ReportDetails').val('');
                        $('#ReportDialog').dialog("close");
                    });
                },
                Cancel: function () { $('#ReportDetails').val(''); $(this).dialog("close"); }
            }
        });
    });

    $("#rating").raty();


    $("#addComment").click(function () {
        $("#commentError").text("");
        var username = $("#userId").text();
        var message = $(".commentText").val();
        rate = $("#rating").raty('score');
        if (message == '' || message == null || rate == ''|| rate == null) {
            $("#commentError").text("You must have a rating and a comment");
        } else if (message.length > 2000) {
            $("#commentError").text("Comment length must be less than 2000 chars.");//constrain input to match container size
        } else {
            var model = {
                NoteId: $("#noteId").text(),
                Rating: rate,
                Message: message
            }
            $.post('/Home/AddComment', {
                model: model
            });
            $(".commentArea").append("<div class='commentsSection'><div class='comment'> <div id='comMessage'> " + message
                + " </div><div id='userCom'>   By " + username
                + " </div></div></div>");
            $("#rating").raty({ score: 0 });
            $("#commentText").val(" ");
        }
    });

    $(".deleteComment").click(function () {
        console.log("hlep");
        var $input = $(this);
        var area = $input.parent().parent();
        area.addClass("hidden");
        if ($("#comId").text() != null) {
            var id = $("#comId").text();
            $.post('/Home/DeleteComment', {
                id: id
            });
        } else {
            alert("Sorry, cannot delete comment at this time.");
        }
    });
});
