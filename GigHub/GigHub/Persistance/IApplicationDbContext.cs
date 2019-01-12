using System.Data.Entity;
using GigHub.Core.Models;

namespace GigHub.Persistance
{
    public interface IApplicationDbContext
    {
        DbSet<Attendence> Attendences { get; set; }
        DbSet<Following> Followings { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Gig> Gigs { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<UserNotification> UserNotificaitons { get; set; }
    }
}