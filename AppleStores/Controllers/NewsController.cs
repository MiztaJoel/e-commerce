using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleStores.Models;
using PagedList;
namespace AppleStores.Controllers
{
    public class NewsController : Controller
    {
		AppleDatabaseEntities db = new AppleDatabaseEntities();
		// GET: News
		public ActionResult Index()
        {
            return View();
        }

		public PartialViewResult NewsListPartial(int? page)
		{
			var pageNumber = page ?? 1;
			var pageSize = 10;
			var newsList = db.News.OrderByDescending(x => x.NewsId).ToPagedList(pageNumber, pageSize);
			return PartialView(newsList);
		}
	}
}