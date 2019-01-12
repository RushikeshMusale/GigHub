using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Following following)
        {
            _context.Followings.Add(following) ;
        }

        //public bool IsUserFollowingArtist(string userId, string artistId)
        //{
        //    return _context.Followings
        //        .Any(f => f.FollowerId == userId
        //                && f.FolloweeId == artistId);
        //}

        //Instead of isUserFollowingArtist create a method as getFollowing
        //It will be more reusable
        public Following GetFollowing(string userId, string artistId)
        {
            return _context.Followings
                .SingleOrDefault(f => f.FollowerId == userId
                        && f.FolloweeId == artistId);
        }

        public void Remove(Following following)
        {
            _context.Followings.Remove(following);
        }
    }
}