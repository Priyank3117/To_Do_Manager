function sendOTP() {
    if ($("#Email").val() == '') {
        $("#EmailSpan").html("Email is required")
    }
    else if (!/^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test($("#Email").val())) {
        $("#EmailSpan").html("Enter valid email")
    }
    else {
        $("#EmailSpan").html("")
        var forgotPassword = {
            Email: $("#Email").val()
        }

        $.ajax({
            url: "/Account/SendOTP",
            type: "POST",
            data: { forgotPassword: forgotPassword },
            beforeSend: function () {
                document.getElementById("SendOTPButton").innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> &nbsp; Loading...`;
            },
            complete: function () {
                $(".SendOTPButton").html("Resend OTP")
            },
            success: function (data) {
                if (data == true) {
                    $("#OTP").removeAttr("disabled", true)
                    $("#Email").attr("disabled", true)
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

function backToForgotPasswordView(){
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
    if ($("#OldPassword").val() == "") {
        $("#OldPasswordSpan").html("Old password is required")

        if ($("#NewPassword").val() == "") {
            $("#NewPasswordSpan").html("New password is required")
        }
    }
    else {
        $("#OldPasswordSpan").html("")
        $("#NewPasswordSpan").html("")

        var changePassword = {
            Email: $("#Email").val(),
            OldPassword: $("#OldPassword").val(),
            NewPassword: $("#NewPassword").val()
        }

        $.ajax({
            url: "/Account/ChangePassword",
            type: "POST",
            data: { resetPassword: changePassword },
            success: function (data) {
                if (data == "EnterValidOldPassword") {
                    $("#OldPasswordSpan").html("Old password is not correct")
                } else {
                    backToForgotPasswordView();
                }
            }
        })
    }
}