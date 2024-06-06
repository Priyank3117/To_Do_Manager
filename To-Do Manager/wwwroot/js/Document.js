
//Function return all the available team to the login user and add the button in the dom
//function SearchAvailableTeam() {
//    $.ajax({
//        type: "GET",
//        url: "/Document/GetAllAvailableTeams",
//        data: { searchTerm: $("#searchTeamInDoc").val() },
//        success: function (result) {

//            $(".UserMemberTeams").empty() 
//            if (result.length != 0) {
//                for (var i = 0; i < result.length; i++) {
//                    if (result[i].status == "Pending") {
//                        $(".UserMemberTeams").append(`<a role="button" onclick="getTeamDetails(` + result[i].teamId + `)" id=` + result[i].teamId + ` class="requestSendButton" disabled>
//                        `+ result[i].teamName + `
//                    </a>`)
//                    } else {
//                        $(".UserMemberTeams").append(`<a role="button" onclick="getTeam(` + result[i].teamId + `,'` + result[i].teamName + `')" id=` + result[i].teamId + ` class="joinTeamButton">
//                        `+ result[i].teamName + `

//                    </a>`)
//                    }
//                }
//                $(".availableTeamsText").css("display", "block")
//            }
//            else {
//                $(".UserMemberTeams").append(`<div class="allTeamContainer" style="justify-content: center;">
//                    <p class="text-muted mt-5" style="font-family: sans-serif;font-size:xx-large;">No Documents Available</p>
//                </div>`)
//                $(".availableTeamsText").css("display", "none")
//            }
//        }
//    })
//}

//This function is user to call the document page which shows list of the documents
function getTeam(teamId,teamName) {

    $(document).on('click', '.joinTeamButton', function() {
        var link = document.createElement('a');
        link.href = '/Document/DocumentPage?teamId=' + teamId + '&teamName=' + teamName
        link.click()
    })
}




//This is the function for the show tooltip
document.addEventListener("DOMContentLoaded", function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});



function MyCustomUploadAdapterPlugin(editor) {
    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
        return new MyUploadAdapter(loader);
    };
}


//Function for the uploading the image to the server
class MyUploadAdapter {
    constructor(loader) {
        this.loader = loader;
    }

    upload() {
        return this.loader.file
            .then(file => new Promise((resolve, reject) => {
                const data = new FormData();
                data.append('file', file);

                fetch('/upload', {
                    method: 'POST',
                    body: data,
                })
                    .then(response => response.json())
                    .then(data => {
                        resolve({
                            default: data.url // URL to the uploaded image
                        });
                    })
                    .catch(error => {
                        reject(error);
                    });
            }));
    }

    abort() {
        // Handle abort if needed
    }
}

//configration of the ckeditor
let editor;

ClassicEditor
    .create(document.querySelector('#editor'), {
        extraPlugins: [MyCustomUploadAdapterPlugin],
      
        toolbar: {

            items: [
                'heading','|','bold','italic', 'underline','strikethrough', '|', 'link', '|','bulletedList','numberedList', '|','indent','outdent', '|', 'blockQuote',
                'insertTable', '|', 'imageUpload', 'mediaEmbed', '|', 'undo', 'redo', '|', 'sourceEditing', 'exportPdf'
            ]
        },
    })
    .then(response => {
        console.log('Response:', response);
        editor = response;
        return response.json();
    })
    .catch(error => {
        console.error(error.stack);
    });











