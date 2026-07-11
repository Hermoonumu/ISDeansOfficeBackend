namespace DeanInfoSystem.Application.DTO;



public class GradeBucketsDTO
{
    public Bin Bin { set; get; }
    public int Count { set; get; }
    public double Percentage { set; get; }
}

public enum Bin
{
    Fail,
    Sufficient,
    Satisfactory,
    Good,
    VeryGood,
    Excellent
}