using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GigHub.Persistance.Repositories;
using GigHub.Persistance;
using Moq;
using System.Data.Entity;
using GigHub.Core.Models;
using GigHub.Tests.Extensions;
using FluentAssertions;

namespace GigHub.Tests.Persistance.Repositories
{
    [TestClass]
    public class NotificationRepositoryTests
    {
        private NotificationRepository _repository;
        private Mock<DbSet<UserNotification>> _mockNotifications;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _mockNotifications = new Mock<DbSet<UserNotification>>();

            Mock<IApplicationDbContext> _mockContext = new Mock<IApplicationDbContext>();            
            _mockContext.SetupGet(g => g.UserNotificaitons).Returns(_mockNotifications.Object);

            _repository = new NotificationRepository(_mockContext.Object);
        }

        [TestMethod]
        public void GetNewNotificationsFor_NotificationIsForADiffernetUser_ShouldNotBeReturned()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser() { Id = "1" };
            Notification notification = Notification.GigCancel(new Gig());
            UserNotification userNotification = new UserNotification(user, notification);

            _mockNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNewNotificationsFor(userNotification.UserId + "-");
            notifications.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNewNotificationsFor_NotificationIsRead_ShouldNotBeReturned()
        {
            ApplicationUser user = new ApplicationUser() { Id = "1" };
            Notification notification = Notification.GigCancel(new Gig());
            UserNotification userNotification = new UserNotification(user, notification);
            userNotification.Read();

            _mockNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNewNotificationsFor(userNotification.UserId);
            notifications.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNewNotificationsFor_NewNotifcationForAGivenUser_ShouldBeReturned()
        {
            ApplicationUser user = new ApplicationUser() { Id = "1" };
            Notification notification = Notification.GigCancel(new Gig());
            UserNotification userNotification = new UserNotification(user, notification);

            _mockNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNewNotificationsFor(userNotification.UserId);
            notifications.Should().Contain(notification);
        }
    }
}
