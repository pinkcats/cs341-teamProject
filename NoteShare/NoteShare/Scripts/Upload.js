//form validation
$(function () {
    $("#uploadForm").validate({
        onsubmit: true,
        rules: {
            topic: {
                required: true
            },
            professor: {
                required: true
            },
            courseName: {
                required: true
            },
            courseNumber: {
                required: true,
                maxlength: 11
            },
            title: {
                required: true,
                maxlength: 255
            },
            file: {
                required: true,
                maxlength: 100
            },
            noteText:{
                maxlength:2000
            }
        },
        messages: {
            topic: {
                required: "Please enter a topic."
            },
            professor: {
                required: "Please enter a name."
            },
            courseName: {
                required: "Please enter a name."
            },
            courseNumber: {
                required: "Please enter course #.",
                maxlength: "Course # must be <11 chars."
            },
            title: {
                equalTo: "Passwords must match.",
                maxlength: "Title must be <255 chars."
            },
            file:{
                required: "Select a file.",
                maxlength: "File name must be <100 chars."
            },
            noteText: {
                maxlength: "Description must be <2000 chars."
            }

        }
    });
});