using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Components.Web;

namespace MongoDB_CRUD_API.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string id { get; set; }
        [BsonElement("Name")]
        public string Users_Name { get; set; } = "default_username";

        public string? Users_Surname { get; set; }

        public string Users_Department { get; set; }

        public string Users_RoleID { get; set; }

        public int Users_Salary { get; set; }

        public DateTime Users_EmploymentDate { get; set; } = DateTime.Now;

        public string Users_Login { get; set; } = "default_login";

        public string Users_Password { get; set; } = "default_password";

    }
}
