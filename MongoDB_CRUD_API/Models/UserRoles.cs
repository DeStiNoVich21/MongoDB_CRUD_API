using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_CRUD_API.Models
{
    public class UserRoles
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; } = "default_RoleName";
        public string RoleDescription { get; set; } = "default_RoleDescription";
        // Другие свойства, если необходимо

    }
}
