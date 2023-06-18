// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {


function previewProfilePicture(event) {
    var input = event.target;
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var ProfilePicture = document.getElementById("profile-picture");
            ProfilePicture.src = e.target.result;
            ProfilePicture.alt = 'Profile picture';
        }
    
    reader.readAsDataURL(input.files[0]);
    }
}