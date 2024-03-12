﻿using AppleStores.Models;
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
		public ActionResult UpdateCart(FormCollection frc)
		{
			string[] quantities = frc.GetValues("quantity");
			List<Cart> lstCart = (List<Cart>)Session[strCart];
			for(int i = 0; i < lstCart.Count; i++)
			{
				lstCart[i].Quantity = Convert.ToInt32(quantities[i]);
			}
			Session[strCart] = lstCart;
			return View("Index");
		}

		public ActionResult CheckOut()
		{
			
			return View("CheckOut");
		}

		public ActionResult ProcessOrder(FormCollection frc)
		{
			//1. save the order into order table
			List<Cart> lstCart = (List<Cart>)Session[strCart];
			Order order = new Order()
			{
				CustomerName = frc["cusName"],
				CustomerPhone = frc["cusPhone"],
				CustomerEmail = frc["cusEmail"],
				CustomerAddress = frc["cusAddress"],
				OrderDate = DateTime.Now,
				PaymentType = "Cash",
				Status = "Processing"

			};

			db.Orders.Add(order);
			db.SaveChanges();

			//2. save the order into order detail table
			foreach(Cart cart in lstCart)
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					OrderID = order.OrderID,
					ProductID = cart.Product.ProductId,
					Quantity = cart.Quantity,
					Price = cart.Product.Price
				};

				db.OrderDetails.Add(orderDetail);
				db.SaveChanges(); 
			}
			//3. Remove shopping cart session
			Session.Remove(strCart);
			return View("OrderSuccess");
		}
	}
}