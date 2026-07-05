namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class EnrollmentException : Exception
{
    public EnrollmentException() : base() { }
    public EnrollmentException(string msg) : base(msg) { }
    public EnrollmentException(string msg, Exception innerException) : base(msg, innerException) { }
}