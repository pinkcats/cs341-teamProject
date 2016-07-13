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

$(document).ready(function () {
    var currentUni = $('#universityId').val();
    for (var i = 0; i < universityNames.length; i++) {
        if (universityNames[i].key == parseInt(currentUni)) {
            $('#autocomplete').val(universityNames[i].value);
            break;
        }
    }
});

//validate password change
$(function () {    
    $('#changePasswordForm').validate({
        onsubmit: true,
        rules: {
            oldPassword: {
                required: true
            },
            password: {
                minlength: 8
            },
            passwordConfirm: {
                equalTo: '#passwordConfirm'
            }
        },
        messages: {
            oldPassword: {
                required: "Enter current pass."
            },
            password: {
                minlength: "New pass must have at least 8 chars."
            },
            passwordConfirm: {
                equalTo: "Passwords must match."
            }
        }
    });

    $('#updateInformationForm').validate({
        onsubmit: true,
        rules: {
            university: {
                required: true
            },
            email: {
                required: true,
                maxlength: 100
            }
        },
        messages: {
            university: {
                required: "Please select university,"
            },
            email: {
                required: "Please enter email.",
                maxlength: "Email must be < 100 chars."
            }
        }
    });

});