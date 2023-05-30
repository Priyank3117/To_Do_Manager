$(document).ready(function () {
    $("#oldPassword").focusout(function () {
        if ($('#oldPassword').val() == '') {
            $("#oldPasswordSpan").html('Old password is required');
            $(".changePasswordButton").attr("disabled", true);
        }
        else if ($('#oldPassword').val().length < 8) {
            $("#oldPasswordSpan").html('Minimum length is 8 character');
            $(".changePasswordButton").attr("disabled", true);
        }
        else {
            $("#oldPasswordSpan").html('');
            $(".changePasswordButton").removeAttr("disabled");
        }
    })

    $("#newPassword").focusout(function () {
        if ($('#newPassword').val() == '') {
            $("#newPasswordSpan").html('New password is required');
            $(".changePasswordButton").attr("disabled", true);
        }
        else if ($('#newPassword').val().length < 8) {
            $("#newPasswordSpan").html('Minimum length is 8 character');
            $(".changePasswordButton").attr("disabled", true);
        }
        else if (!$('#newPassword').val().toString().includes("@")) {
            $("#newPasswordSpan").html('Enter Strong password');
            $(".changePasswordButton").attr("disabled", true);
        }
        else if ($('#newPassword').val() == $('#oldPassword').val()) {
            $("#newPasswordSpan").html('Old password and New password can not be same');
            $(".changePasswordButton").attr("disabled", true);
        }
        else {
            $("#newPasswordSpan").html('');
            $(".changePasswordButton").removeAttr("disabled");
        }
    })

    $("#confirmPassword").focusout(function () {
        if ($('#confirmPassword').val() == '') {
            $("#confirmPasswordSpan").html('Confirm password is required');
            $(".changePasswordButton").attr("disabled", true);
        }
        else if ($('#confirmPassword').val().length < 8) {
            $("#confirmPasswordSpan").html('Minimum length is 8 character');
            $(".changePasswordButton").attr("disabled", true);
        }
        else if ($('#confirmPassword').val() != $('#newPassword').val()) {
            $("#confirmPasswordSpan").html('Password does not matched');
            $(".changePasswordButton").attr("disabled", true);
        }
        else {
            $("#confirmPasswordSpan").html('');
            $(".changePasswordButton").removeAttr("disabled");
        }
    })
});

// Change Password
function ChangePassword() {
    var oldPassword = $('#oldPassword').val();
    var newPassword = $('#newPassword').val();
    if ($('#oldPassword').val() == '') {
        $("#oldPasswordSpan").html('Old password is required');
    }
    else if ($('#oldPassword').val().length < 8) {
        $("#oldPasswordSpan").html('Minimum length is 8 character');
    }
    else if ($('#newPassword').val() == '') {
        $("#newPasswordSpan").html('New password is required');
    }
    else if ($('#newPassword').val().length < 8) {
        $("#newPasswordSpan").html('Minimum length is 8 character');
    }
    else if (!$('#newPassword').val().toString().includes("@")) {
        $("#newPasswordSpan").html('Enter Strong password');
    }
    else if ($('#confirmPassword').val() == '') {
        $("#confirmPasswordSpan").html('Confirm password is required');
    }
    else if ($('#confirmPassword').val().length < 8) {
        $("#confirmPasswordSpan").html('Minimum length is 8 character');
    }
    else if ($('#newPassword').val() == $('#oldPassword').val()) {
        $("#newPasswordSpan").html('Old password and New password can not be same');
        $(".changePasswordButton").attr("disabled", true);
    }
    else {
        $.ajax({
            type: "POST",
            url: "/UserProfile/ChangePassword",
            data: { oldPassword: oldPassword, newPassword: newPassword },
            success: function (result) {
                if (result == "Changed") {
                    $(".changePasswordButton").html("Password Changed")
                }
                if (result == "Enter Valid Old Password") {
                    $("#oldPasswordSpan").html('Enter Valid Old Password');
                }
            }
        });
    }

}

// Profile Image Preview
var loadFile = function (event) {
    var image = document.getElementById("profileImage");
    const imageFiles = event.target.files[0];
    var imageURL = "/images/" + event.target.files[0].name;
    console.log(imageFiles)
    $.ajax({
        type: "POST",
        url: "/UserProfile/ChangeImage",
        data: { imageURL: imageURL },
        success: function (result) {
            if (result == "Changed") {
                image.src = URL.createObjectURL(event.target.files[0]);
            }
        }
    });
};