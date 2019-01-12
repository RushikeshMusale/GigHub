
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Persistance;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            //if (_context.Followings.Any(f => f.FolloweeId == userId && f.FollowerId == dto.FollowerId))
            //    return BadRequest("Followings Already Exist");

            if (_unitOfWork.Followings.GetFollowing(userId, dto.FollowerId)!=null)
                return BadRequest("Followings Already Exist");

            Following following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FollowerId
            };

            _unitOfWork.Followings.Add(following);
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unfollow(string Id)
        {
            //var follower = _context.Followings.SingleOrDefault
            //                        (
            //                                f => f.FollowerId == userId &&
            //                                f.FolloweeId == Id
            //                        );

            var follower = _unitOfWork.Followings.GetFollowing(User.Identity.GetUserId(), Id);

            if (follower == null)
                return NotFound();

            _unitOfWork.Followings.Remove(follower);
            _unitOfWork.Complete();
                                  
            return Ok();
        }
        //[HttpPost]
        // To test in postman put follower id in body
        //public IHttpActionResult Follow([FromBody]string FollowerId)
        //{
        //    var userId = User.Identity.GetUserId();

        //    if (_context.Followings.Any(f => f.FolloweeId == userId && f.FollowerId == FollowerId))
        //        return BadRequest("Followings Already Exist");

        //    Following following = new Following
        //    {
        //        FollowerId = FollowerId,
        //        FolloweeId = userId
        //    };

        //    _context.Followings.Add(following);
        //    _context.SaveChanges();
        //    return Ok();
        //}

    }
}