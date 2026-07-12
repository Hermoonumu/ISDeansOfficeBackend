using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.DTO;


public class RankingDTO
{
    public Guid? ProgramId { set; get; }
    public EdProgram? Program { set; get; }
    public List<RankingEntryDTO> RankingList { set; get; } = [];
}



public class RankingEntryDTO
{
    public int Rank { set; get; }
    public Guid? StudentId { set; get; }
    public UserDTO? Student { set; get; }
    public double? AverageGrade { set; get; }
    public bool? HasAcademicDebt { set; get; }
}