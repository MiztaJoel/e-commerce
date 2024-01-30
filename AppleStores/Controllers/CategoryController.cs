using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleStores.Models;
using System.Net;
using System.Data.Entity;



namespace AppleStores.Controllers
{
	public class CategoryController : Controller
	{
		AppleDatabaseEntities db = new AppleDatabaseEntities();
		// GET: Category
		public ActionResult Index()
		{
			return View(db.Cateories.OrderBy(x => x.Name).ToList());
		}
		//This return value to the _mainLayout view
		public PartialViewResult CategoryPartial()
		{
			var categoryList = db.Cateories.OrderBy(x => x.Name).ToList();
			return PartialView(categoryList);
		}

		public ActionResult Create()
		{
			return View();
		}
		//Post: Category/craete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "CategoryId,Name")] Cateory cateory)
		{
			if (ModelState.IsValid)
			{
				db.Cateories.Add(cateory);
				db.SaveChanges();
				return RedirectToAction("Index");

			}
			return View(cateory);
		}
		//Get:Category/Edit/1
		[HttpGet]
		[ActionName("Edit")]
		public ActionResult Edit_Category(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Cateory cateory = db.Cateories.Find(id);
			if (cateory == null)
			{
				return HttpNotFound();
			}
			return View(cateory);
		}
		//Post:Category/Edit/1
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include ="CategoryId,Name")] Cateory cateory)
		{
			if (ModelState.IsValid)
			{
				db.Entry(cateory).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(cateory);
		}

		public ActionResult Details(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var category = db.Cateories.Find(id);
			if(category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var category = db.Cateories.Find(id);
			db.Cateories.Remove(category);
			db.SaveChanges();

			return RedirectToAction("index");
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			base.Dispose(disposing);
			}
			base.Dispose(disposing);
		}
	}
}