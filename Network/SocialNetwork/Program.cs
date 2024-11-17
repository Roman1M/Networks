using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Identity;
using Data;
using Data.Entities;

var builder = WebApplication.CreateBuilder(args);

string connStr = builder.Configuration.GetConnectionString("LocalDb")!;

builder.Services.AddDbContext<SocialNetworkDbContext>(opt => opt.UseSqlServer(connStr));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SocialNetworkDbContext>();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseMiddleware<GlobalErrorHandler>();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200", "http://localhost:3000", "https://nice-tree-0cc12e410.5.azurestaticapps.net")
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
