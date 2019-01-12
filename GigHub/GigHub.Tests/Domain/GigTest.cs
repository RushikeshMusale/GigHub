using FluentAssertions;
using GigHub.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GigHub.Tests.Domain
{
    [TestClass]
    public class GigTest
    {
        [TestMethod]
        public void Cancel_WhenCalled_ShouldSetIsCancelledToTrue()
        {
            Gig gig = new Gig();
            gig.Cancel();

            gig.IsCanceled.Should().BeTrue();
        }

        [TestMethod]
        public void Cancel_WhenCalled_EachAttendentShouldHaveANotification()
        {
            Gig gig = new Gig();
            gig.Attendences.Add(new Attendence() { Attendee = new ApplicationUser() { Name = "1" } });

            gig.Cancel();

            var attendees = gig.Attendences.ToList();

            attendees[0].Attendee.UserNotifications.Count().Should().Be(1);
        }
    }
}
