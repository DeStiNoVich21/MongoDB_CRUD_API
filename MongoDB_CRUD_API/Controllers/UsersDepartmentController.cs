using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB_CRUD_API.Configuration;
using MongoDB_CRUD_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MongoDB_CRUD_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersDepartmentController
    {


        private readonly IMongoCollection<UserDepartment> _UserDepartment;


        private readonly IConfiguration _configuration;
        private MongoClient _mongoClient;


        public UsersDepartmentController(IOptions<MongoDbConfiguration> mongoDbConfiguration, IConfiguration configuration)
        {
            _mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);

            var mongoDatabase = _mongoClient.GetDatabase(
                mongoDbConfiguration.Value.DatabaseName);


            _UserDepartment = mongoDatabase.GetCollection<UserDepartment>(mongoDbConfiguration.Value.UserRolesCollectionName);

            _configuration = configuration;

        }

        [HttpGet]
        [CustomAuthorize]
        public async Task<List<UserDepartment>> GetAsync() =>
            await _UserDepartment.Find(_ => true).ToListAsync();


        [HttpGet("{id}")]
        [CustomAuthorize]
        public async Task<UserDepartment?> GetAsync(string ID) =>
            await _UserDepartment.Find(x => x.id.ToString() == ID).FirstOrDefaultAsync();


        [HttpPost]
        [CustomAuthorize]
        public async Task CreateAsync(UserDepartment userDepartment)
        {
            if (string.IsNullOrEmpty(userDepartment.id.ToString()))
            {
                userDepartment.id = ObjectId.GenerateNewId().ToString();
            }
            await _UserDepartment.InsertOneAsync(userDepartment);
        }


        [HttpPut]
        [CustomAuthorize]
        public async Task UpdateAsync(String ID, UserDepartment UpdatedDepartment) =>
            await _UserDepartment.ReplaceOneAsync(x => x.id.ToString() == ID, UpdatedDepartment);

        [HttpDelete]
        [CustomAuthorize]
        public async Task RemoveAsync(String ID) =>
            await _UserDepartment.DeleteOneAsync(x => x.id.ToString() == ID);

    }
}
