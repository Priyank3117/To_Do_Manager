function SearchAvailableTeam() {
    $.ajax({
        type: "GET",
        url: "/Document/GetAllAvailableTeams",
        data: { searchTerm: $("#searchTeamInDoc").val() },
        success: function (result) {

            $(".UserMemberTeams").empty()
            if (result.length != 0) {
                for (var i = 0; i < result.length; i++) {
                    if (result[i].status == "Pending") {
                        $(".UserMemberTeams").append(`<a role="button" onclick="getTeamDetails(` + result[i].teamId + `)" id=` + result[i].teamId + ` class="requestSendButton" disabled>
                        `+ result[i].teamName + `
                    </a>`)
                    } else {
                        $(".UserMemberTeams").append(`<a role="button" onclick="getTeam(` + result[i].teamId + `)" id=` + result[i].teamId + ` class="joinTeamButton">
                        `+ result[i].teamName + `
                    </a>`)
                    }
                }
                $(".availableTeamsText").css("display", "block")
            }
            else {
                $(".UserMemberTeams").append(`<div class="allTeamContainer" style="justify-content: center;">
                    <p class="text-muted mt-5" style="font-family: sans-serif;font-size:xx-large;">No Documents Available</p>
                </div>`)
                $(".availableTeamsText").css("display", "none")
            }
        }
    })
}


function getTeam(teamId) {

    $(document).on('click', '.joinTeamButton', function () {
        var link = document.createElement('a');
        link.href = '/Document/DocumentPage?teamId=' + teamId
        link.click()
    })
}


document.addEventListener("DOMContentLoaded", function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});


