using FluentAssertions;
using GigHub.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GigHub.Tests.Domain
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void GigCancel_WhenCalled_ShouldReturnGigCanceledNotification()
        {
            Gig gig = new Gig();
            Notification notification =  Notification.GigCancel(gig);


            //Note: we can assert 2 things here, because those assertions are strongly related & testing one logical test
            //This notification
            // object should be in the right state, meaning its type should be
            // GigCanceled and its gig should be the gig for each we created 
            // the notification. 
            notification.Type.Should().Be(NotificationType.GigCanceled);
            notification.Gig.Should().Be(gig);
        }
    }
}
