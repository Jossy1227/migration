using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using TmsApi.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
{

using (var scope = app.Services.CreateScope())
{
var dbcontext = scope.ServiceProvider.GetRequiredService<TmsDbContext>();
dbcontext.Database.Migrate(); 
if (!dbcontext.Students.Any())
{
var students = new List<Student>
{
new() { RegistrationNumber = "TMS-2026-0001", Name = "AliceSmith", GPA = 3.8m, IsActive = true },
new() { RegistrationNumber = "TMS-2026-0002", Name = "Bob Jones", GPA = 2.9m, IsActive = true },
new() { RegistrationNumber = "TMS-2026-0003", Name = "Charlie Brown", GPA = 3.4m, IsActive = false },
new() { RegistrationNumber = "TMS-2026-0004", Name = "DianaPrince", GPA = 3.9m, IsActive = true },
new() { RegistrationNumber = "TMS-2026-0005", Name = "EvanWright", GPA = 2.5m, IsActive = true }
};
dbcontext.Students.AddRange(students);
var courses = new List<Course>
{
new() { Code = "CS-101", Title = "Introduction to ComputerScience", Capacity = 30 },
new() { Code = "CS-201", Title = "Data Structures and Algorithms", Capacity = 25 },
new() { Code = "MAT-101", Title = "Calculus I", Capacity =
40 }
};
dbcontext.Courses.AddRange(courses);
dbcontext.SaveChanges();
var enrollments = new List<Enrollment>
{
new() { StudentId = students[0].Id, CourseId = courses[0].Id, Grade = 4.0m },
new() { StudentId = students[0].Id, CourseId = courses[1].Id, Grade = 3.6m },
new() { StudentId = students[1].Id, CourseId = courses[0].Id, Grade = 2.8m },
new() { StudentId = students[3].Id, CourseId = courses[1].Id, Grade = 3.9m }
};
dbcontext.Enrollments.AddRange(enrollments);
dbcontext.SaveChanges();
}
}


builder.Services.AddDbContext<TmsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TmsDatabase"))
        .LogTo(Console.WriteLine, LogLevel.Information) // Log SQL to output window
        .EnableSensitiveDataLogging()); 



// Configure a simple authentication scheme
builder.Services.AddAuthentication();     
//builder.Services
//.AddAuthentication("Training")
//.AddScheme<AuthenticationSchemeOptions,TrainingAuthHandler>("Training", 
//Options =>{});
builder.Services.AddAuthorization();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TmsDbContext>();
    dbContext.Database.Migrate();
}

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

app.Run();}
