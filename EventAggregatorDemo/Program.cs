/*
    @Function:
        将
            1.某个类型的消息
            2.某个对象
            3.接收到消息后对象所执行的方法 
        注册到Dictionary中,
        在调用Aggregator的Send方法后，将需要Send的 某个类型的消息 send给注册了该类型信息的对象。
*/

var rec = new MessageReceiver();
var rec_1 = new MessageReceiver();

EventAggregator.Instance.Send<StringMessage>(new StringMessage("send string message"));
EventAggregator.Instance.Send<IntMessage>(new IntMessage(222));


rec.Register<FloatMessage>(rec.FloatAction);
EventAggregator.Instance.Send<FloatMessage>(new FloatMessage(3.14f));




/// <summary>
/// 订阅-发布模式Demo
/// </summary>
// var Junevy = new Subscriber("Junevy");
// var science = new FullNews("Science", "Human will be forever.");
// var biology = new FullNews("Biology", "Human detected QingMeiSu");
// var geometry = new FullNews("Geometry", "Triangle angle will forever be 180 degree.");

// var Young = new Subscriber("Young");

// NewsAggregator.Instance.Publish(science);
// NewsAggregator.Instance.Publish(biology);


class MessageReceiver
{
    /// <summary>
    /// MessageReceiver对象默认注册的消息类型
    /// </summary>
    public MessageReceiver()
    {
        EventAggregator.Instance.Register<StringMessage>(this, DoSth);
        EventAggregator.Instance.Register<IntMessage>(this, DoSth1);
    }

    /// <summary>
    /// 自定义注册消息类型
    /// </summary>
    /// <typeparam name="T">消息的类型</typeparam>
    /// <param name="action">接收到消息后所执行的委托</param>
    public void Register<T>(Action<T> action)
    {
        EventAggregator.Instance.Register<T>(this, action);
    }

    internal void FloatAction(FloatMessage message)
    {
        Console.WriteLine($"this is float action!!! register successful! :: {message.Message}");
    }

    private void DoSth(StringMessage message)
    {
        Console.WriteLine(message.Message); //?
    }
    private void DoSth1(IntMessage message)
    {
        Console.WriteLine(message.Message); //?
    }
}

/// <summary>
/// 消息类型
/// </summary>
record StringMessage(string Message);
record IntMessage(int Message);
record FloatMessage(float Message);


/// <summary>
/// Event Aggregator
/// 事件聚合器，用于转发消息
/// </summary>
class EventAggregator
{
    public static EventAggregator Instance { get; } = new();

    /// <summary>
    /// Key:注册消息的类型.
    /// Value:
    ///     List<MsgReceiver>
    ///     对象中包含注册者和所执行的回调方法.
    /// <param name="object receiver">消息类型</param>
    /// <param name="Action<object> ac">回调函数</param>
    /// </summary>
    private Dictionary<Type, List<MsgReceiver>> events = [];
    record MsgReceiver(object receiver, Action<object> ac);

    /// <summary>
    /// 注册某个类型的消息,将注册者和回调函数合并为 新的对象，添加到字典中.
    /// </summary>
    /// <typeparam name="T">注册消息的类型</typeparam>
    /// <param name="receiver">注册者</param>
    /// <param name="action">回调函数</param>
    public void Register<T>(object register, Action<T> action)
    {
        var type = typeof(T);
        if (!events.ContainsKey(type))
            events[type] = [];
        events[type].Add(new(register, o => action((T)o)));
    }

    /// <summary>
    /// 遍历注册字典的Key，如果包含所发送的消息类型，则将该消息发送至Value中的所有对象
    /// </summary>
    /// <typeparam name="T">所发送消息的类型</typeparam>
    /// <param name="message">所发送的消息</param>
    public void Send<T>(T message)
    {
        var type = typeof(T);
        if (!events.ContainsKey(type)) return;
        foreach (var rec in events[type])
        {
            rec.ac.Invoke(message!);
        }
    }
}