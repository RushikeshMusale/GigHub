using GigHub.Core;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    public class GigsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {

            //.Include requires using System.Data.Entity
            // since attendences is collection & we are interested in attendee,
            // we eager load it using .select(a=>a.Attendee) 
            //var gig = _context.Gigs
            //    .Include(g=>g.Attendences.Select(a=>a.Attendee))                
            //    .Single(g => g.Id == id && g.ArtistId == userId);

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(id);

            if (gig == null || gig.IsCanceled)
                return NotFound();            

            if (gig.ArtistId != User.Identity.GetUserId())
                return Unauthorized();

            gig.Cancel();

            _unitOfWork.Complete();            

            return Ok();
        }
    }
}