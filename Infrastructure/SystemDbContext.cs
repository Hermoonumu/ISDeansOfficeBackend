using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;
public class SystemDbContext : DbContext
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<EdProgram> Programs { get; set; }
    public required DbSet<Subject> Subjects { get; set; }
    public required DbSet<Department> Departments { get; set; }
    public required DbSet<Curriculum> Curricula { get; set; }
    public required DbSet<StudentGrade> Grades { get; set; }
    public required DbSet<EducatorCurriculum> EducCurr { set; get; }


    public SystemDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mBuild)
    {
        mBuild.Entity<User>(u =>
        {
            u.HasKey(e => e.Id);
            u.HasIndex(e => e.Username).IsUnique();

            u.HasOne(e => e.Program)
            .WithMany()
            .HasForeignKey(e => e.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);

            u.Property<uint>("ver").IsConcurrencyToken();
        });


        mBuild.Entity<EdProgram>(p =>
        {
            p.HasKey(e => e.Id);

            p.HasOne(e => e.Department)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

            p.Property<uint>("ver").IsConcurrencyToken();
        });


        mBuild.Entity<Subject>(s =>
        {
            s.HasKey(e => e.Id);
            s.HasIndex(e => e.SubjectName).IsUnique();

            s.Property<uint>("ver").IsConcurrencyToken();
        });


        mBuild.Entity<Department>(d =>
        {
            d.HasKey(e => e.Id);
            d.HasIndex(e => e.DepartmentName).IsUnique();

            d.Property<uint>("ver").IsConcurrencyToken();
        });


        mBuild.Entity<Curriculum>(c =>
        {
            c.HasKey(e => e.Id);

            c.HasOne(e => e.EdProgram)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

            c.HasOne(e => e.Subject)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

            c.Property<uint>("ver").IsConcurrencyToken();
        });


        mBuild.Entity<StudentGrade>(sg =>
        {
            sg.HasKey(e => e.Id);

            sg.HasOne(e => e.Curriculum)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

            sg.HasOne(e => e.Student)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

            sg.Property<uint>("ver").IsConcurrencyToken();
        });


        mBuild.Entity<EducatorCurriculum>(ec =>
            {
                ec.HasKey(e => e.Id);

                // Prevent duplicate assignments
                ec.HasIndex(e => new { e.UserId, e.CurriculumId }).IsUnique();

                ec.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

                ec.HasOne(e => e.Curriculum)
                  .WithMany()
                  .HasForeignKey(e => e.CurriculumId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

    }



}