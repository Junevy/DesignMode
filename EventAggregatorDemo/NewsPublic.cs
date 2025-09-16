using System.Globalization;

class Subscriber
{
    public string Name { get; set; }
    public Subscriber(string name)
    {
        this.Name = name;
        NewsAggregator.Instance.Subscribe(this, ReadNews);

    }
    private void ReadNews(FullNews news)
    {
        Console.WriteLine($"{this.Name} received a news : {news.news} from {news.topic}");
    }
}

/// <summary>
/// 消息的结构
/// </summary>
class FullNews
{
    public string topic { get; set; }
    public string news { get; set; }
    public FullNews(string topic, string news)
    {
        this.topic = topic;
        this.news = news;
    }
}

/// <summary>
/// 事件中心\聚合器,负责分发消息 至订阅消息的对象.
/// </summary>
class NewsAggregator
{
    /// <summary>
    /// 存储订阅信息的对象. 包含 订阅者 和 接收到消息后的行为 两个属性
    /// </summary>
    class SubscribeInfo
    {
        public Subscriber subser;
        public Action<FullNews> read;

        public SubscribeInfo(Subscriber subser, Action<FullNews> read)
        {
            this.subser = subser;
            this.read = read;
        }
    }

    /// <summary>
    /// 存储所有订阅者的信息
    /// </summary>
    private Dictionary<Type, List<SubscribeInfo>> subscriberList = [];

    public static NewsAggregator Instance { get; } = new();

    public void Subscribe(Subscriber subscriber, Action<FullNews> action)
    {
        var type = typeof(FullNews);
        if (!subscriberList.ContainsKey(type))
            subscriberList[type] = [];
        // if(subscriberList[type].Contains())
        subscriberList[type].Add(new SubscribeInfo(subscriber, action));
    }

    public void Publish(FullNews fullNews)
    {
        if (!subscriberList.ContainsKey(typeof(FullNews))) return;
        foreach (var suber in subscriberList[typeof(FullNews)])
        {
            suber.read.Invoke(fullNews);
        }
    }
}