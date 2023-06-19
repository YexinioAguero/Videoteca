// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function previewProfilePicture(event) {
    var input = event.target;
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var profilePicture = document.getElementById('profile-picture');
            profilePicture.src = e.target.result;
            profilePicture.alt = 'Profile Picture';
        }
        reader.readAsDataURL(input.files[0]);
    }
}    
