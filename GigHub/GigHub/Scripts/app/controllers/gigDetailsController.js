var GigDetailsController = function (followingService) {

    var button = $(".js-toggle-following");

    var init = function () {
        $(".js-toggle-following").click(toggleFollowing);
    }

    var toggleFollowing = function (e) {
        var button = $(e.target);
        var followerId = button.attr("data-following")
        debugger;
        if (button.hasClass("btn-default"))
            followingService.createFollowing(followerId, done, fail);
        else
            followingService.removeFollowing(followerId, done, fail);
    }

    var done = function () {
        var text = (button.text() == "Follow" ? "Following" : "Follow");
        button.toggleClass("btn-default").toggleClass("btn-info").text(text);
    }

    var fail = function () {
        alert("something failed")
    }   

    return {
        init: init
    }
}(FollowingService);

