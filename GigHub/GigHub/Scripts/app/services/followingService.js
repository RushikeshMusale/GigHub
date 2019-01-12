var FollowingService = function () {

    var createFollowing = function (followerId,done, fail) {
        $.post("/api/followings", { followerId: followerId})
               .done(done)
               .fail(fail)
    }

    var removeFollowing = function (followerId, done, fail) {
        $.ajax({
            url: "/api/followings/" + followerId,
            method: "DELETE",
        })
                .done(done)
                .fail(fail)
    }

    return {
        createFollowing: createFollowing,
        removeFollowing: removeFollowing
    }
}();