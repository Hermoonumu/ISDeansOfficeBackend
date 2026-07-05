namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class ProgramDoesntExistException : Exception
{
    public ProgramDoesntExistException() : base() { }
    public ProgramDoesntExistException(string msg) : base(msg) { }
    public ProgramDoesntExistException(string msg, Exception innerException) : base(msg, innerException) { }
}