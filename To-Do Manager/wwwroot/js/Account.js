function sendOTP() {
    if ($("#Email").val() == '') {
        $("#EmailSpan").html("Email is required")
    }
    else if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{1,}$/.test($("#Email").val())) {
        $("#EmailSpan").html("Enter valid email")
    }
    else {
        $("#EmailSpan").html("")
        var forgotPassword = {
            Email: $("#Email").val()
        }

        var isOTPSent = false

        $.ajax({
            url: "/Account/SendOTP",
            type: "POST",
            data: { forgotPassword: forgotPassword },
            beforeSend: function () {
                document.getElementById("SendOTPButton").innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> &nbsp; Loading...`;
            },
            complete: function () {
                if (isOTPSent) {
                    $(".SendOTPButton").html("Resend OTP")
                } else {
                    $(".SendOTPButton").html("Send OTP")
                }                
            },
            success: function (data) {
                if (data == true) {
                    $("#OTP").removeAttr("disabled", true)
                    $("#Email").attr("disabled", true)
                    isOTPSent = true;
                } else {
                    $("#EmailSpan").html("User not registered")
                    isOTPSent = false;
                }
            }
        })
    }
}

function verifyOTP() {

    if ($("#OTP").val() == "") {
        $("#OTPSpan").html("OTP is required")

        if ($("#Email").val() == "") {
            $("#EmailSpan").html("Email is required")
        }
    }
    else {
        $("#OTPSpan").html("")
        $("#EmailSpan").html("")

        var forgotPassword = {
            Email: $("#Email").val(),
            OTP: $("#OTP").val()
        }

        $.ajax({
            url: "/Account/VerifyOTP",
            type: "POST",
            data: { forgotPassword: forgotPassword },
            success: function (data) {
                if (data == "ValidOTP") {
                    getChangePasswordView($("#Email").val());
                }
                else if (data == "OTPIsExpired") {
                    $("#OTPSpan").html("OTP is expired")
                }
                else if (data == "InvalidOTP") {
                    $("#OTPSpan").html("Wrong OTP")
                }
                else if (data == "FirstGenerateOTP") {
                    $("#OTPSpan").html("First generate OTP")
                }
            }
        })
    }
}

function getChangePasswordView(email) {
    $.ajax({
        url: "/Account/GetChangePasswordView",
        type: "GET",
        data: {},
        success: function (data) {
            $(".AccounPageContent").empty()
            $(".AccounPageContent").html(data)
            $("#Email").val(email)
        }
    })
}

function backToForgotPasswordView() {
    $.ajax({
        url: "/Account/GetForgotPasswordView",
        type: "GET",
        data: {},
        success: function (data) {
            $(".AccounPageContent").empty()
            $(".AccounPageContent").html(data)
        }
    })
}

function changePassword() {

    $("#ConfirmNewPasswordSpan").html("")
    $("#NewPasswordSpan").html("")

    if ($("#NewPassword").val() == "") {
        $("#NewPasswordSpan").html("New password is required")
    }
    else if ($("#ConfirmNewPassword").val() == "") {
        $("#ConfirmNewPasswordSpan").html("Confirm New Password is required")
    }
    else if (!/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/.test($("#NewPassword").val())) {
        $("#NewPasswordSpan").html("Password should contains minimum 8 character, one capital character, one numeric value and one special symbol")
    }
    else if ($("#NewPassword").val() != $("#ConfirmNewPassword").val()) {
        $("#NewPasswordSpan").html("Password does not matched")
    }
    else {
        $("#ConfirmNewPasswordSpan").html("")
        $("#NewPasswordSpan").html("")

        var changePassword = {
            Email: $("#Email").val(),
            ConfirmNewPassword: $("#ConfirmNewPassword").val(),
            NewPassword: $("#NewPassword").val()
        }

        $.ajax({
            url: "/Account/ChangePassword",
            type: "POST",
            data: { resetPassword: changePassword },
            success: function (data) {
                if (data == "Changed") {                    
                    backToForgotPasswordView();
                    Swal.fire({
                        position: 'top-start',
                        icon: 'success',
                        title: 'Password Changed',
                        showConfirmButton: false,
                        timer: 1500
                    })
                } else {
                    $("#OldPasswordSpan").html("Something went wrong!")
                }
            }
        })
    }
}