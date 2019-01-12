using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using GigHub.Controllers;
using GigHub.Core.Persistance;
using GigHub.Persistance;
using GigHub.Integration.Tests.Extensions;
using GigHub.Core.Models;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using GigHub.Core.ViewModels;

namespace GigHub.Integration.Tests.Controllers
{
    [TestFixture]
    public class GigsControllerTests
    {
        //For integration Tests we need real unit of work & application db context
        private GigsController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
             _context = new ApplicationDbContext();
            _controller = new GigsController(new UnitOfWork(_context));           
        }

        [TearDown] //Dispose _Context
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated] //DON'T FORGET to add Isolated Attribute on test, otherwise this gig will not be deleted after test is executed
        public void Mine_WhenCalled_ShouldReturnUpcomingGigs()
        {
            // #Arange

            //Let's mock current user in integration test intead of fixture.
            //unlike unit tests
            ApplicationUser user = _context.Users.First();

            // Let's take first user : flexible test case as username is not hardcoded
            // seed method in setupfixture ensures that there are two users
            _controller.MockCurrentUser(user.Name, user.Id);

            var genre = _context.Genres.First();
            Gig gig = new Gig { Artist = user, Datetime = DateTime.Now.AddDays(1), Genre = genre, Venue = "-" };
            _context.Gigs.Add(gig);
            _context.SaveChanges(); //DON'T FORGET to add Isolated Attribute on test, otherwise this gig will not be deleted after test is executed

            // #Act

            // Originally return type of mine was ActionResult
            // Mine() has only one execution path & returning always View, so make it's return type as ViewResult
            // pros-> avoid extra casting. Otherwise model is not available
            var result = _controller.Mine();

            // #Assert

            // we are returning IEnumerable<Gig>
            (result.Model as IEnumerable<Gig>).Should().HaveCount(1);          
        }

        [Test, Isolated] //DON'T FORGET to add Isolated Attribute on test, otherwise this gig will not be deleted after test is executed
        public void Modify_WhenCalled_ShouldUpdateTheGivenGig()
        {
            // #Arange

            //Let's mock current user in integration test intead of fixture.
            //unlike unit tests
            ApplicationUser user = _context.Users.First();

            // Let's take first user : flexible test case as username is not hardcoded
            // seed method in setupfixture ensures that there are two users
            _controller.MockCurrentUser(user.Name, user.Id);

            var genre = _context.Genres.Single(g=>g.Id ==1);
            Gig gig = new Gig { Artist = user, Datetime = DateTime.Now.AddDays(1), Genre = genre, Venue = "-" };
            _context.Gigs.Add(gig);
            _context.SaveChanges(); //DON'T FORGET to add Isolated Attribute on test, otherwise this gig will not be deleted after test is executed

            // #Act

            // Originally return type of mine was ActionResult
            // Mine() has only one execution path & returning always View, so make it's return type as ViewResult
            // pros-> avoid extra casting. Otherwise model is not available
            var result = _controller.Modify(new GigFormViewModel
            {
                id = gig.Id,
                Date = DateTime.Now.AddMonths(1).ToString("d MMM yyyy"),
                Time = "20:00",
                Venue = "Venue",
                Genre =  2,
            });

            // #Assert

            // Changes in act are saved in Database, but not in in memory gig object
            // This will refresh the gig object with database values
            _context.Entry(gig).Reload();

            gig.Datetime.Should().Be(DateTime.Today.AddMonths(1).AddHours(20)); // Note DateTime.Today instead of DateTime.Now
            gig.Venue.Should().Be("Venue");
            gig.GenreId.Should().Be(2);           
          
        }
    }
}
