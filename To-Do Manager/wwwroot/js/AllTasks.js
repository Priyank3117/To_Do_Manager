$(document).ready(function () {
    searchTeams();

    $("#CalenderView").css("display", "none")
    $(".allTeamsContainer").css("display", "block")
    $("#list").css("background-color", "#333")
    $("#list").css("color", "white")
    $(".fc-prev-button, .fc-next-button").css("background-color", "#333")
    $(".fc-today-button").addClass("todayButton")
    $(".fc-today-button").css("color", "#000000")
})

function getDataForAddTask(teamId, userId) {
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

function addTask() {

    $("#TaskNameSpan").html("")
    $("#TaskDescriptionSpan").html("")
    $("#StartDateSpan").html("")
    $("#EndDateSpan").html("")

    var startDate = new Date($("#StartDate").val());
    var endDate = new Date($("#EndDate").val());

    if ($("#TaskName").val() == "") {
        $("#TaskNameSpan").html("Task name is required")
    }
    else if ($("#TaskDescription").val() == "") {
        $("#TaskDescriptionSpan").html("Task DescriptionSpan is required")
    }
    else if ($("#StartDate").val() == "") {
        $("#StartDateSpan").html("Start Date is required")
    }
    else if ($("#EndDate").val() == "") {
        $("#EndDateSpan").html("End Date is required")
    }
    else if (startDate > endDate) {
        $("#StartDateSpan").html("Enter valid start & end date")
    }
    else {

        var task = {
            TeamId: $("#TeamId").val(),
            UserId: $('#TaskAssignTo option:selected').val(),
            TaskName: $("#TaskName").val(),
            TaskDescription: $("#TaskDescription").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
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

function markTaskAsCompleteOrUncomplete(userId, teamId, taskId) {

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
                $('#Task_' + taskId + '').html(`<svg xmlns="http://www.w3.org/2000/svg" width="25"
                                                     height="25" fill="currentColor"
                                                     class="bi bi-check-circle-fill" viewBox="0 0 16 16">
                                                                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z" />
                                                                    </svg>
                                                                </button>`)
            } else {
                $('#Task_' + taskId + '').empty()
                $('#Task_' + taskId + '').html(`<svg xmlns="http://www.w3.org/2000/svg" width="25"
                                                     height="25" fill="currentColor"
                                                     class="bi bi-check-circle" viewBox="0 0 16 16">
                                                                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                                                                        <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z" />
                                                                    </svg>`)
            }
        }
    })
}

function addTaskToTodayTask(userId, teamId, taskId) {

    var task = {
        TeamId: teamId,
        UserId: userId,
        TaskId: taskId,
    }

    $.ajax({
        type: "POST",
        url: "/AllTasks/AddTaskToTodayTask",
        data: { task: task },
        success: function (IsTodayTask) {
            if (IsTodayTask == true) {
                $('#task_' + taskId + '').empty()
                $('#task_' + taskId + '').html(`<svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-arrow-left-circle-fill" viewBox="0 0 16 16">
                                                                                            <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm3.5 7.5a.5.5 0 0 1 0 1H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5z" />
                                                                                        </svg>`)
            } else {
                $('#task_' + taskId + '').empty()
                $('#task_' + taskId + '').html(`<svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-arrow-left-circle" viewBox="0 0 16 16">
                                                                                            <path fill-rule="evenodd" d="M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8zm15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-4.5-.5a.5.5 0 0 1 0 1H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5z" />
                                                                                        </svg>`)
            }
        }
    })
}

function searchTeams() {
    $.ajax({
        type: "GET",
        url: "/AllTasks/GetAllTaskOfAllTeams",
        data: { searchTerm: $("#searchTeam").val() },
        success: function (result) {
            $(".allTeamsContainer").empty()
            $(".allTeamsContainer").html(result)
        }
    })
}

$("#grid").click(function () {
    $("#CalenderView").css("display","block")
    $(".allTeamsContainer").css("display", "none")
    $("#grid").css("background-color", "#333")
    $("#grid").css("color", "white")
    $("#list").css("background-color", "white")
    $("#list").css("color", "#333")
})

$("#list").click(function () {
    $("#CalenderView").css("display", "none")
    $(".allTeamsContainer").css("display", "block")
    $("#list").css("background-color", "#333")
    $("#list").css("color", "white")
    $("#grid").css("background-color", "white")
    $("#grid").css("color", "#333")
})