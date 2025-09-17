using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

//序列化
SerializeViewModel svm = new() { Name = "junevy", Age = 20};
var json = JsonSerializer.Serialize(svm);
Console.WriteLine(json);

//反序列化
var jsonObj = JsonSerializer.Deserialize<SerializeViewModel>(json);
Console.WriteLine(jsonObj?.Name);

partial class SerializeViewModel : ObservableObject
{
    [ObservableProperty]
    [property: JsonPropertyName("EN Name")] //修改序列化后键值对key的名称。
    string name;

    [ObservableProperty]
    [property: JsonPropertyName("Expect Age")]
    int age;
}