// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getTeamManagementPage(notificationId) {
    markNotificationAsRead(notificationId)
    window.location = 'https://localhost:7100/TeamManagement';
}

function getToDoPage(notificationId) {
    markNotificationAsRead(notificationId)
    window.location = 'https://localhost:7100/Home/AllTeamsPage';
}

function markNotificationAsRead(notificationId) {
    $.ajax({
        type: "POST",
        url: "/Home/MarkNotificationAsRead",
        data: { notificationId: notificationId },
        success: function (result) {           
        }
    })
}

function clearAllNotifications() {
    $.ajax({
        type: "POST",
        url: "/Home/ClearAllNotifications",
        data: { },
        success: function (result) {
            if (result == true) {                
                var noNotification = `<li class="firstRowNotification">
                    <p class="p-0 m-0" style="font-weight: 500;">Notification</p>
                    <a role="button" class="text-muted" id="clearAllNotification" onclick="clearAllNotifications()">Clear All</a>
                </li>
                <li class="noNotification">
                    <img src="/HTML/images/bell-big.png" alt="">
                    <p class="p-0 m-0 text-muted">You do not have any new notification</p>
                </li>`
                $("#forReopenThis").html(noNotification)
                $(".notificationDot").css("display", "none")
            }
        }
    })
}