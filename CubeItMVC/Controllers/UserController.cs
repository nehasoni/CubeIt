using CubeItMVC.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CubeItMVC.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        readonly OrmLiteConnectionFactory dbFactory;
        public UserController()
        {
            dbFactory = new OrmLiteConnectionFactory(ConnString, SqlServerDialect.Provider);
            dbFactory.RegisterConnection("ConString", ConnString, SqlServerDialect.Provider);
        }
        public string ConnString = ConfigurationManager.ConnectionStrings["CubeConnectionString"].ToString();
        [HttpPost]
        public ActionResult CreateUser(CreateUserRequest req)
        {
            CreateUserResponse response = new CreateUserResponse();
            UserModel model = new UserModel()
            {
                name = req.Name,
                City = req.City,
                IsActive = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var res = model.CreateUser(model, dbFactory);
            response.city = req.City;
            response.name = req.Name;
            response.id = res;
            response.IsSuccess = true;
            return Json(response);
        }
	}
}