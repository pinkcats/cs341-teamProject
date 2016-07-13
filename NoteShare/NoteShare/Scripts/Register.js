$(function () {
    $("input#autocomplete").autocomplete({
        source: universityNames,
        minLength: 5,
        change: function (event, ui) {
            //this code implements mustmatch that prevents user from entering a university not in our repository//
            //This code must be in front of the code to put the id in the hidden window//
            var source = $(this).val();
            var found = $('.ui-autocomplete li').text().search(source);
            if (found < 0) {
                $(this).val('');
            }
            //this part puts the id in the hidden 
            $(this).val(ui.item.value);
            $('#universityId').val(ui.item.key);
        }
    });
});
//form validation
$(function () {
$("#registerForm").validate({
        onsubmit: true,
    rules: {
            email: {
                required:true
            },
            universityName: {
                required: true
            },
            username: {
                required: true
            },
            password: {
                minlength: 8
            },
        passwordConfirm: {
                equalTo: "#PasswordConfirm"
        }
        },
        messages: {
            email: {
                required: "Please enter a valid email."
            },
            universityName: {
                required: "Please select a university."
            },
            username: {
                required: "Please enter a username."
            },
            password: {
                minlength: "Must contain at least 8 characters"
            },
            passwordConfirm: {
                equalTo: "Passwords must match."
    }

        }
    });
});