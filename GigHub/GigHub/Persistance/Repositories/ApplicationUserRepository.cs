using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;        
        }

        public ApplicationUser GetArtist(string userId)
        {
            return _context.Users
                        .Include(f=>f.Followers)
                        .SingleOrDefault(x => x.Id == userId);
        }

        public IEnumerable<ApplicationUser> GetArtsistFollowedBy(string userId)
        {
            return _context.Followings
               .Where(f => f.FollowerId == userId)
               .Select(f => f.Followee)
               .ToList();
        }
    }
}