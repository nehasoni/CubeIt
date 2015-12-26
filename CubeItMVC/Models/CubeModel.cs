using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CubeItMVC.Models
{
    [Alias("Cubes")]
    public class CubeModel
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreateCube(CubeModel model, int userid, OrmLiteConnectionFactory dbFactory)
        {
            int lastInsertedId = 0;
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                dbConn.Save(model);
                lastInsertedId = model.Id;
                CubeUserMappingModel mapping = new CubeUserMappingModel() {
                    CubeId = lastInsertedId,
                    UserId = userid,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                dbConn.Insert<CubeUserMappingModel>(mapping);
            }
            return lastInsertedId;
        }
    }

    public class CreateCubeRequest
    {
        public string Name { get; set; }
        public int UserId { get; set; }
    }

    public class CreateCubeResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public int userid { get; set; }
        public bool IsSuccess { get; set; }
    }

    [Alias("CubeUserMapping")]
    public class CubeUserMappingModel
    {
        [AutoIncrement]
        public int Id { get; set; }
        public int CubeId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class DeleteCube
    {
        public int UserId { get; set; }
        public int CubeId { get; set; }
        public bool DeleteCubeForUser(DeleteCube req, OrmLiteConnectionFactory dbFactory)
        {
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                dbConn.ExecuteNonQuery("Update cubeusermapping set isactive = 0 where cubeid = " + req.CubeId + " and userid = " + req.UserId);
                dbConn.ExecuteNonQuery("Update cubes set isactive = 0 where id = " + req.CubeId );
            }
            return true;
        }
    }

    public class ShareCubeRequest
    {
        public int Cubeid { get; set; }
        public int user_id { get; set; }
        public int ShareCube(CubeUserMappingModel model, OrmLiteConnectionFactory dbFactory)
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

    public class ShareCubeResponse
    {
        public int id {get;set;}
        public int cubeid {get;set;}
        public int userid {get;set;}
    }

    public class GetAllCubesRequest
    {
        public int userid { get; set; }
        public List<CubeModel> GetAllCubes(int userid, OrmLiteConnectionFactory dbFactory)
        {
            List<CubeModel> cubes = new List<CubeModel>();
            using (var dbConn = dbFactory.OpenDbConnection())
            {
                List<CubeUserMappingModel> list = dbConn.Select<CubeUserMappingModel>("Select * from cubeusermapping where userid = " + userid + " and isactive = 1");
                var ids = list.Select(x => x.CubeId).ToList();
                string idArray = String.Join(",",ids);
                if (idArray == "")
                {
                    return cubes;
                }
                else
                    cubes = dbConn.Select<CubeModel>("Select * from Cubes where id in (" + idArray + ") and isactive = 1");
                return cubes;
            }
        }
    }

    public class GetAllCubesResponse
    {
        public GetAllCubesResponse()
        {
            cubes = new List<CubeResponse>();
        }
        public List<CubeResponse> cubes { get; set; }
    }

    public class CubeResponse
    {
        public int id {get;set;}
        public string name {get;set;}
        public int userId {get;set;}
    }

   
}