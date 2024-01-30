using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleStores.Models;
using PagedList;


namespace AppleStores.Controllers
{
    public class ProductsController : Controller
    {
		AppleDatabaseEntities db = new AppleDatabaseEntities();
		// GET: Product
		public ActionResult Index()
        {
            return View();
        }

		public PartialViewResult ProductListPartial(int? page )
		{
			var pageNumber = page ?? 1;
			var pageSize = 12;
			//var productList = db.Products.OrderByDescending(x => x.ProductId).ToList();
			var productList = db.Products.OrderByDescending(x => x.ProductId).ToPagedList(pageNumber, pageSize);
			return PartialView(productList);
		}
    }
}