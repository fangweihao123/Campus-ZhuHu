using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using System.Web.Mvc;
using VONE.Models;
using VONE;
using System.Net;
using System.Web;

namespace VONE.Controllers
{
    public class HomeController : Controller
    {
        //private Entities entity = new Entities();
        // GET: Home
            public ViewResult Index()
            {
                return View();
            }


            [HttpGet]
            public ViewResult SignIn()                   //index就是signin的action
            {
                return View();
            }

            [HttpPost]
            public ViewResult SignIn(GuestResponse guestResponse)
            { 
                if (ModelState.IsValid)
                {
                    var api = new DBAPI();
                    if (api.SignIn(guestResponse.Account, guestResponse.Password, guestResponse.Occupation))
                    {
                        string name = api.SearchName(guestResponse.Account, guestResponse.Occupation);
                        HttpCookie cookie1 = new HttpCookie("Account", (guestResponse.Account).ToString());
                        HttpCookie cookie2 = new HttpCookie("user", HttpUtility.UrlEncode(name));
                        HttpCookie cookie3 = new HttpCookie("Occupation", guestResponse.Occupation);
                        cookie1.Expires = DateTime.Now.AddHours(1);
                        cookie2.Expires = DateTime.Now.AddHours(1);
                        cookie3.Expires = DateTime.Now.AddHours(1);
                        Response.Cookies.Add(cookie1);
                        Response.Cookies.Add(cookie2);
                        Response.Cookies.Add(cookie3);
                }
                    return View("Index");
                }
                else
                {
            
                    return View();
                }
            }
            
            [HttpPost]
            public ViewResult SignUp(SignUpForm signupform)
            {
                if (ModelState.IsValid)
                {
                    var person = new STUDENT
                    {
                        S_ID = signupform.Id,
                        NAME=signupform.Account,
                        S_KEY=signupform.Password
                    };

                    using (var db = new Entities())
                    {
                        db.STUDENT.Add(person);
                        db.SaveChanges();
                    }
                        return View("test");
                }
                else
                {
                    return View();
                }
            }

            [HttpGet]
            public ViewResult SignUp()
            {
                return View();
            }

            public ViewResult LogOut()
            {
                Response.Cookies["Account"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Occupation"].Expires = DateTime.Now.AddDays(-1);
                return View("Index");
            }

            public ViewResult Success()
            {
                return View();
            }
          
    }
}