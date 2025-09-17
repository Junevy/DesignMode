using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

#region Final Output
// {"EN Name":"junevy","Expect Age":20}
// junevy
// L1 Received : hello messenger.
// L2 Received : Chanel 2.
// This is request message
// This is AsyncRequestMessage
#endregion

#region Serialize and Deserialize
//序列化
SerializeViewModel svm = new() { Name = "junevy", Age = 20};
var json = JsonSerializer.Serialize(svm);
Console.WriteLine(json);

//反序列化
var jsonObj = JsonSerializer.Deserialize<SerializeViewModel>(json);
Console.WriteLine(jsonObj?.Name);
#endregion

#region Messenger, with token
//测试Messenger, with token
IMessenger messenger = WeakReferenceMessenger.Default;
var L1 = new Listener(messenger, 1, msg => Console.WriteLine($"L1 Received : {msg}"));
var L2 = new Listener(messenger, 2, msg => Console.WriteLine($"L2 Received : {msg}"));
messenger.Send<SimpleMessage, int>(new SimpleMessage("hello messenger."), 1);
messenger.Send(new SimpleMessage("Chanel 2."), 2);
#endregion

#region RequestMessage
// RequestMessage
var L3 = new Listener(messenger, 3, msg => Console.WriteLine($"L3 Received : {msg}"));
var reply = messenger.Send(new RequestMessage<string>(), 3);

// var L3 = new Listener(messenger, 3, msg => Console.WriteLine($"L3 Received : {msg}"));
// var reply = messenger.Send(new RequestMessage<string>(), 2);
// System.InvalidOperationException : token 2的消息已经被 L2 处理，这里会报错。

Console.WriteLine(reply.Response.ToString());
#endregion

#region AsyncRequestMessage

/// token 2 send RequestMessage，但Reply已经被同为token 2的L2处理完毕，如果把token 4 =》2 则会报错。
/// token 4 没有send RequestMessage，所以不会报错
var L4 = new Listener(messenger, 4, msg => Console.WriteLine($"L4 Received : {msg}"));
L4.RegisterAsyncRequestMessage(messenger, 4);
var response_4 = await messenger.Send(new AsyncRequestMessage<string>(), 4);
Console.WriteLine(response_4);

#endregion

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
    public Listener(IMessenger messenger, int token, Action<string> method)
    {
        //当接收到消息时，将会自动调用注册的匿名函数
        // (o, m) =>
        // {
        //     method(m.message);
        //     Console.WriteLine(o.ToString());
        //     // Console.WriteLine(typeof(o).ToString());
        // } );

        //其中，将this（即接收消息的对象）作为参数1，Send的msg作为参数2，传递给这个匿名方法。
        messenger.Register<SimpleMessage, int>(this, token, (o, m) => method(m.message));
        messenger.Register<RequestMessage<string>, int>(this, token,
            (o, request) => request.Reply("This is request message"));
        // messenger.Register<SimpleMessage>(this,);
    }

    public void RegisterAsyncRequestMessage(IMessenger messenger, int token)
    {
        messenger.Register<AsyncRequestMessage<string>, int>(this, token,
            (obj, msg) => msg.Reply(GetStringAsync()));
    }

    private async Task<string> GetStringAsync()
    {
        await Task.Delay(2000);
        return "This is AsyncRequestMessage";
    }

}

record SimpleMessage(string message);