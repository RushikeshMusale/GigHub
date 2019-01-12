using System;

namespace GigHub.Core.Models
{
    public class UserNotification
    {
        public string UserId { get; private set; }
        
        public int NotificationId { get; private set; }

        //Make all properties private set which otherwise can lead domain model to invalid state
        public ApplicationUser User { get; private set; }

        public Notification Notification { get; private set; }

        public bool IsRead { get; private set; }

        //Use public parametrized constructor to maintain valid state of business domain
        public UserNotification(ApplicationUser user, Notification notification)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (notification == null)
                throw new ArgumentNullException("notification");

            User = user;
            Notification = notification;
        }

        //use protected constructor so that Entity Framework can create object
        protected UserNotification()
        {

        }

        public void Read()
        {
            IsRead = true;
        }
    }
}