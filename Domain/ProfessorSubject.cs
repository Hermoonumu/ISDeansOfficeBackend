namespace DeanInfoSystem.Domain;


public class ProfessorSubject
{
    public Guid Id { set; get; }

    public Guid UserId { set; get; }
    public User User { set; get; }

    public Guid SubjectId { set; get; }
    public Subject Subject { set; get; }
}