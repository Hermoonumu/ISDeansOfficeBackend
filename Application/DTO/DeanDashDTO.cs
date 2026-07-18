using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.DTO;



public class DeanDashDTO
{
    public required int AllStudentsCount { set; get; }
    public required int TeachingStaffCount { set; get; }
    public required int ActiveProgramCount { set; get; }

    public required List<Curriculum> MissingEducatorCurricula { set; get; }
    public required int PendingGradeCount { set; get; }


    public required List<StudentDeanDashDTO> EndangeredStudents { set; get; }

    public required List<StudentDeanDashDTO> ScholarshipWorthy { set; get; }


}