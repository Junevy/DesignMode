using System.Runtime.CompilerServices;

// 工厂模式示例
IFactory factory = new IPhoneFactory();
var ip = factory.GetPhone();
ip.Call();



