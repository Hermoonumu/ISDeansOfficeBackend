namespace DeanInfoSystem.Application.Enrollment;


public interface IEnrollmentService
{
    public Task EnrollStudentIntoProgramAsync(Guid StudentId, Guid ProgramId);
    public Task UpdateStudentGradesOnNewCurriculumAsync(Guid NewCurriculumId);

}