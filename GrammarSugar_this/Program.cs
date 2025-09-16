string str = "hello junevy";
var typeStr = str.WordCount();
Console.WriteLine(typeStr);   //output: System.String

int num = 2;
var typeInt = num.WordCount(); //output: System.Int32
Console.WriteLine(typeInt);

static class StringExtention
{
    public static Type WordCount<T>(this T str)
    {
        return typeof(T);
    }
}