string str = "hello junevy";
var typeStr = str.WordCount();
Console.WriteLine(typeStr);   //output: System.String

int num = 2;
var typeInt = num.WordCount(); //output: System.Int32
Console.WriteLine(typeInt);

Demo1 demo1 = new();
string s_2 = "123";
var value_2 = demo1.GetValue(s_2);
Console.WriteLine(value_2);
Console.WriteLine(value_2.GetType());

Console.WriteLine("========================");

Demo<string> demo = new();
string s = "123";
var value_1 = demo.GetValue(s);
Console.WriteLine(value_1);
Console.WriteLine(value_1.GetType());

static class StringExtention
{
    public static Type WordCount<T>(this T str)
    {
        return typeof(T);
    }
}

class Demo<T>
{
    public T GetValue(T key)
    {
        return key;
    }
}
class Demo1
{
    public T GetValue<T>(T key)
    {
        return key;
    }

    public int GetValue(int key)
    {
        Console.WriteLine("this is int");
        return key;
    }

    public string GetValue(string key)
    {
        Console.WriteLine("this is string");

        return key;
    }
}


