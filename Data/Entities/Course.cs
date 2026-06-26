namespace TmsApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Courses
{
    public class Course
{
public int Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public int MaxCapacity { get; set; }    
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

}
//Confirm that CourseConfiguration has the rules for both properties configured:
public void Configure(EntityTypeBuilder<Course> builder)
{
builder.HasKey(c => c.Id);
        builder.Property(c => c.Code).IsRequired().HasMaxLength(10);
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.HasIndex(c => c.Code).IsUnique();
        builder.HasMany(c => c.Enrollments)
               .WithOne(e => e.Course)
               .HasForeignKey(e => e.CourseId);
}
// Navigation property for many-to-many relationship
}

