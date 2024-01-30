using AppleStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AppleStores.Controllers
{
    public class ShoppingCartController : Controller
    {
		AppleDatabaseEntities db = new AppleDatabaseEntities();
		private string strCart = "Cart";
		// GET: ShoppingCart
		public ActionResult Index()
        {
            return View();
        }
		public ActionResult OrderNow(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (Session[strCart] == null)
			{
				List<Cart> lstCart = new List<Cart>
				{
					new Cart(db.Products.Find(id),1)
				};
				Session[strCart] = lstCart;
			}
			else
			{
				List<Cart> lstCart = (List<Cart>)Session[strCart];
				//this step below are added for update quantity of data
				int check = IsExistingCheck(id);
				if (check == -1)
					lstCart.Add(new Cart(db.Products.Find(id), 1));
				else
					lstCart[check].Quantity++;
				Session[strCart] = lstCart;
			}

			return View("Index");
			
		}
		private int IsExistingCheck(int? id)
		{
			List<Cart> lstCart = (List<Cart>)Session[strCart];

			for (int i=0; i<lstCart.Count; i++)
			{
				if (lstCart[i].Product.ProductId == id) return i;
			}
			return -1;

		}
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			int check = IsExistingCheck(id);
			List<Cart> lstCart = (List<Cart>)Session[strCart];
			lstCart.RemoveAt(check);
			return View("Index");
		}
	}
}