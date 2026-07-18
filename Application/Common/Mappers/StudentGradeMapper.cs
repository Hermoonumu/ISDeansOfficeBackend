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
            Student = sg.Student is not null ? UserMapper.UserToDTO(sg.Student) : null,
            CurriculumId = sg.CurriculumId,
            SubjectName = sg.Curriculum.Subject.SubjectName,
            Status = sg.Status,
            Grade = sg.Grade,
            GradingDate = sg.GradingDate,
            GradedById = sg.GradedById,
            GradedBy = sg.GradedBy is not null ? UserMapper.UserToDTO(sg.GradedBy) : null
        };
    }
}