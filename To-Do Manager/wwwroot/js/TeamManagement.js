$(document).ready(function () {

    getAllTeamDetailsPartialView();

    $("#TeamName").focusout(function () {
        if ($("#TeamName").val().trim() == "") {
            $("#TeamNameSpan").html("Team name is required")
        } else {
            $("#TeamNameSpan").html("")
        }
    })

    $("#TeamDescription").focusout(function () {
        if ($("#TeamDescription").val().trim() == "") {
            $("#TeamDescriptionSpan").html("Description is required")
        } else {
            $("#TeamDescriptionSpan").html("")
        }
    })

    $("#AddUser").focusout(function () {
        $("#AddUserSpan").html("")
    })

    $("#AddUserInTeam").focusout(function () {
        $("#AddUserInTeamSpan").html("")
    })
})

var usersEmailInCreateTeam = []
$(".addUser").click(function () {
    if ($("#AddUser").val() == "") {
        $("#AddUserSpan").html("Email is required")
    } else if (!/^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test($("#AddUser").val())) {
        $("#AddUserSpan").html("Invalid Email")
    } else {
        $("#AddUserSpan").html("")
        var idOfEmailText = $(".AddedUser").length + 1;
        var canAdd = true;

        for (var i = 0; i < usersEmailInCreateTeam.length; i++) {
            if (usersEmailInCreateTeam[i] == `` + $("#AddUser").val() + ``) {
                $("#AddUserSpan").html("Already Added")
                canAdd = false;
            } else {
                $("#AddUserSpan").html("")
            }
        }

        if (canAdd) {
            var emailHTML = `<div class="AddedUser mt-2" id=` + idOfEmailText + `>
                                            <p class="text-muted m-0 userEmail">`+ $("#AddUser").val() + `</p>
                                            <a role="button" onclick="removeUser(`+ idOfEmailText + `)" class="ms-2">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" style="color: black;" fill="currentColor"
                                                     class="bi bi-x-lg" viewBox="0 0 16 16">
                                                    <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
                                                </svg>
                                            </a>
                                        </div>`
            $(".AddedUserContainer").append(emailHTML)
            usersEmailInCreateTeam.push($("#AddUser").val())
            $("#AddUser").val("")
        }
    }
})

// Add User In Team Validation
var usersEmail = []
$(".addUserInTeam").click(function () {
    if ($("#AddUserInTeam").val() == "") {
        $("#AddUserInTeamSpan").html("Email is required")
    } else if (!/^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test($("#AddUserInTeam").val())) {
        $("#AddUserInTeamSpan").html("Invalid Email")
    } else {
        $("#AddUserInTeamSpan").html("")
        var idOfEmailText = $(".AddedUserInTeam").length + 1;
        var canAdd = true;
        
        for (var i = 0; i < usersEmail.length; i++) {
            if (usersEmail[i] == `` + $("#AddUserInTeam").val() + ``) {
                $("#AddUserInTeamSpan").html("Already Added")
                canAdd = false;
            } else {
                $("#AddUserInTeamSpan").html("")
            }
        }

        if (canAdd) {
            var emailHTML = `<div class="AddedUserInTeam mt-2" id="` + idOfEmailText + `InTeam">
                                            <p class="text-muted m-0 userEmailInTeam">`+ $("#AddUserInTeam").val() + `</p>
                                            <a role="button" onclick="removeUserFromTeam(`+ idOfEmailText + `)" class="ms-2">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" style="color: black;" fill="currentColor"
                                                     class="bi bi-x-lg" viewBox="0 0 16 16">
                                                    <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
                                                </svg>
                                            </a>
                                        </div>`
            $(".AddedUserInTeamContainer").append(emailHTML)
            usersEmail.push($("#AddUserInTeam").val())
            $("#AddUserInTeam").val("")
        }
    }
})

function removeUser(idOfEmailText) {
    console.log(usersEmailInCreateTeam)
    usersEmailInCreateTeam.splice(usersEmailInCreateTeam.indexOf($('#' + idOfEmailText + ' p').html()), 1);
    console.log(usersEmailInCreateTeam)
    $('#' + idOfEmailText + '').remove();
}

function removeUserFromTeam(idOfEmailText) {
    usersEmail.splice(usersEmail.indexOf($('#' + idOfEmailText + 'InTeam p').html()), 1);
    $('#' + idOfEmailText + 'InTeam').remove();
}

