using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CubeItMVC.Models
{
    [Alias("Users")]
    public class UserModel
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string name { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreateUser(UserModel model, OrmLiteConnectionFactory dbFactory)
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

    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class CreateUserResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public bool IsSuccess { get; set; }
    }
}