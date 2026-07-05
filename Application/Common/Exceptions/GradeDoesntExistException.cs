
namespace DeanInfoSystem.Application.Common.Exceptions;

[Serializable]
public class GradeDoesntExistException
 : Exception
{
    public GradeDoesntExistException
() : base() { }
    public GradeDoesntExistException
(string msg) : base(msg) { }
    public GradeDoesntExistException
(string msg, Exception innerException) : base(msg, innerException) { }
}