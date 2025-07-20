namespace TestApp1.DTOs;

public class LOlz
{
    public string Text { get; set; }
    public int Value { get; set; }
    public string LolzIteration(string Text, int Value)
    {
        var result = string.Empty;
        for (int i = 0; i < Value; i++)
            result += Text;
        return result;
    }
}