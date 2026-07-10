using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.StudentGrades;


public interface IStudentGradeService
{
    public Task GradeAsync(User user, Guid cardId, uint grade);
    public Task BulkGradeAsync(User user, List<BulkGradeDTO> bgDTO);
    public Task<List<StudentGrade>> GetStudentGradesAsync(Guid StudentId);
    public Task<List<StudentGrade>> GetGradesByCurriculumAsync(Guid CurrId);
    public Task<List<StudentGrade>> GetGradesByEducatorIdAsync(Guid UserId);
}