function createTeam() {

    if ($("#TeamName").val().trim() == "") {
        $("#TeamNameSpan").html("Team name is required")
    }
    else if ($("#TeamDescription").val().trim() == "") {
        $("#TeamDescriptionSpan").html("Description is required")
    }
    else {
        $("#TeamNameSpan").html("")
        $("#TeamDescriptionSpan").html("")

        var team = new FormData();
        team.append('TeamName', $("#TeamName").val());
        team.append('TeamDescription', $("#TeamDescription").val());

        var allUsersEmails = $(".userEmail")

        for (var i = 0; i < allUsersEmails.length; i++) {
            team.append('UserEmails', allUsersEmails[i].innerHTML);
        }

        $.ajax({
            type: "POST",
            url: "/Home/CreateTeam",
            processData: false,
            contentType: false,
            beforeSend: function () {
                $("#CreateTeamButton").attr("disabled", true)
                document.getElementById("CreateTeamButton").innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> &nbsp; Loading...`;
            },
            complete: function () {
                $("#CreateTeamButton").html("Created")
                $("#CreateTeamButton").removeAttr("disabled")
            },
            data: team,
            success: function (result) {
                if (result == true) {
                    $("#CreateTeamModal").modal("hide")
                    location.reload(true)
                }
            }
        })
    }
}

function getJoinTeamPartialView() {
    $.ajax({
        type: "GET",
        url: "/TeamManagement/GetAllTeams",
        data: { searchTerm: "" },
        success: function (result) {
            $(".TeamRowContainer").empty()
            $(".TeamRowContainer").html(result)
            $(".teamManagementTopRow").addClass("justify-content-between")
            $(".teamManagementTopRow").removeClass("justify-content-end")
            $(".backToTeamManagementPage").css("display", "block")

        }
    })
}

function getAllTeamDetailsPartialView() {
    $.ajax({
        type: "GET",
        url: "/TeamManagement/GetAllTeamDetailsPartialView",
        data: { searchTerm: "" },
        success: function (result) {
            $(".TeamRowContainer").empty()
            $(".TeamRowContainer").html(result)
            $(".teamManagementTopRow").removeClass("justify-content-between")
            $(".teamManagementTopRow").addClass("justify-content-end")
            $(".backToTeamManagementPage").css("display", "none")
        }
    })
}

function getTeamDetails(teamId) {

    if (!($('#' + teamId + '').hasClass("requestSendButton"))) {
        $.ajax({
            type: "POST",
            url: "/Home/GetTeamDetails",
            data: { teamId: teamId },
            success: function (result) {
                $("#ViewTeamTeamId").val(result.teamId)
                $("#Name").val(result.teamName)
                $("#Description").val(result.teamDescription)
                $("#JoinTeamModal").modal("show")
            }
        })
    }
}

function requestToJoinTeam() {
    var userRequest = {
        "TeamId": $("#ViewTeamTeamId").val()
    }

    $.ajax({
        type: "POST",
        url: "/Home/RequestToJoinTeam",
        data: { userRequest: userRequest },
        success: function (result) {
            $('#' + $("#ViewTeamTeamId").val() + '').addClass("requestSendButton")
            $('#' + $("#ViewTeamTeamId").val() + '').attr("disabled", true)
            $("#JoinTeamModal").modal("hide")
        }
    })
}

// Add User In Team
var teamIdForAddUser = 0;
var teamNameForAddUser = "";
function openAddUserModal(teamId, teamName) {
    $(".AddedUserInTeamContainer").empty()
    $("#AddUserInTeamSpan").html("")
    usersEmail = [];
    console.log(usersEmail)
    $("#AddUserModal").modal("show")
    teamIdForAddUser = teamId;
    teamNameForAddUser = teamName;
}

function addUserInTeam() {
    var allUsersEmails = $(".userEmailInTeam")

    if (allUsersEmails.length == 0) {
        $("#AddUserInTeamSpan").html("Please first add user")
    }
    else {
        var team = new FormData();
        team.append('TeamId', teamIdForAddUser);
        team.append('TeamName', teamNameForAddUser);

        for (var i = 0; i < allUsersEmails.length; i++) {
            team.append('UserEmails', allUsersEmails[i].innerHTML);
        }

        $.ajax({
            type: "POST",
            url: "/TeamManagement/AddUserInTeam",
            processData: false,
            contentType: false,
            beforeSend: function () {
                $("#addUserInTeam").attr("disabled", true)
                document.getElementById("addUserInTeam").innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> &nbsp; Loading...`;
            },
            complete: function () {
                $("#addUserInTeam").html("Created")
                $("#addUserInTeam").removeAttr("disabled")
            },
            data: team,
            success: function (result) {
                if (result == true) {
                    location.reload(true)
                }
            }
        })
    }
}

// Remove User From Team
var userIdForRemoveUser = 0;
var teamIdForRemoveUser = 0;

function openRemoveUserModal(userName, userId, teamId) {
    $(".warnigLine").html('Are you sure to remove <b>' + userName + '</b> from the team?')
    userIdForRemoveUser = userId;
    teamIdForRemoveUser = teamId;
    $("#RemoveUserModal").modal("show")
}

