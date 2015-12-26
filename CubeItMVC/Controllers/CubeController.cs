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
    public class CubeController : Controller
    {
        //
        // GET: /Cube/
        public ActionResult Index()
        {
            return View();
        }

        readonly OrmLiteConnectionFactory dbFactory;
        public CubeController()
        {
            dbFactory = new OrmLiteConnectionFactory(ConnString, SqlServerDialect.Provider);
            dbFactory.RegisterConnection("ConString", ConnString, SqlServerDialect.Provider);
        }
        public string ConnString = ConfigurationManager.ConnectionStrings["CubeConnectionString"].ToString();
        [HttpPost]
        public ActionResult CreateCube(CreateCubeRequest req)
        {
            CreateCubeResponse response = new CreateCubeResponse();
            CubeModel model = new CubeModel()
            {
                name = req.Name,
                IsActive = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var res = model.CreateCube(model,req.UserId, dbFactory);
            response.name = req.Name;
            response.id = res;
            response.userid = req.UserId;
            response.IsSuccess = true;
            return Json(response);
        }

        [HttpPost]
        public ActionResult CreateContent(CreateContentRequest req)
        {
            CreateContentResponse response = new CreateContentResponse();
            ContentModel model = new ContentModel()
            {
                Link = req.Link,
                IsActive = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var res = model.CreateContent(model, req.UserId, dbFactory);
            response.link = req.Link;
            response.id = res;
            response.userid = req.UserId;
            response.IsSuccess = true;
            return Json(response);
        }

        [HttpPost]
        public ActionResult AddContentToCube(AddContentToCubeRequest req)
        {
            AddContentToCubeResponse response = new AddContentToCubeResponse();
            CubeContentMappingModel model = new CubeContentMappingModel()
            {
                CubeId = req.CubeId,
                ContentId = req.content_id,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var res = model.AddContentToCube(model, dbFactory);
            response.contentid = req.content_id;
            response.id = res;
            response.cubeid = req.CubeId;
            response.IsSuccess = true;
            return Json(response);
        }

        [HttpGet]
        public JsonResult DeleteContent(DeleteContentFromCube req)
        {
            req.DeleteContent(req, dbFactory);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteCube(DeleteCube req)
        {
            req.DeleteCubeForUser(req, dbFactory);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShareCube(ShareCubeRequest req)
        {
            ShareCubeResponse response = new ShareCubeResponse();
            CubeUserMappingModel model = new CubeUserMappingModel()
            {
                CubeId = req.Cubeid,
                UserId = req.user_id,
                IsActive = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            
            };
            var res = req.ShareCube(model, dbFactory);
            response.cubeid = req.Cubeid;
            response.id = res;
            response.userid = req.user_id;
            return Json(response);
        }

        [HttpPost]
        public ActionResult ShareContent(ShareContentRequest req)
        {
            ShareContentResponse response = new ShareContentResponse();
            ContentUserMappingModel model = new ContentUserMappingModel()
            {
                ContentId = req.ContentId,
                UserId = req.user_id,
                IsActive = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now

            };
            var res = req.ShareCube(model, dbFactory);
            response.cubeid = req.ContentId;
            response.id = res;
            response.userid = req.user_id;
            return Json(response);
        }

        [HttpGet]
        public ActionResult GetAllCubes(GetAllCubesRequest req)
        {
            GetAllCubesResponse response = new GetAllCubesResponse();
            var cubes = req.GetAllCubes(req.userid, dbFactory);
            cubes.ForEach(x =>
            {
                CubeResponse cube = new CubeResponse()
                {
                    id = x.Id,
                    name = x.name,
                    userId = req.userid
                };
                response.cubes.Add(cube);
            });
            return Json(response.cubes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllContents(GetAllContentRequest req)
        {
            GetAllContentResponse response = new GetAllContentResponse();
            var contents = req.GetAllContent(req.userid, dbFactory);
            contents.ForEach(x =>
            {
                ContentResponse content = new ContentResponse()
                {
                    id = x.Id,
                    link = x.Link,
                    userId = req.userid
                };
                response.contents.Add(content);
            });
            return Json(response.contents, JsonRequestBehavior.AllowGet);
        }
	}
}