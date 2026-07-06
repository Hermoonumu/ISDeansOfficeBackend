namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class UpdateFailedException : Exception
{
    public UpdateFailedException() : base() { }
    public UpdateFailedException(string msg) : base(msg) { }
    public UpdateFailedException(string msg, Exception innerException) : base(msg, innerException) { }
}