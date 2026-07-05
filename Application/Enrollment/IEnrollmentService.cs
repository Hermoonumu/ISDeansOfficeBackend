namespace DeanInfoSystem.Application.Enrollment;


public interface IEnrollmentService
{
    public Task EnrollStudentIntoProgramAsync(string StudentId, string ProgramId);

}