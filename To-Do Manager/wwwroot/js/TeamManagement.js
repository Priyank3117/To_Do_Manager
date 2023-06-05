$(document).ready(function () {

    getAllTeamDetailsPartialView();

    $("#TeamName").focusout(function () {
        if ($("#TeamName").val() == "") {
            $("#TeamNameSpan").html("Team name is required")
        } else {
            $("#TeamNameSpan").html("")
        }
    })

    $("#TeamDescription").focusout(function () {
        if ($("#TeamDescription").val() == "") {
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

$(".addUser").click(function () {
    if ($("#AddUser").val() == "") {
        $("#AddUserSpan").html("Email is required")
    } else if (!/^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test($("#AddUser").val())) {
        $("#AddUserSpan").html("Invalid Email")
    } else {
        $("#AddUserSpan").html("")
        debugger
        var idOfEmailText = $(".AddedUser").length + 1;

        var emailHTML = `<div class="AddedUser mt-2" id=` + idOfEmailText + `>
                                            <p class="text-muted m-0 userEmail">`+ $("#AddUser").val() + `</p>
                                            <a role="button" onclick="RemoveUser(`+ idOfEmailText + `)" class="ms-2">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                                     class="bi bi-x-lg" viewBox="0 0 16 16">
                                                    <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
                                                </svg>
                                            </a>
                                        </div>`
        $(".AddedUserContainer").append(emailHTML)
        $("#AddUser").val("")
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
        debugger
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

function RemoveUser(idOfEmailText) {
    $('#' + idOfEmailText + '').remove();
}

function removeUserFromTeam(idOfEmailText) {
    $('#' + idOfEmailText + 'InTeam').remove();
}

function createTeam() {

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
        data: team,
        success: function (result) {
            if (result == true) {
                $("#CreateTeamModal").modal("hide")
            }
        }
    })
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
function OpenAddUserModal(teamId, teamName) {
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

function RemoveUser() {
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