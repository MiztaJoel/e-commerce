using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

		public PartialViewResult ProductListPartial(int? page, int? category )
		{
			var pageNumber = page ?? 1;
			var pageSize = 12;
			//var productList = db.Products.OrderByDescending(x => x.ProductId).ToList();
			if(category != null)
			{
				ViewBag.category = category;
				var productList = db.Products
					.OrderByDescending(x => x.ProductId)
					.Where(x=>x.CategoryId==category)
					.ToPagedList(pageNumber, pageSize);
				return PartialView(productList);
			}
			else
			{
				var productList = db.Products.OrderByDescending(x => x.ProductId).ToPagedList(pageNumber, pageSize);
				return PartialView(productList);
			}
		}


		public PartialViewResult FirstProductListPartial(int? page)
		{
			var pageNumber = page ?? 1;
			var pageSize = 12;
			//var productList = db.Products.OrderByDescending(x => x.ProductId).ToList();
			var productList = db.Products.OrderByDescending(x => x.ProductId).ToPagedList(pageNumber, pageSize);
			return PartialView(productList);
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}
	}
}