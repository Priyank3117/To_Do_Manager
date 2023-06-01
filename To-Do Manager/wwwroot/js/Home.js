$(document).ready(function () {

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
        data: { searchTerm: $("#searchTeam").val() },
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

function GetDataForAddTask(teamId, userId) {
    $.ajax({
        type: "POST",
        url: "/Home/GetDataForAddTask",
        data: { teamId: teamId, userId: userId },
        success: function (result) {
            if (result.length != 0) {
                $("#TaskAssignTo").empty()
                for (var i = 0; i < result.length; i++) {
                    $("#TaskAssignTo").append('<option value="' + result[i].userId + '">' + result[i].userName + '</option>')
                }
            } else {
                $(".assignedTo").css("display", "none")
            }
            $("#TeamId").val(teamId)
            $("#AddTaskModal").modal("show")
        }
    })
}

function AddTask() {

    if ($("#TaskName").val() == "") {
        $("#TaskNameSpan").html("Task name is required")
    }
    else {
        $("#TaskNameSpan").html("")

        var task = {
            TeamId: $("#TeamId").val(),
            UserId: $('#TaskAssignTo option:selected').val(),
            TaskName: $("#TaskName").val(),
        }

        $.ajax({
            type: "POST",
            url: "/Home/AddTask",
            data: { task: task },
            success: function (result) {
                if (result == true) {
                    location.reload(true);
                }
            }
        })
    }
}

function MarkTaskAsCompleteOrUncomplete(userId, teamId, taskId) {

    var task = {
        TeamId: teamId,
        UserId: userId,
        TaskId: taskId,
    }

    $.ajax({
        type: "POST",
        url: "/Home/MarkTaskAsCompleteOrUncomplete",
        data: { task: task },
        success: function (result) {
            if (result == true) {
                $('#Task_' + taskId + '').empty()
                $('#Task_' + taskId + '').html(`<svg xmlns="http://www.w3.org/2000/svg" width="20"
                                                     height="20" fill="currentColor"
                                                     class="bi bi-check-circle-fill" viewBox="0 0 16 16">
                                                                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z" />
                                                                    </svg>
                                                                </button>`)
            } else {
                $('#Task_' + taskId + '').empty()
                $('#Task_' + taskId + '').html(`<svg xmlns="http://www.w3.org/2000/svg" width="20"
                                                     height="20" fill="currentColor"
                                                     class="bi bi-check-circle" viewBox="0 0 16 16">
                                                                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                                                                        <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z" />
                                                                    </svg>`)
            }
        }
    })
}

//function SearchTeamInAllTeamsPage() {
//    $.ajax({
//        type: "GET",
//        url: "/Home/GetAllTeams",
//        data: { searchTerm: $("#searchTeam").val() },
//        success: function (result) {

//            $(".allTeamContainer").empty()
//            for (var i = 0; i < result.length; i++) {
//                if (result[i].status == "Pending") {
//                    $(".allTeamContainer").append(`<a role="button" onclick="GetTeamDetails(` + result[i].teamId + `)" id=` + result[i].teamId + ` class="requestSendButton" disabled>
//                        `+ result[i].teamName + `
//                    </a>`)
//                } else {
//                    $(".allTeamContainer").append(`<a role="button" onclick="GetTeamDetails(` + result[i].teamId + `)" id=` + result[i].teamId + ` class="joinTeamButton">
//                        `+ result[i].teamName + `
//                    </a>`)
//                }
//            }
//        }
//    })
//}