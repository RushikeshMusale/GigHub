using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        ApplicationDbContext _context;

        public UserNotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserNotification> GetNewUserNotificationsFor(string userId)
        {
            return _context.UserNotificaitons
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();
        }
    }
}