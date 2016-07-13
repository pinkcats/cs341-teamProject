$(function () {
    $("#forgotPassForm").validate({
        onsubmit: true,
        rules: {
            email: {
                required: true,
                maxlength: 100
            },
            username: {
                required: true,
                maxlength: 100
                
            }
        },
        messages: {
            email: {
                required: "Please enter a valid email.",
                maxlength: "Entry exceeded max length."
            },
            username: {
                required: "Please enter a username.",
                maxlength: "Entry exceeded max length."
            }
        }
    });
});