namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class CurriculumDoesntExistException : Exception
{
    public CurriculumDoesntExistException() : base() { }
    public CurriculumDoesntExistException(string msg) : base(msg) { }
    public CurriculumDoesntExistException(string msg, Exception innerException) : base(msg, innerException) { }
}