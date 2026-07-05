namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class SubjectDoesntExistException : Exception
{
    public SubjectDoesntExistException() : base() { }
    public SubjectDoesntExistException(string msg) : base(msg) { }
    public SubjectDoesntExistException(string msg, Exception innerException) : base(msg, innerException) { }
}