function removeUserFromTeam() {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/RemoveUserFromTeam",
        data: { userId: userIdForRemoveUser, teamId: teamIdForRemoveUser },
        success: function (result) {
            if (result == true) {
                location.reload(true)
            }
        }
    })
}

// Set Reporting Person
var teamIdForSetReportingPerson = 0;
var userIdOfTeamMember = 0;

function openModalForSetReportingPerson(userId, teamId) {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/GetAllMemberToSetReportingPerson",
        data: { userId: userId, teamId: teamId },
        success: function (result) {
            teamIdForSetReportingPerson = teamId;
            userIdOfTeamMember = userId;

            var user = ""
            for (var i = 0; i < result.length; i++) {
                if (i != 0) {
                    user += `<div class="form-check d-flex justify-content-start align-items-center my-3">
                            <input class="form-check-input me-4" type="radio" name="SetReportingPersonRadio" id="`+ result[i].userId + `RP" value="` + result[i].userId + `">
                            <label class="form-check-label" for="`+ result[i].userId + `RP">
                                <span class="d-flex justify-content-start align-items-center">
                                    <img src="`+ result[i].avatar + `" class="userAvatar me-2">
                                    `+ result[i].userName + `
                                </span>
                            </label>
                        </div>`
                } else {
                    user += `<div class="form-check d-flex justify-content-start align-items-center my-3">
                            <input class="form-check-input me-4" type="radio" name="SetReportingPersonRadio" id="`+ result[i].userId + `RP" value="` + result[i].userId + `" checked>
                            <label class="form-check-label" for="`+ result[i].userId + `RP">
                                <span class="d-flex justify-content-start align-items-center">
                                    <img src="`+ result[i].avatar + `" class="userAvatar me-2">
                                    `+ result[i].userName + `
                                </span>
                            </label>
                        </div>`
                }

            }
            $(".selectReportingPerson").empty()
            $(".selectReportingPerson").html(user)
            $("#SetReportingPersonModal").modal("show")
        }
    })
}

function setReportingPerson() {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/SetReportingPerson",
        data: { userIdOfTeamMember: userIdOfTeamMember, userIdOfReportingPerson: $("input[name='SetReportingPersonRadio']:checked").val(), teamId: teamIdForSetReportingPerson },
        success: function (result) {
            if (result == true) {
                location.reload(true)
            }
        }
    })
}

// Remove Reporting Person
function removeReportingPerson(teamMemberUserId, reportingPersonUserId, teamId) {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/RemoveReportingPerson",
        data: { teamMemberUserId: teamMemberUserId, reportingPersonUserId: reportingPersonUserId, teamId: teamId },
        success: function (result) {
            if (result == true) {
                location.reload(true)
            }
        }
    })
}

// Accept Join Request
function acceptJoinRequest(userId, teamId) {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/AcceptJoinRequest",
        data: { userId: userId, teamId: teamId },
        success: function (result) {
            if (result == true) {
                location.reload(true)
            }
        }
    })
}

// Decline Join Request
function declineJoinRequest(userId, teamId) {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/DeclineJoinRequest",
        data: { userId: userId, teamId: teamId },
        success: function (result) {
            if (result == true) {
                location.reload(true)
            }
        }
    })
}

// Accept Leave Request
function acceptLeaveRequest(userId, teamId) {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/AcceptLeaveRequest",
        data: { userId: userId, teamId: teamId },
        success: function (result) {
            if (result == true) {
                location.reload(true)
            }
        }
    })
}

// Decline Join Request
function declineLeaveRequest(userId, teamId) {
    $.ajax({
        type: "POST",
        url: "/TeamManagement/DeclineLeaveRequest",
        data: { userId: userId, teamId: teamId },
        success: function (result) {
            if (result == true) {
                location.reload(true)
            }
        }
    })
}

// Leave From Team
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
                location.reload(true)
            }
        }
    });
}

function openCreateTeamModal() {
    $(".AddedUserContainer").empty()
    usersEmailInCreateTeam = [];
    $("#TeamName").val("")
    $("#TeamDescription").val("")
    $("#AddUser").val("")

    $("#TeamNameSpan").html("")
    $("#TeamDescriptionSpan").html("")
    $("#AddUserSpan").html("")

    $("#CreateTeamModal").modal("show")
}

// Delete Team
var teamIdForDeleteTeam = 0;

function deleteTeam(teamId, teamName) {
    $(".warnigLineForDeleteTeam").html('Are you sure to delete <b>' + teamName + '</b> team?')
    teamIdForDeleteTeam = teamId;

    $("#DeleteTeamModal").modal("show")
}

function finalDeleteTeam() {
    if (teamIdForDeleteTeam != 0) {
        $.ajax({
            type: "POST",
            url: "/TeamManagement/DeleteTeam",
            data: { teamId: teamIdForDeleteTeam },
            success: function (data) {
                if (data == true) {
                    location.reload(true)
                }
            }
        });
    }
}