using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;


var builder = WebApplication.CreateBuilder(args);


// Configure a simple authentication scheme
builder.Services.AddAuthentication();     
builder.Services.AddDbContext<TmsDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("TmsDatabase")));   
//builder.Services
//.AddAuthentication("Training")
//.AddScheme<AuthenticationSchemeOptions,TrainingAuthHandler>("Training", 
//Options =>{});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/assessments/results", () =>
{
    return Results.Ok(new
    {
        courseCode = "CS-101",
        studentId = "S-001",
        letterGrade = "A"
    });
})
.RequireAuthorization();

app.Run();
