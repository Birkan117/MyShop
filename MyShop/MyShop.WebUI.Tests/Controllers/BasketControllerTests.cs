using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;
using MyShopServices;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTests
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //-----------SETTING UP TESTS-----------//
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();

            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(products, baskets);
            var controller = new BasketController(basketService);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);


            //-----------ACTING ON YOUR TESTS-----------//
            //This is to test the service
            //basketService.AddToBasket(httpContext, "1"); 
            controller.AddToBasket("1");

            Basket basket = baskets.Collection().FirstOrDefault();


            //-----------ASSERTING YOUR TESTS-----------//
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);

        }
        
        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //-----------SETTING UP TESTS-----------//
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();

            //Manually adding in products to the db
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 40.00m });
            products.Insert(new Product() { Id = "3", Price = 20.00m });
            //Manually creating the basket
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "3", Quantity = 3 });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);

            var controller = new BasketController(basketService);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            
            //-----------ACTING ON YOUR TESTS---------- -//
            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;
            
            //-----------ASSERTING YOUR TESTS-----------//
            Assert.AreEqual(6, basketSummary.BasketCount);
            Assert.AreEqual(120.00m, basketSummary.BasketTotal);
            
        }
    }
}
