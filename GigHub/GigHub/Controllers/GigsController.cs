using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Persistance;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {                                     
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(UnitOfWork unitOfWork)
        {                                                      
            _unitOfWork = unitOfWork;
        }

        public ViewResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _unitOfWork.Gigs.GetUpcomingGigsByArtist(userId);

            return View(gigs);
        }

        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            GigsViewModel ViewModel = new GigsViewModel
            {
                Heading = "Gig I'm Attending",
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(userId),
                ShowAction = User.Identity.IsAuthenticated,
                Attendences =  _unitOfWork.Attendences.GetFutureAtendnences(userId).ToLookup(g => g.GigId)            };

            return View("Gigs", ViewModel);
        }

       

      
        public ActionResult Search(GigsViewModel viewModel)
        {
            //In this way filtering is done in Index -> modified code in index
            //new { query = viewModel.SearchTerm} -> send only required things to controller.
            //  no need to send whole viewmodel
            return RedirectToAction("Index","Home",new { query = viewModel.SearchTerm});
        }

        [Authorize]
        public ActionResult Create()
        {
            var ViewModel = new GigFormViewModel
            {
                Heading = "Add a Gig",
                Genres = _unitOfWork.Genres.GetGenres(),
            };
            return View("GigForm",ViewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();
           
            var ViewModel = new GigFormViewModel
            {
                Heading="Edit a Gig",
                id= gig.Id,
                Date = gig.Datetime.ToString("d MMM yyyy"),
                Time = gig.Datetime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Genres = _unitOfWork.Genres.GetGenres(),
            };
            return View("GigForm", ViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel ViewModel)
        {

            if (!ModelState.IsValid)
            {
                ViewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm",ViewModel);
            }
            Gig gig = new Gig(
            
                _unitOfWork.ApplicationUser.GetArtist(User.Identity.GetUserId()),
                ViewModel.GetDatetime(),
                ViewModel.Venue,
                ViewModel.Genre
            );

            _unitOfWork.Gigs.Add(gig);           
            _unitOfWork.Complete();
            return RedirectToAction("Mine","Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(GigFormViewModel ViewModel)
        {

            if (!ModelState.IsValid)
            {
                ViewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm",ViewModel);
            }

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(ViewModel.id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId() )
                return new HttpUnauthorizedResult();

            gig.Update(ViewModel.Venue, ViewModel.Genre,ViewModel.GetDatetime());           

            _unitOfWork.Complete();
            return RedirectToAction("Mine","Gigs");
        }
       
        public ActionResult Details(int id)
        {
            var attendingMessage = "";
            var gig = _unitOfWork.Gigs.GetGigWithArtists(id);

            if (gig == null)
                return HttpNotFound();

            var userId = User.Identity.GetUserId();            

            if(_unitOfWork.Attendences.GetAttendence(userId, gig.Id)!=null)
            {
                attendingMessage = "You are going to this event";
            }

            var isFollowing = (_unitOfWork.Followings.GetFollowing(userId, gig.ArtistId)!=null);

            var viewModel = new GigDetailViewModel
            {
                Gig = gig,
                AttendingMessage = attendingMessage,
                IsFollowing = isFollowing
            };
                             
            return View("Details",viewModel);
        }

        [HttpPost]
        public ActionResult Details(GigDetailViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}