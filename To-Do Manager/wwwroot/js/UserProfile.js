$(document).ready(function () {

    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "/UserProfile/GetUserAvatar",
            data: {},
            success: function (result) {
                var userName = result.firstName + ' ' + result.lastName
                $("#profileImage").attr('src', '' + result.avatar + '')
                $("#UserNameInUserProfile").append('' + userName + '')
            }
        })
    })

    // Change Password Validation
    $("#oldPassword").focusout(function () {
        if ($('#oldPassword').val() == '') {
            $("#oldPasswordSpan").html('Old password is required');
        }
        else if ($('#oldPassword').val().length < 8) {
            $("#oldPasswordSpan").html('Minimum length is 8 character');
        }
        else {
            $("#oldPasswordSpan").html('');
            $(".changePasswordButton").removeAttr("disabled");
        }
    })

    $("#newPassword").focusout(function () {
        if ($('#newPassword').val() == '') {
            $("#newPasswordSpan").html('New password is required');
        }
        else if ($('#newPassword').val().length < 8) {
            $("#newPasswordSpan").html('Minimum length is 8 character');
        }
        else if ($('#newPassword').val() == $('#oldPassword').val()) {
            $("#newPasswordSpan").html('Old password and New password can not be same');
        }
        else {
            $("#newPasswordSpan").html('');
            $(".changePasswordButton").removeAttr("disabled");
        }
    })

    $("#confirmPassword").focusout(function () {
        if ($('#confirmPassword').val() == '') {
            $("#confirmPasswordSpan").html('Confirm password is required');
        }
        else if ($('#confirmPassword').val().length < 8) {
            $("#confirmPasswordSpan").html('Minimum length is 8 character');
        }
        else if ($('#confirmPassword').val() != $('#newPassword').val()) {
            $("#confirmPasswordSpan").html('Password does not matched');
        }
        else {
            $("#confirmPasswordSpan").html('');
            $(".changePasswordButton").removeAttr("disabled");
        }
    })
})

//Open Change Password Modal
function openChangePasswordModal() {
    $('#oldPassword').val("")
    $('#newPassword').val("")
    $('#confirmPassword').val("")

    $("#oldPasswordSpan").html('')
    $("#newPasswordSpan").html('')
    $("#confirmPasswordSpan").html('')

    $("#changePasswordModal").modal("show")
}

// Change Password
function changePassword() {
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
    else if ($('#confirmPassword').val() == '') {
        $("#confirmPasswordSpan").html('Confirm password is required');
    }
    else if ($('#confirmPassword').val().length < 8) {
        $("#confirmPasswordSpan").html('Minimum length is 8 character');
    }
    else if ($('#newPassword').val() == $('#oldPassword').val()) {
        $("#newPasswordSpan").html('Old password and New password can not be same');
    }
    else if (!/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/.test($('#newPassword').val())) {
        $("#newPasswordSpan").html("Password should contains minimum 8 character, one capital character, one numeric value and one special symbol")
    }
    else {
        $.ajax({
            type: "POST",
            url: "/UserProfile/ChangePassword",
            data: { oldPassword: oldPassword, newPassword: newPassword },
            success: function (result) {
                if (result == "Changed") {
                    $(".changePasswordButton").html("Password Changed")
                    $("#changePasswordModal").modal("hide")
                    Swal.fire({
                        position: 'top-start',
                        icon: 'success',
                        title: 'Password Changed',
                        showConfirmButton: false,
                        timer: 1500
                    })
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
    const file = event.target.files[0];

    var formData = new FormData();
    formData.append('file', file);

    $.ajax({
        type: "POST",
        url: "/UserProfile/ChangeImage",
        processData: false,
        contentType: false,
        data: formData,
        success: function (result) {
            if (result == "Changed") {
                /*image.src = URL.createObjectURL(event.target.files[0]);*/
                location.reload(true)
            }
        }
    });
};

// Leave From Team
function getAllTeams() {
    $.ajax({
        type: "POST",
        url: "/UserProfile/GetTeamNames",
        data: { },
        success: function (data) {
            console.log(data)
            var team = "";

            for (var i = 0; i < data.length; i++) {
                var button = "";

                if (data[i].leaveStatus == "RequestedForLeave") {
                    button = `<button type="button" class="leaveTeamSmallButton" disabled>
                                Requsted
                            </button>`
                } else {
                    button = `<button type="button" class="leaveTeamSmallButton" id="` + data[i].teamId + `" onclick="leaveFromTeam(` + data[i].teamId + `,` + data[i].userId + `, \'` + data[i].teamName +`\')">
                                Leave
                            </button>`
                }

                team += `<div class="ListOfTeams">
                            <p class="m-0">`+ data[i].teamName + `</p>
                            `+ button +`
                         </div>`;
            }

            $(".ListOfTeamsContainer").html(team)
            $("#LeaveTeamsModal").modal("show")
        }
    });
}

function leaveFromTeam(teamId, userId, teamName) {
    $(".warningText").html(`Are you sure to leave from <span class="h5">` + teamName + `</span> team?`)
    $(".leaveTeamButtonContainer").html(`<button type="button" class="CancelCreateTeamButton me-2"
                                                    data-bs-toggle="modal"
                                                    data-bs-target="#LeaveTeamsModal">
                                                Cancel
                                            </button>
                                            <button type="button" class="leaveTeamsButton" onclick="finalLeaveFromTeam(` + teamId + `,` + userId + `)">Leave</button>`)
    $("#LeaveWarningModal").modal("show")
    $("#LeaveTeamsModal").modal("hide")
}

function finalLeaveFromTeam(teamId, userId) {
    $.ajax({
        type: "POST",
        url: "/UserProfile/LeaveFromTeam",
        data: { teamId: teamId, userId: userId },
        success: function (data) {
            if (data == true) {
                $('#' + teamId + '').html("Requsted")
                $('#' + teamId + '').attr("disabled", true)
                $("#LeaveWarningModal").modal("hide")
                $("#LeaveTeamsModal").modal("show")
            }
        }
    });
}

// Leave All Teams
function leaveAllTeams() {
    $(".warningText").html(`Are you sure to leave from all the teams?`)
    $(".leaveTeamButtonContainer").html(`<button type="button" class="CancelCreateTeamButton me-2"
                                                    data-bs-toggle="modal">
                                                Cancel
                                            </button>
                                            <button type="button" class="leaveTeamsButton" onclick="leaveFromAllTeam()">Yes</button>`)
    $("#LeaveWarningModal").modal("show")
}

function leaveFromAllTeam() {
    $.ajax({
        type: "POST",
        url: "/UserProfile/LeaveFromAllTeam",
        data: { },
        success: function (data) {
            if (data == true) {

                $(".leaveTeamSmallButton").html("Requsted")

                $("#LeaveWarningModal").modal("hide")
            }
        }
    });
}