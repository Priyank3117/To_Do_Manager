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

            $("#TaskName").val("")
            $("#TaskDescription").val("")
            $("#StartDate").val("")
            $("#EndDate").val("")

            $("#TaskNameSpan").html("")
            $("#TaskDescriptionSpan").html("")
            $("#StartDateSpan").html("")
            $("#EndDateSpan").html("")

            if (result.length != 0) {
                $("#TaskAssignTo").empty()
                for (var i = 0; i < result.length; i++) {
                    $("#TaskAssignTo").append('<option value="' + result[i].userId + '">' + result[i].userName + '</option>')
                }
                $(".assignedTo").css("display", "block")
            } else {
                $(".assignedTo").css("display", "none")
            }
            $("#TeamId").val(teamId)
            $("#AddTaskModal").modal("show")
        }
    })
}

$("#StartDate").change(function () {
    $("#EndDate").attr("min", $("#StartDate").val())
})

$("#EndDate").change(function () {
    $("#StartDate").attr("max", $("#EndDate").val())
})

function addTask() {

    $("#TaskNameSpan").html("")
    $("#TaskDescriptionSpan").html("")
    $("#StartDateSpan").html("")
    $("#EndDateSpan").html("")

    var startDate = new Date($("#StartDate").val());
    var today = new Date();

    if ($("#TaskName").val().trim() == "") {
        $("#TaskNameSpan").html("Task name is required")
    }
    else if ($("#TaskDescription").val().trim() == "") {
        $("#TaskDescriptionSpan").html("Task DescriptionSpan is required")
    }
    else if ($("#StartDate").val() == "") {
        $("#StartDateSpan").html("Start Date is required")
    }
    else if ($("#EndDate").val() == "") {
        $("#EndDateSpan").html("End Date is required")
    }
    else if (startDate.getDate() < today.getDate()) {
        $("#StartDateSpan").html("Enter valid start date")
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

    $.ajax({
        type: "POST",
        url: "/AllTasks/GetForAddTaskToToDo",
        data: { teamId: teamId },
        success: function (result) {
            console.log(result)
            if (result.length == 0) {
                AddTaskToToDo(userId, teamId, taskId);
            }
            else {

                teamIdForAddTaskToToDo = teamId;
                taskIdForAddTaskToToDo = taskId;

                var user = ""
                for (var i = 0; i < result.length; i++) {
                    if (i != 0) {
                        user += `<div class="form-check d-flex justify-content-start align-items-center my-3">
                            <input class="form-check-input me-4" type="radio" name="AddTaskToTodayTask" id="`+ result[i].userId + `User" value="` + result[i].userId + `">
                            <label class="form-check-label" for="`+ result[i].userId + `User">
                                <span class="d-flex justify-content-start align-items-center">
                                    <img src="`+ result[i].avatar + `" class="userAvatar me-2">
                                    `+ result[i].userName + `
                                </span>
                            </label>
                        </div>`
                    } else {
                        user += `<div class="form-check d-flex justify-content-start align-items-center my-3">
                            <input class="form-check-input me-4" type="radio" name="AddTaskToTodayTask" id="`+ result[i].userId + `User" value="` + result[i].userId + `" checked>
                            <label class="form-check-label" for="`+ result[i].userId + `User">
                                <span class="d-flex justify-content-start align-items-center">
                                    <img src="`+ result[i].avatar + `" class="userAvatar me-2">
                                    `+ result[i].userName + `
                                </span>
                            </label>
                        </div>`
                    }

                }
                $(".selectMember").empty()
                $(".selectMember").html(user)

                $("#AddTaskToTodayTaskModal").modal("show")
            }
        }
    })
}

var teamIdForAddTaskToToDo = 0;
var taskIdForAddTaskToToDo = 0;

function AddTaskToToDoForMember() {
    if (teamIdForAddTaskToToDo != 0 && taskIdForAddTaskToToDo != 0) {
        var task = {
            TeamId: teamIdForAddTaskToToDo,
            UserId: $("input[name='AddTaskToTodayTask']:checked").val(),
            TaskId: taskIdForAddTaskToToDo,
        }

        $.ajax({
            type: "POST",
            url: "/AllTasks/AddTaskToTodayTaskForTeamMember",
            data: { task: task },
            success: function (IsTodayTask) {
                if (IsTodayTask == true) {
                    searchTeams();
                    $("#AddTaskToTodayTaskModal").modal("hide")
                }
            }
        })
    }
}

function AddTaskToToDo(userId, teamId, taskId) {
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

$("#StartDateForFilter").change(function () {
    $("#EndDateForFilter").attr("min", $("#StartDateForFilter").val())
})

$("#EndDateForFilter").change(function () {
    $("#StartDateForFilter").attr("max", $("#EndDateForFilter").val())
})

function openFilterModal() {
    $("#TaskNameForFilter").val($("#searchTask").val())
    $("#FilterModal").modal("show")
}

function searchTeams(isClearAll) {

    if (isClearAll == true) {
        teamName = ""
        taskName = ""
        startDate = ""
        endDate = ""
        $('input[name="TaskStatusForFilter"]:checked').prop('checked', false)
        $("#searchTask").val("")
    }
    else {
        var teamName = $("#TeamNameForFilter").val()
        var taskName = $("#TaskNameForFilter").val()
        var startDate = $("#StartDateForFilter").val()
        var endDate = $("#EndDateForFilter").val()
        var taskStatus = $('input[name="TaskStatusForFilter"]:checked').val()
    }

    var filter = {
        TeamName: teamName,
        TaskName: taskName,
        StartDate: startDate,
        EndDate: endDate,
        TaskStatus: taskStatus
    }

    $.ajax({
        type: "POST",
        url: "/AllTasks/GetAllTaskOfAllTeams",
        data: { filter: filter },
        success: function (result) {
            $(".allTeamsContainer").empty()
            $(".allTeamsContainer").html(result)
            $("#filterapplied").empty()

            if (teamName != "")
                $("#filterapplied").append(`<div class="filterTag">
                <p class="m-0 p-0"><b>Team Name - </b>`+ teamName + `</p>
                <a role="button" class="teamNameApplied"><img src="/images/cancel.png"></a>
            </div>`)

            if (taskName != "")
                $("#filterapplied").append(`<div class="filterTag">
                <p class="m-0 p-0"><b>Task Name - </b>`+ taskName + `</p>
                <a role="button" class="taskNameApplied"><img src="/images/cancel.png"></a>
            </div>`)

            if (startDate != "")
                $("#filterapplied").append(`<div class="filterTag">
                <p class="m-0 p-0"><b>Start Date - </b>`+ startDate + `</p>
                <a role="button" class="startDateApplied"><img src="/images/cancel.png"></a>
            </div>`)

            if (endDate != "")
                $("#filterapplied").append(`<div class="filterTag">
                <p class="m-0 p-0"><b>End Date - </b>`+ endDate + `</p>
                <a role="button" class="endDateApplied"><img src="/images/cancel.png"></a>
            </div>`)

            if (taskStatus != "" && taskStatus == "true") {
                $("#filterapplied").append(`<div class="filterTag">
                <p class="m-0 p-0"><b>Task Status - </b>Complete</p>
                <a role="button" class="taskStatusApplied"><img src="/images/cancel.png"></a>
            </div>`)
            }
            else if (taskStatus != "" && taskStatus == "false") {
                $("#filterapplied").append(`<div class="filterTag">
                <p class="m-0 p-0"><b>Task Status - </b>Incomplete</p>
                <a role="button" class="taskStatusApplied"><img src="/images/cancel.png"></a>
            </div>`)
            }

            if (teamName != "" || taskName != "" || startDate != "" || endDate != "" || taskStatus != undefined) {
                $("#filterapplied").append(`<div class="filterTag">
                <a role="button" onclick="searchTeams(true)"
                   class="text-muted d-flex justify-content-center align-items-center"
                   style="text-decoration: none;color:black">
                    <p class="m-0 p-0">
                        Clear all
                    </p>
                    &nbsp;
                    <img src="/images/cancel.png" alt="" style="width: 10px;height: 10px;">
                </a>
            </div>`)
            }

            $("#FilterModal").modal("hide")
        }
    })
}

$("body").on("click", ".teamNameApplied", function () {
    $("#TeamNameForFilter").val("")
    searchTeams();
})

$("body").on("click", ".taskNameApplied", function () {
    $("#TaskNameForFilter").val("")
    searchTeams();
})

$("body").on("click", ".startDateApplied", function () {
    $("#StartDateForFilter").val("")
    searchTeams();
})

$("body").on("click", ".endDateApplied", function () {
    $("#EndDateForFilter").val("")
    searchTeams();
})

$("body").on("click", ".taskStatusApplied", function () {
    $('input[name="TaskStatusForFilter"]:checked').prop('checked', false)
    searchTeams();
})

$("#grid").click(function () {
    $("#CalenderView").css("display", "block")
    $(".allTeamsContainer").css("display", "none")
    $("#grid").css("background-color", "#333")
    $("#grid").css("color", "white")
    $("#list").css("background-color", "white")
    $("#list").css("color", "#333")
    $(".filterRowContainer").css("display", "none")
    $(".filterButton").css("display", "none")
    $("#searchTask").css("display", "none")
    $(".taskColorInfo").css("display", "block")
})

$("#list").click(function () {
    $("#CalenderView").css("display", "none")
    $(".allTeamsContainer").css("display", "block")
    $("#list").css("background-color", "#333")
    $("#list").css("color", "white")
    $("#grid").css("background-color", "white")
    $("#grid").css("color", "#333")
    $(".filterRowContainer").css("display", "block")
    $(".filterButton").css("display", "flex")
    $("#searchTask").css("display", "block")
    $(".taskColorInfo").css("display", "none")
})

// View Task Details
function openTaskDetailOffcanvas(e, taskId) {
    e.stopPropagation()
    var myOffcanvas = document.getElementById('TaskDetailOffCanvas')
    var bsOffcanvas = new bootstrap.Offcanvas(myOffcanvas)

    $.ajax({
        type: "POST",
        url: "/Home/GetTaskDetails",
        data: { taskId: taskId },
        beforeSend: function () {
            $("#TaskDetailOffCanvas").removeClass("show")
        },
        success: function (result) {
            $("#TeamNameInOffcanvas").empty()
            $("#TeamNameInOffcanvas").html('' + result.teamName + '')
            $("#TaskIdInOffcanvas").val('' + result.taskId + '')
            $("#TaskNameInOffcanvas").val('' + result.taskName + '')
            if (result.taskDescription != null) {
                $("#TaskDescriptionInOffcanvas").val('' + result.taskDescription + '')
            } else {
                $("#TaskDescriptionInOffcanvas").val('')
            }
            $("#StartDateInOffcanvas").val('' + result.startDateForDisplay + '')
            $("#EndDateInOffcanvas").val('' + result.endDateForDisplay + '')
            $("#EndDateInOffcanvas").attr("min", $("#StartDateInOffcanvas").val())
            if (result.isCompleted == true) {
                $("input[name=TaskStatusInOffcanvas][value='true']").prop("checked", true);
            } else {
                $("input[name=TaskStatusInOffcanvas][value='false']").prop("checked", true);
            }

            $("#TaskNameInOffcanvasSpan").html("")
            $("#StartDateInOffcanvasSpan").html("")
            $("#EndDateInOffcanvasSpan").html("")

            bsOffcanvas.show()
        }
    })
}

// Edit Task In OffCanvas

$("#StartDateInOffcanvas").change(function () {
    $("#EndDateInOffcanvas").attr("min", $("#StartDateInOffcanvas").val())
})

$("#EndDateInOffcanvas").change(function () {
    $("#StartDateInOffcanvas").attr("max", $("#EndDateInOffcanvas").val())
})

function editTask() {

    $("#TaskNameInOffcanvasSpan").html("")
    $("#StartDateInOffcanvasSpan").html("")
    $("#EndDateInOffcanvasSpan").html("")

    if ($("#TaskNameInOffcanvas").val().trim() == "") {
        $("#TaskNameInOffcanvasSpan").html("Task name is required")
    }
    else if ($("#EndDateInOffcanvas").val() == "") {
        $("#EndDateInOffcanvasSpan").html("End Date is required")
    }
    else {
        var task = {
            "TaskId": $("#TaskIdInOffcanvas").val(),
            "TaskName": $("#TaskNameInOffcanvas").val(),
            "TaskDescription": $("#TaskDescriptionInOffcanvas").val(),
            "StartDate": $("#StartDateInOffcanvas").val(),
            "EndDate": $("#EndDateInOffcanvas").val(),
            "IsCompleted": $('input[name="TaskStatusInOffcanvas"]:checked').val()
        }

        $.ajax({
            type: "POST",
            url: "/Home/EditTask",
            data: { task: task },
            success: function (data) {
                if (data == true) {
                    searchTeams();
                    $('#TaskDetailOffCanvas').offcanvas('hide')
                    toastr.success('Task Edited');
                } else {
                    toastr.error('Not Saved');
                }
            }
        })
    }
}

function searchTask() {
    var filter = {
        TaskName: $("#searchTask").val()
    }

    $.ajax({
        type: "POST",
        url: "/AllTasks/GetAllTaskOfAllTeams",
        data: { filter: filter },
        success: function (result) {
            $(".allTeamsContainer").empty()
            $(".allTeamsContainer").html(result)
            $("#filterapplied").empty()            

            if (taskName != "")
                $("#filterapplied").append(`<div class="filterTag">
                <p class="m-0 p-0"><b>Task Name - </b>`+ taskName + `</p>
                <a role="button" class="taskNameApplied"><img src="/images/cancel.png"></a>
            </div>`)            

            if (taskName != "") {
                $("#filterapplied").append(`<div class="filterTag">
                <a role="button" onclick="searchTeams(true)"
                   class="text-muted d-flex justify-content-center align-items-center"
                   style="text-decoration: none;color:black">
                    <p class="m-0 p-0">
                        Clear all
                    </p>
                    &nbsp;
                    <img src="/images/cancel.png" alt="" style="width: 10px;height: 10px;">
                </a>
            </div>`)
            }            
        }
    })
}

function addTaskOnEnter(teamId) {
    if ($('#AddTaskOnEnter_' + teamId + '').val().trim() != "") {

        var task = {
            "TeamId": teamId,
            "TaskName": $('#AddTaskOnEnter_' + teamId + '').val().trim()
        }

        $.ajax({
            type: "POST",
            url: "/Home/AddTask",
            data: { task: task },
            success: function (data) {
                if (data) {
                    searchTeams();
                    toastr.success('Task Added');
                } else {
                    toastr.error('Task Not Added');
                }
            }
        })
    }
}

function addTaskOnEnterForMember(userId, teamId) {
    if ($('#AddTaskOnEnterForMember_' + userId + '-' + teamId + '').val().trim() != "") {

        var task = {
            "TeamId": teamId,
            "UserId": userId,
            "TaskName": $('#AddTaskOnEnterForMember_' + userId + '-' + teamId + '').val().trim()
        }

        $.ajax({
            type: "POST",
            url: "/Home/AddTask",
            data: { task: task },
            success: function (data) {
                if (data) {
                    searchTeams();
                    toastr.success('Task Added');
                } else {
                    toastr.error('Task Not Added');
                }
            }
        })
    }
}