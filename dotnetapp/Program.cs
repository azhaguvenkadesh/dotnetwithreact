using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LoanApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnString")));
// Configure JWT authentication
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<LoanApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IUserService, UserService>();



//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"], // Change this to the appropriate audience
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });

builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
//    c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoginAuth", Version = "v1" });
//}
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Create roles if they don't exist
    if (!await roleManager.RoleExistsAsync("admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("admin"));
    }

    if (!await roleManager.RoleExistsAsync("user"))
    {
        await roleManager.CreateAsync(new IdentityRole("user"));
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        //c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoginAuth v1")
        );
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();


// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Console.WriteLine("bye");

app.Run();
