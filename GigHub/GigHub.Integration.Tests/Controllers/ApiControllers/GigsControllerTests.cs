using System;
using NUnit.Framework;
using GigHub.Core.Persistance;
using GigHub.Persistance;
using GigHub.Core.Models;
using System.Linq;
using GigHub.Integration.Tests.Extensions;
using FluentAssertions;

namespace GigHub.Integration.Tests.Controllers.ApiControllers
{
    [TestFixture]
    public class GigsControllerTests
    {
        private GigHub.Controllers.Api.GigsController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext();
            _controller = new GigHub.Controllers.Api.GigsController(new UnitOfWork(_context));
        }

        [Test,Isolated]
        public void Cancel_WhenCalled_ShouldCancelTheGig()
        {
            //Arrange            
            ApplicationUser user = _context.Users.First();
            _controller.MockApiCurrentUser(user.UserName, user.Id);

            Genre genre = _context.Genres.First();
            Gig gig = new Gig
            {
                Artist = user,
                Datetime = DateTime.Now.AddDays(1),
                Genre = genre,
                Venue ="-",               
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            //Act
            var result = _controller.Cancel(gig.Id);
            //now we will have to reload it to get id of the gig           

            //Assert
            //_context.Entry(gig).Reload();
            // We don't need to reload the gig here, because we already have actual gig object
            // in the case of update, we had ViewModel instead of actual gig object
            gig.IsCanceled.Should().BeTrue();
        }
    }
}
