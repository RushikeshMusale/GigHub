using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class AttendencesControllerTest
    {
        private AttendencesController _controller;
        private Mock<IAttendenceRepository> _mockRepositoy;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private string _userId;

        [TestInitialize]
        public void TestIntialize()
        {            
            _mockRepositoy = new Mock<IAttendenceRepository>();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.SetupGet(a => a.Attendences).Returns(_mockRepositoy.Object);

            _userId = "1";

            _controller = new AttendencesController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser("user1@domain.com", _userId);
        }

        [TestMethod]
        public void Attend_UserAttendingAGigForWhichHeHasAnAttendence_ShouldReturnBadRequest()
        {
            //Attendence attendence = new Attendence { AttendeeId = _userId }; 
            //Setting Attendece object with userId is not required. Since we perform setup on repository method
            //and testing controller, not the repository

            Attendence attendence = new Attendence(); //this will be enough

            //This simply means that when GetAttendece is called, it will return Attendece object          
            _mockRepositoy.Setup(g => g.GetAttendence(_userId , 1)).Returns(attendence);

            var result =_controller.Attend(new AttendeceDto() { GigId = 1 });

            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public void Attend_ValidRequest_ShouldReturnOk()
        {
            var result = _controller.Attend(new AttendeceDto());
            result.Should().BeOfType<OkResult>();         
        }

        [TestMethod]
        public void Delete_NoAttendeceWithGivenIdExists_ShouldReturnBadRequest()
        {
            var result = _controller.Delete(1);
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Delete_ValidRequest_ShouldReturnOk()
        {
            Attendence attendence = new Attendence();
            _mockRepositoy.Setup(g => g.GetAttendence(_userId, 1)).Returns(attendence);

            var result = _controller.Delete(1);
            result.Should().BeOfType<OkNegotiatedContentResult<int>>();

        }

        [TestMethod]
        public void Delete_ValidRequest_ShouldReturnIdOfDeletedAttendence()
        {
            Attendence attendence = new Attendence();
            _mockRepositoy.Setup(g => g.GetAttendence(_userId, 1)).Returns(attendence);

            var result = (OkNegotiatedContentResult<int>) _controller.Delete(1);
            result.Content.Should().Be(1);
        }
    }
}
