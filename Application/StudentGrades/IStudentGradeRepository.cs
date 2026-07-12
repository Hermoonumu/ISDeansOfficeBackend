using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.StudentGrades;


public interface IStudentGradeRepository
{
    public Task InstantiateGradeAsync(StudentGrade sg);
    public Task InstantiateGradesRangeAsync(List<StudentGrade> sgs);
    public Task<StudentGrade?> GetStudentGradeByGuidAsync(Guid guid);
    public Task<List<StudentGrade>> GetStudentGradeRangeAsync(List<Guid> sgIds);
    public Task<List<StudentGrade>> GetStudentGradesAsync(Guid StudentId);
    public Task<List<StudentGrade>> GetGradesByCurriculumAsync(Guid CurrId);
    public Task<List<StudentGrade>> GetGradesByEducatorIdAsync(Guid UserId);
    public Task<List<StudentGrade>> GetStudentGradeByStudentIdRangeAsync(List<Guid> UserIds);
    public Task RemoveGradesRangeAsync(List<Guid> sgIds);


}