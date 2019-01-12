using GigHub.Core;
using GigHub.Core.Persistance;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class FolloweesController : Controller
    {       
        private readonly IUnitOfWork _unitOfWork;

        public FolloweesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Followees
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();        
            var artists = _unitOfWork.ApplicationUser.GetArtsistFollowedBy(userId);

            return View(artists);
        }
    }
}