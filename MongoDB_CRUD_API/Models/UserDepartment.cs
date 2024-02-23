using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_CRUD_API.Models
{
    public class UserDepartment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        //Commit test
        public string id { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; } = "default_RoleName";
        public string DepartmentDescription { get; set; } = "default_RoleDescription";
        // Другие свойства, если необходимо
    }
}
