using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

//序列化
SerializeViewModel svm = new() { Name = "junevy", Age = 20};
var json = JsonSerializer.Serialize(svm);
Console.WriteLine(json);

//反序列化
var jsonObj = JsonSerializer.Deserialize<SerializeViewModel>(json);
Console.WriteLine(jsonObj?.Name);

//测试Messenger
IMessenger messenger = WeakReferenceMessenger.Default;
var L1 = new Listener(messenger, msg => Console.WriteLine($"L1 Received : {msg}"));
var L2 = new Listener(messenger, msg => Console.WriteLine($"L2 Received : {msg}"));
messenger.Send<SimpleMessage>(new SimpleMessage("hello messenger."));


partial class SerializeViewModel : ObservableObject
{
    [ObservableProperty]
    [property: JsonPropertyName("EN Name")] //修改序列化后键值对key的名称。
    string name;

    [ObservableProperty]
    [property: JsonPropertyName("Expect Age")]
    int age;
}

partial class RelayCommandAttributeDemo : ObservableObject
{

    [ObservableProperty]
    bool isActive;

    /// AllowConcurrentExecutions => 允许并发（简单来讲按钮可多次点击）
    /// IncludeCancelCommand => 可取消的Task，参数需传递CancellationToken
    /// 自动生成 GetTaskCancellCommand
    [RelayCommand(CanExecute = nameof(IsActive), AllowConcurrentExecutions = true, IncludeCancelCommand = true)]
    async Task GetTask(CancellationToken cancellationTokenSource)
    {
        await Task.Delay(2500);
        Console.WriteLine("Test");
    }


    //模拟情况：UI界面有个Button Binding了这个Command，且CommandParameter Binding isActive。
    [RelayCommand]
    void TestParameter(bool? isActive)
    {
        Console.WriteLine(isActive);
    }
    //实际在Binding isActive时，无论isActive是true还是false，按钮始终处于灰色。
    //这是因为RelayCommand对接收的类型参数有个特殊要求：必须为引用类型。
    //最简单的解决办法：修改 bool isActive => bool? isActive （NullAble类型也是引用类型）。
}

/// <summary>
/// 测试Messenger
/// </summary>
class Listener
{
    public Listener(IMessenger messenger, Action<string> method)
    {
        //当接收到消息时，将会自动调用注册的匿名函数
        // (o, m) =>
        // {
        //     method(m.message);
        //     Console.WriteLine(o.ToString());
        //     // Console.WriteLine(typeof(o).ToString());
        // } );

        //其中，将this（即接收消息的对象）作为参数1，Send的msg作为参数2，传递给这个匿名方法。
        messenger.Register<SimpleMessage>(this,  (o, m) =>
        {
            method(m.message);
            Console.WriteLine(o.ToString());
            // Console.WriteLine(typeof(o).ToString());
        } );
        // messenger.Register<SimpleMessage>(this,);
    }
}

record SimpleMessage(string message);