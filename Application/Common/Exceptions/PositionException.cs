namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class PositionException : Exception
{
    public PositionException() : base() { }
    public PositionException(string msg) : base(msg) { }
    public PositionException(string msg, Exception innerException) : base(msg, innerException) { }
}