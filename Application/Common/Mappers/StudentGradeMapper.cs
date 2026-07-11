using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Common.Mappers;



public class StudentGradeMapper
{
    public static StudentGradeDTO SGToDTO(StudentGrade sg)
    {
        return new StudentGradeDTO()
        {
            Id = sg.Id,
            StudentId = sg.StudentId,
            Student = UserMapper.UserToDTO(sg.Student),
            CurriculumId = sg.CurriculumId,
            Curriculum = sg.Curriculum,
            Status = sg.Status,
            Grade = sg.Grade,
            PassedDate = sg.PassedDate,
            GradedById = sg.GradedById,
            GradedBy = UserMapper.UserToDTO(sg.GradedBy)
        };
    }
}