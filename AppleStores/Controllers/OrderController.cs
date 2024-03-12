using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppleStores.Models;
using CrystalDecisions.CrystalReports.Engine;
using PagedList;
using System.IO;

namespace AppleStores.Controllers
{
    public class OrderController : Controller
	{
		AppleDatabaseEntities db = new AppleDatabaseEntities();
		// GET: Order
		public ActionResult Index(int? page)
        {
			var pageNumber = page ?? 1;
			var pageSize = 6;
			//var productList = db.Products.OrderByDescending(x => x.ProductId).ToList();
			var orderList = db.Orders.OrderByDescending(x => x.OrderID).ToPagedList(pageNumber, pageSize);
			return View(orderList); 
		}
		public ActionResult ExportOrderListing()
		{
			ReportDocument rd = new ReportDocument();
			rd.Load(Path.Combine(Server.MapPath("~/MyReports/OrderListing.rpt")));
			rd.SetDataSource(db.Orders.ToList());

			Response.Buffer = false;
			Response.ClearContent();
			Response.ClearHeaders();

			Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
			str.Seek(0, SeekOrigin.Begin);
			string savedFilename = string.Format("OrderListing_{0}", DateTime.Now);
			return File(str,"application/pdf",savedFilename);
		}
		// GET: Order/Details/5
		public ActionResult Details(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var order = db.Orders.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
