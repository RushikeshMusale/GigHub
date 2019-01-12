using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class GigsControllerTest
    {
        private GigsController _controller;
        private Mock<IGigRepository> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IGigRepository>();
            var mockUoW = new Mock<IUnitOfWork>();

            //This will return empty mockRepository when controllers call gigs repository
            mockUoW.SetupGet(u => u.Gigs).Returns(_mockRepository.Object);

            _controller = new GigsController(mockUoW.Object);
            _userId = "1"; //this way of writing will help in multiuser tests easily - Step 1
            _controller.MockCurrentUser("user1@domain.com", _userId);
        }


        [TestMethod]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()        
        {
            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_GigIsCancelled_ShouldReturnNotFound()
        {
            //Arrange
            Gig gig = new Gig(); //for now we have changed protected Gig constructor to public
            gig.Cancel();

            //When getGigWithAttendees(int id) is called, it will return above gig object
            _mockRepository.Setup(g => g.GetGigWithAttendees(1)).Returns(gig);

            //Act
            var result = _controller.Cancel(1);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancellingAnotherUsersGig_ShouldReturnUnauthorized()
        {
            //Arange
            Gig gig = new Gig { ArtistId = _userId + "-" }; // Step 2 - multiuser testing            

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig); 

            //Act
            var result = _controller.Cancel(1); //the parameter should be same as we have arraanged in above line

            //Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            Gig gig = new Gig { ArtistId = _userId };
            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}
