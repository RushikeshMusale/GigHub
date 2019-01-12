var AttendenceService = function () {

    //we don't have refernece to button, hence move it to parameter
    var createAttendence = function (gigId, done, fail) {
        // This will not work because ApiController is expecting gigId parameter
        // and jquery is sending data-gig-id
        // $.post("/api/Attendences", button.attr("data-gig-id"))
        // One option is to use object literal with empty string
        // $.post("/api/Attendences", {"": button.attr("data-gig-id")})
        // Secon option is to use DTO
        $.post("/api/Attendences", { gigId: gigId })
            .done(done)
            .fail(fail)
    }

    var deleteAttendece = function (gigId, done, fail) {
        $.ajax({
            url: "api/attendences/" + gigId,
            method: "DELETE",
        })
            .done(done)
            .fail(fail)
    }

    return {
        createAttendence: createAttendence,
        deleteAttendece: deleteAttendece
    }

}();

