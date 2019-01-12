using GigHub.Core;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //query=null, query is optional & it's default value is null
        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _unitOfWork.Gigs.GetUpcomingGigsByQuery(query);
            var attendeces = _unitOfWork.Attendences
                                .GetFutureAtendnences(User.Identity.GetUserId())
                                .ToLookup(g => g.GigId); 
            //Lookup vs Dictionary
            //lookup > Key don't have to be unique & if key does not exist it doesn't throw exception            

            GigsViewModel viewModel = new GigsViewModel
            {
                Heading = "Upcoming Gigs",
                UpcomingGigs = upcomingGigs,
                ShowAction = User.Identity.IsAuthenticated,
                SearchTerm=query,
                Attendences = attendeces
            };

            return View("Gigs", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}