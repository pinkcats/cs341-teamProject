$(document).ready(function () {
    //overall rating
    $(".adminUser").each(function () {
        var childText = $($(this).children("#score")[0]).text();
        $($(this).children(".rating")[0]).raty({ 'score': parseFloat(childText), 'readOnly': true, 'half': true });
        if ($(this).children(".userSuspended").text() == "True") {
            $(this).siblings(".adminNotesArea").addClass("hidden");
        }
    });

    //suspend user click and popup
    $(".suspendUser").click(function () {
        var $input = $(this);

        var isSuspened = ($input.siblings('.userSuspended').text() == 'True');
        var user = $input.siblings(".id").text();

        var textForModal = (isSuspened) ? "Unsuspend " + user : "Suspend " + user;
        $("#suspention").text((isSuspened) ? "unsuspend" : "suspend");
        $('#SuspendOrUnsuspendUser').dialog({
            modal: true,
            title: textForModal,
            width: 400,
            buttons:{
                "Change Suspension" : function(){
                    if(isSuspened){
                        $.post("/Admin/UnsuspendUser", {userId: user}, function(){
                            $input.html("Suspend");
                            $input.siblings(".userSuspended").text("False"); 
                            $input.parent().siblings(".adminNotesArea").removeClass("hidden");
                        });
                    } else {
                        $.post("/Admin/SuspendUser", {userId: user}, function(){
                            $input.html("Unsuspend");
                            $input.siblings(".userSuspended").text("True");
                            $input.parent().siblings(".adminNotesArea").addClass("hidden");
                        });
                    }
                    $(this).dialog("close");
                },
                Cancel: function(){$(this).dialog("close");}
            }
        })
    });

    //suspend note and popup
    $(".suspendNote").click(function () {
        var $input = $(this);

        var isSuspended = ($input.siblings(".noteSuspended").text() == "True");
        var noteId = $input.siblings(".noteId").text();
        var noteTitle = $input.siblings(".title").text();
        var textForModal = (isSuspended) ? "Unsuspend " + noteTitle : "Suspend " + noteTitle;
        $("#suspention").text((isSuspended) ? "unsuspend" : "suspend");
        $("#SuspendOrUnsuspendNote").dialog({
            modal: true,
            title: textForModal,
            width: 400,
            buttons: {
                "Change Suspension": function () {
                    if (isSuspended) {
                        $.post("/Admin/UnsuspendNote", { noteId: noteId }, function () {
                            $input.html("Suspend");
                            $input.siblings(".noteSuspended").text("False");
                        });
                    } else {
                        $.post("/Admin/SuspendNote", { noteId: noteId }, function () {
                            $input.html("Unsuspend");
                            $input.siblings(".noteSuspended").text("True");
                        });
                    }
                    $(this).dialog("close");
                },
                Cancel: function () { $(this).dialog("close");}
            }
        })
    });

});