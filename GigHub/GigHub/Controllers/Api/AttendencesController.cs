using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Persistance;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendencesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendencesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        // By Default int is expected in url not in body
        // so decorate it with [FromBody]
        // To avoid this, use DTOs
        public IHttpActionResult Attend(AttendeceDto dto)
        {
            var userId = User.Identity.GetUserId();
            // Now we can use GetAttendence here & Delete method
            //if (_context.Attendences.Any(
            //        a => a.AttendeeId == userId
            //        && a.GigId == dto.GigId)
            //    )
            //    return BadRequest("The attendence already exists");

            if(_unitOfWork.Attendences.GetAttendence(userId, dto.GigId)!=null)
                return BadRequest("The attendence already exists");

            Attendence attendence = new Attendence
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _unitOfWork.Attendences.Add(attendence);
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            //var attendence = _context.Attendences.SingleOrDefault(
            //                        a => a.AttendeeId == userId 
            //                        && a.GigId == id);

            var attendence = _unitOfWork.Attendences.GetAttendence(User.Identity.GetUserId(), id);

            if (attendence == null)
                return NotFound();

            _unitOfWork.Attendences.Remove(attendence);
            _unitOfWork.Complete();
            return Ok(id);
        }
    }
}
