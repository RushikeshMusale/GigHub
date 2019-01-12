using GigHub.Core.Repositories;
using GigHub.Persistance;
using GigHub.Persistance.Repositories;

namespace GigHub.Core.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _context;

        public IGigRepository Gigs { get; private set; }
        public IAttendenceRepository Attendences { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public IUserNotificationRepository UserNotification { get;private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(context);
            Attendences = new AttendenceRepository(context);
            Genres = new GenreRepository(context);
            Followings = new FollowingRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            Notification = new NotificationRepository(context);
            UserNotification = new UserNotificationRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}