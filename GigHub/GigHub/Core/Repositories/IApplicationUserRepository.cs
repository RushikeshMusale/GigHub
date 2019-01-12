using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IApplicationUserRepository
    {
        IEnumerable<ApplicationUser> GetArtsistFollowedBy(string userId);
        ApplicationUser GetArtist(string userId);
    }
}