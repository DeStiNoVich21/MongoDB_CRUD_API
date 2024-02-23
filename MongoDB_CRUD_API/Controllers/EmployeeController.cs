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
    public class EmployeeController : Controller
    {
        private readonly IMongoCollection<UserRoles> _Userroles;
        private readonly IMongoCollection<UserDepartment> _UserDepartment;
        private readonly IMongoCollection<Employee> _Employee;
        private readonly IConfiguration _configuration;
        private MongoClient _mongoClient;
      
        
        public EmployeeController(IOptions<MongoDbConfiguration> mongoDbConfiguration, IConfiguration configuration)
        {
            _mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString); 

            var mongoDatabase = _mongoClient.GetDatabase(
                mongoDbConfiguration.Value.DatabaseName);

            _Employee = mongoDatabase.GetCollection<Employee>(mongoDbConfiguration.Value.EmployeesCollectionName);
            _Userroles = mongoDatabase.GetCollection<UserRoles>(mongoDbConfiguration.Value.UserRolesCollectionName);
            _UserDepartment = mongoDatabase.GetCollection<UserDepartment>(mongoDbConfiguration.Value.UserDepartmentCollectionName);
            _configuration = configuration;

        }

        [HttpGet]
        [CustomAuthorize]
        public async Task<List<Employee>> GetAsync() =>
            await _Employee.Find(_ => true).ToListAsync();


        [HttpGet("{id}")]
        [CustomAuthorize]
        public async Task<Employee?> GetAsync(string ID) =>
            await _Employee.Find(x => x.id.ToString() == ID).FirstOrDefaultAsync();


        [HttpPost]
        [CustomAuthorize]
        public async Task CreateAsync(Employee NewEmployee)
        {
            if (string.IsNullOrEmpty(NewEmployee.id.ToString()))
            {
                NewEmployee.id = ObjectId.GenerateNewId().ToString();
            }
            await _Employee.InsertOneAsync(NewEmployee);
        }


        [HttpPut]
        [CustomAuthorize]
        public async Task UpdateAsync(String ID, Employee UpdatedEmployee) =>
            await _Employee.ReplaceOneAsync(x => x.id == ID, UpdatedEmployee);

        [HttpDelete]
        [CustomAuthorize]
        public async Task RemoveAsync(String ID) =>
            await _Employee.DeleteOneAsync(x => x.id.ToString() == ID);



        [HttpPost("Login")]
        public async Task<IActionResult> YourAction([FromBody] Users person, IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            if (person == null)
            {
                return BadRequest(); // Если JSON пустой или не соответствует структуре Person
            }

            var foundPerson = await _Employee.Find(p => p.Users_Login == person.Users_Login && p.Users_Password == person.Users_Password).FirstOrDefaultAsync();


            if (foundPerson != null)
            {
                var role = await _Userroles.Find(p=> p.id.ToString()== foundPerson.Users_RoleID).FirstOrDefaultAsync();
                // Запись найдена, выполните нужные действия
                var claims = new List<Claim>
                {
                  new Claim(ClaimTypes.Name, foundPerson.Users_Name),
                  new Claim(ClaimTypes.Role, role.RoleName)
                };

                SymmetricSecurityKey GetSymmetricSecurityKey() =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: _configuration["JwtSettings:Issuer"],
                        audience: _configuration["JwtSettings:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromHours(24)),
                        signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)); ; ;
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var department = await _UserDepartment.Find(p => p.id.ToString() == foundPerson.Users_Department).FirstOrDefaultAsync();

                var response = new
                {
                    access_token = encodedJwt,
                    username = foundPerson.Users_Name,
                };

                return Ok(response);
            }
            else
            {
                // Запись не найдена, верните ошибку или что-то другое
                return NotFound();
            }
        }
    }



}
