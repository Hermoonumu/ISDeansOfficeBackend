namespace DeanInfoSystem.Application.DTO;

public class EducatorDashboardDTO
{
    public int TotalPendingGrades { get; set; }
    public List<PendingTaskDTO> PriorityTasks { get; set; }

    public int TotalTeachingHours { get; set; }
    public int ActiveCurriculaCount { get; set; }
    public int StudentCount { set; get; }


    public List<SubjectPerformanceDTO> SubjectPerformances { get; set; }
}

public class PendingTaskDTO
{
    public Guid CurriculumId { get; set; }
    public string SubjectName { get; set; }
    public int UngradedStudentsCount { get; set; }
}

public class SubjectPerformanceDTO
{
    public string SubjectName { get; set; }
    public double AverageGrade { get; set; }
    public int PassedCount { get; set; }
    public int FailedCount { get; set; }
}