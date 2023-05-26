$(document).ready(function () {

    SearchTeam();

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

        var emailHTML = `<div class="AddedUser mt-2" id=` + idOfEmailText +`>
                                            <p class="text-muted m-0 userEmail">`+ $("#AddUser").val() +`</p>
                                            <a role="button" onclick="RemoveUser(`+ idOfEmailText +`)" class="ms-2">
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

function RemoveUser(idOfEmailText) {
    $('#' + idOfEmailText + '').remove();
}

$(".createTeamButton").click(function () {
    debugger
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
})

function SearchTeam() {
    $.ajax({
        type: "GET",
        url: "/Home/GetAllTeams",
        data: { searchTerm: $("#searchTeam").val()},
        success: function (result) {

            $(".allTeamContainer").empty()
            for (var i = 0; i < result.length; i++) {
                if (result[i].status == "Pending") {
                    $(".allTeamContainer").append(`<a role="button" onclick="GetTeamDetails(` + result[i].teamId + `)" id=` + result[i].teamId + ` class="requestSendButton" disabled>
                        `+ result[i].teamName + `
                    </a>`)
                } else {
                    $(".allTeamContainer").append(`<a role="button" onclick="GetTeamDetails(` + result[i].teamId + `)" id=` + result[i].teamId + ` class="joinTeamButton">
                        `+ result[i].teamName + `
                    </a>`)
                }
            }
        }
    })
}

function GetTeamDetails(teamId) {

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

function RequestToJoinTeam() {
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