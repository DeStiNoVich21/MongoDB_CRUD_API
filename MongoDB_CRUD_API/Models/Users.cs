using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_CRUD_API.Models
{
    public class Users
    {

        public string Users_Login { get; set; } = "default_login";

        public string Users_Password { get; set; } = "default_password";

    }
}
