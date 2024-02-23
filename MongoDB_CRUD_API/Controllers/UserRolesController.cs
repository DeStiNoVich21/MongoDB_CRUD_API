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

    public class UserRolesController
    { 
            private readonly IMongoCollection<UserRoles> _Userroles;
   
            
            private readonly IConfiguration _configuration;
            private MongoClient _mongoClient;


            public UserRolesController(IOptions<MongoDbConfiguration> mongoDbConfiguration, IConfiguration configuration)
            {
                _mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);

                var mongoDatabase = _mongoClient.GetDatabase(
                    mongoDbConfiguration.Value.DatabaseName);

          
                _Userroles = mongoDatabase.GetCollection<UserRoles>(mongoDbConfiguration.Value.UserRolesCollectionName);
           
                _configuration = configuration;

            }

            [HttpGet]
            [CustomAuthorize]
            public async Task<List<UserRoles>> GetAsync() =>
                await _Userroles.Find(_ => true).ToListAsync();


            [HttpGet("{id}")]
            [CustomAuthorize]
            public async Task<UserRoles?> GetAsync(string ID) =>
                await _Userroles.Find(x => x.id.ToString() == ID).FirstOrDefaultAsync();


            [HttpPost]
            [CustomAuthorize]
            public async Task CreateAsync(UserRoles userroles)
            {
                if (string.IsNullOrEmpty(userroles.id.ToString()))
                {
                    userroles.id = ObjectId.GenerateNewId().ToString();
                }
                await _Userroles.InsertOneAsync(userroles);
            }


            [HttpPut]
            [CustomAuthorize]
            public async Task UpdateAsync(String ID, UserRoles UpdatedEmployee) =>
                await _Userroles.ReplaceOneAsync(x => x.id.ToString() == ID, UpdatedEmployee);

            [HttpDelete]
            [CustomAuthorize]
            public async Task RemoveAsync(String ID) =>
                await _Userroles.DeleteOneAsync(x => x.id.ToString() == ID);



            

        
    }
}
