using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MongoDB.Driver;
using MongoDB_CRUD_API.Configuration;
using MongoDB_CRUD_API.Models;
using MongoDB_CRUD_API.Controllers;

var builder = WebApplication.CreateBuilder(args);
SymmetricSecurityKey GetSymmetricSecurityKey() =>
       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]));
builder.Services.AddControllers();
builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection("MongoDBconnection"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<MongoDbConfiguration>>();
    return new MongoClient(options.Value.ConnectionString);
});
builder.Services.AddSingleton<EmployeeController>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // óêàçûâàåò, áóäåò ëè âàëèäèðîâàòüñÿ èçäàòåëü ïðè âàëèäàöèè òîêåíà
            ValidateIssuer = true,
            // ñòðîêà, ïðåäñòàâëÿþùàÿ èçäàòåëÿ
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            // áóäåò ëè âàëèäèðîâàòüñÿ ïîòðåáèòåëü òîêåíà
            ValidateAudience = true,
            // óñòàíîâêà ïîòðåáèòåëÿ òîêåíà
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            // áóäåò ëè âàëèäèðîâàòüñÿ âðåìÿ ñóùåñòâîâàíèÿ
            ValidateLifetime = true,
            // óñòàíîâêà êëþ÷à áåçîïàñíîñòè
            IssuerSigningKey = GetSymmetricSecurityKey(),
            // âàëèäàöèÿ êëþ÷à áåçîïàñíîñòè
            ValidateIssuerSigningKey = true,
        };
    });


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Äîáàâüòå àâòîðèçàöèþ
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();
// óñëîâíàÿ áä ñ ïîëüçîâàòåëÿìè
app.UseStaticFiles(); // Enable serving static files (such as HTML, CSS, JS) from wwwroot directory.
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" } // Set the default file to index.html
});
app.Map("/data", [Authorize] () => new { message = "Hello World!" });
app.Run();



record class User(string Name, string Password);