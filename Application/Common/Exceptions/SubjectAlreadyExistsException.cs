namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class SubjectAlreadyExistsException : Exception
{
    public SubjectAlreadyExistsException() : base() { }
    public SubjectAlreadyExistsException(string msg) : base(msg) { }
    public SubjectAlreadyExistsException(string msg, Exception innerException) : base(msg, innerException) { }
}