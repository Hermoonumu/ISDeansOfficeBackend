namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class DepartmentDoesntExistException : Exception
{
    public DepartmentDoesntExistException() : base() { }
    public DepartmentDoesntExistException(string msg) : base(msg) { }
    public DepartmentDoesntExistException(string msg, Exception innerException) : base(msg, innerException) { }
}