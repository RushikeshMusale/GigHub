using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistance;
using GigHub.Persistance.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;

namespace GigHub.Tests.Persistance.Repositories
{

    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockGigs;
        private Mock<DbSet<Attendence>> _mockAttendence;
        private AttendenceRepository _attendenceRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockGigs = new Mock<DbSet<Gig>>();
            _mockAttendence = new Mock<DbSet<Attendence>>();
            var mockContext = new Mock<IApplicationDbContext>();

            mockContext.Setup(g => g.Gigs).Returns(_mockGigs.Object);
            mockContext.Setup(g => g.Attendences).Returns(_mockAttendence.Object);
            _repository = new GigRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned()
        {
            Gig gig = new Gig { Datetime = DateTime.Now.AddDays(-1) , ArtistId ="1"};
            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUpcomingGigsByArtist("1");
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCancel_ShouldNotBeReturned()
        {
            Gig gig = new Gig { Datetime = DateTime.Now.AddDays(1), ArtistId = "1" };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist("1");
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtit_GigIsAForDifferentArtist_ShouldNotBeReturned()
        {
            Gig gig = new Gig { Datetime = DateTime.Now.AddDays(1), ArtistId = "1" };
            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId + "-");
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForTheGivenArtistAndIsInTheFuture_ShouldBeReturned()
        {
            Gig gig = new Gig { Datetime = DateTime.Now.AddDays(1), ArtistId = "1" };
            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId);
            gigs.Should().Contain(gig);
        }

        [TestMethod]
        public void GetGigsUserAttending_AttendenceForADifferentUser_ShouldNotBeReturned()
        {
            Gig gig = new Gig() { Datetime = DateTime.Now.AddDays(1) };
            Attendence attendence = new Attendence() { Gig = gig, AttendeeId = "1" };
            _mockAttendence.SetSource(new[] { attendence });

            var gigs = _repository.GetGigsUserAttending( "-");
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserAttending_GigIsInThePast_ShouldNotBeReturned()        
        {
            Gig gig = new Gig() { Datetime = DateTime.Now.AddDays(-1) };
            Attendence attendence = new Attendence { Gig = gig, AttendeeId = "1" };
            _mockAttendence.SetSource(new[] { attendence });

            var gigs = _repository.GetGigsUserAttending(attendence.AttendeeId);
            gigs.Should().BeEmpty();

        }

        [TestMethod]
        public void GetGigsUserAttending_GigIsInTheFutureAndUserIsAttending_ShouldBeReturned()
        {
            Gig gig = new Gig { Datetime = DateTime.Now.AddDays(1) };
            Attendence attendence = new Attendence { Gig = gig, AttendeeId = "1" };
            _mockAttendence.SetSource(new[] { attendence });

            var gigs = _repository.GetGigsUserAttending(attendence.AttendeeId);
            gigs.Should().Contain(gig);
            //gigs.Should().HaveCount(1);
        }
    }
}
