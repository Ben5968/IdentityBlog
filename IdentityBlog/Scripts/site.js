
$(document).ready(function() {
    assignUserRole = function (id) {
        $.get("/Admin/AddUserToRole/" + id, function (response) {
            console.log(response);
        });
    };
});