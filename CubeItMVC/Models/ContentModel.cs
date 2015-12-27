using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CubeItMVC.Models
{
    [Alias("Content")]
    public class ContentModel
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreateContent(ContentModel model, int userid, OrmLiteConnectionFactory dbFactory)
        {
            int lastInsertedId = 0;
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                dbConn.Save(model);
                lastInsertedId = model.Id;
                ContentUserMappingModel mapping = new ContentUserMappingModel()
                {
                    ContentId = lastInsertedId,
                    UserId = userid,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                dbConn.Insert<ContentUserMappingModel>(mapping);
            }
            return lastInsertedId;
        }
    }

    public class CreateContentRequest
    {
        public string Link { get; set; }
        public int UserId { get; set; }
    }

    public class CreateContentResponse
    {
        public int id { get; set; }
        public string link { get; set; }
        public int userid { get; set; }
        public bool IsSuccess { get; set; }
    }

    [Alias("ContentUsersMapping")]
    public class ContentUserMappingModel
    {
        [AutoIncrement]
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }

    [Alias("CubeContentMapping")]
    public class CubeContentMappingModel
    {
        [AutoIncrement]
        public int Id { get; set; }
        public int CubeId { get; set; }
        public int ContentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int AddContentToCube(CubeContentMappingModel model, int userid, OrmLiteConnectionFactory dbFactory)
        {
            int lastInsertedId = 0;
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                CubeContentMappingModel mapping = new CubeContentMappingModel()
                {
                    CubeId = model.CubeId,
                    ContentId = model.ContentId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                dbConn.Save(mapping);
                lastInsertedId = model.Id;
                var cubes = dbConn.Select<CubeUserMappingModel>("select * from cubeusermapping where cubeid = " + model.CubeId + " and isactive = 1");
                var userids = cubes.Select(x => x.UserId).ToList();
                userids.Remove(userid);
                if (userids != null && userids.Count > 0)
                {
                    userids.ForEach(x =>
                    {
                        ContentUserMappingModel mapping1 = new ContentUserMappingModel()
                        {
                            ContentId = model.ContentId,
                            UserId = x,
                            ModifiedDate = DateTime.Now,
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        };
                        dbConn.Save(mapping1);
                    });
                }
            }
            return lastInsertedId;
        }
    }

    public class AddContentToCubeRequest
    {
        public int content_id { get; set; }
        public int UserId { get; set; }
        public int CubeId { get; set; }
    }

    public class AddContentToCubeResponse
    {
        public int id { get; set; }
        public int cubeid { get; set; }
        public int contentid { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class DeleteContentFromCube
    {
        public int UserId { get; set; }
        public int CubeId { get; set; }
        public int ContentId { get; set; }
        public bool DeleteContent(DeleteContentFromCube req, OrmLiteConnectionFactory dbFactory)
        {
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                dbConn.ExecuteNonQuery("Update cubecontentmapping set isactive = 0 where cubeid = " + req.CubeId + " and contentid = " + req.ContentId);
                dbConn.ExecuteNonQuery("Update contentusersmapping set isactive = 0 where userid = " + req.UserId + " and contentid = " + req.ContentId);
            }
            return true;
        }
    }

    public class ShareContentRequest
    {
        public int ContentId { get; set; }
        public int user_id { get; set; }
        public int ShareCube(ContentUserMappingModel model, OrmLiteConnectionFactory dbFactory)
        {
            int lastInsertedId = 0;
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                dbConn.Save(model);
                lastInsertedId = model.Id;
            }
            return lastInsertedId;
        }
    }

    public class ShareContentResponse
    {
        public int id { get; set; }
        public int cubeid { get; set; }
        public int userid { get; set; }
    }

    public class GetAllContentRequest
    {
        public int userid { get; set; }
        public List<ContentModel> GetAllContent(int userid, OrmLiteConnectionFactory dbFactory)
        {
            List<ContentModel> contents = new List<ContentModel>();
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                List<ContentUserMappingModel> list = dbConn.Select<ContentUserMappingModel>("Select * from contentusersmapping where userid = " + userid + " and isactive = 1");
                var ids = list.Select(x => x.ContentId).ToList();
                string idArray = String.Join(",", ids);
                if (idArray == "")
                {
                    return contents;
                }
                else
                    contents = dbConn.Select<ContentModel>("Select * from Content where id in (" + idArray + ") and isactive = 1");

                return contents;
            }
        }
    }

    public class GetAllContentResponse
    {
        public GetAllContentResponse()
        {
            contents = new List<ContentResponse>();
        }
        public List<ContentResponse> contents { get; set; }
    }

    public class ContentResponse
    {
        public int id { get; set; }
        public string link { get; set; }
        public int userId { get; set; }
    }
}