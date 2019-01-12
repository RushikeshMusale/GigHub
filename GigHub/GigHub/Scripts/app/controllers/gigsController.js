
//To call the service 
//step 1: add reference in the controller
var GigsController = function (attendenceService) {

    var init = function (container) {

        $(container).on("click", ".js-toggle-attendence", toggleAttendece)
        //$(".js-toggle-attendence").click(toggleAttendece);
        // Problem with this code is if elements .js-toggle-attendence are added after page load
        // click event will not be registered for them. Because we are calling init() on document.read()

        // Another issue is if there are 10 buttons with .js-toggle-attendence, 
        // there will be 10 instance of function toggleAttendence in memory
    }

    var button;

    var toggleAttendece = function (e) {
        button = $(e.target);
        var gigId = button.attr("data-gig-id");
        if (button.hasClass("btn-default"))
            //step 3: call the service
            attendenceService.createAttendence(gigId, done, fail);
        else
            attendenceService.deleteAttendece(gigId, done, fail);

    }

    var done = function () {
        var text = (button.text() == "Going?" ? "Going" : "Going?");
        debugger;
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    }

    var fail = function () {
        alert("something failed");
    }

    return {
        init: init
    }
}(AttendenceService); //Step2: register the service
//(); this is important. it means variables, functions inside this block is limited. 


