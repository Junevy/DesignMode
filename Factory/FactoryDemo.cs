class FactoryDemo
{

}

// 产品
abstract class Phone
{
    public virtual void Call()
    {
        Console.WriteLine("supper call");
    }
}

class IPhone : Phone
{
    public override void Call()
    {
        Console.WriteLine("Iphone call");
    }
}

class SumsungPhone : Phone
{
    public override void Call()
    {
        Console.WriteLine("Sumsang call");
    }
}

// 工厂
interface IFactory
{
    Phone GetPhone();
}

class IPhoneFactory : IFactory
{
    public Phone GetPhone()
    {
        return new IPhone();
    }
}
class SumsangFactory : IFactory
{
    public Phone GetPhone()
    {
        return new SumsungPhone();
    }
}
