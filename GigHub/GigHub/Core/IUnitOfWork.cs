using GigHub.Core.Repositories;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IAttendenceRepository Attendences { get; }
        IFollowingRepository Followings { get; }
        IGenreRepository Genres { get; }
        IGigRepository Gigs { get; }
        IApplicationUserRepository ApplicationUser { get;  }
        INotificationRepository Notification { get;  }
        IUserNotificationRepository UserNotification { get;}
        void Complete();
    }
}