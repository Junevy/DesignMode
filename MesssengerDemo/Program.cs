using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

// Console.WriteLine("hello package");
MainViewMode mainViewMode = new();
DemoViewModel demoViewModel = new()
{
    Name = "Junevy"
};

class MainViewMode 
{
    public MainViewMode()
    {
        WeakReferenceMessenger.Default.Register<PropertyChangedMessage<string>>(this, OnNameChanged);
        WeakReferenceMessenger.Default.Register<ValueChangedMessage<string>>(this, OnValueChanged);
    }

    private void OnValueChanged(object recipient, ValueChangedMessage<string> message)
    {
        throw new NotImplementedException();
    }

    private void OnNameChanged2(object recipient, string message)
    {
        Console.WriteLine($"Recipient: {nameof(recipient)} received message : {message}");
    }

    private void OnNameChanged(object recipient, PropertyChangedMessage<string> message)
    {
        Console.WriteLine($"Recipient: {nameof(recipient)} received message : {message.NewValue}");
    }

    // public void Receive(T message)
    // {
    //     throw new NotImplementedException();
    // }
}

partial class DemoViewModel : ObservableObject
{
    // [ObservableProperty]
    private string name = "hello";
    public string Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value))
            {
                WeakReferenceMessenger.Default.
                    Send(new PropertyChangedMessage<string>(this, nameof(name), "null", value));
                // WeakReferenceMessenger.Default.Send<string>(value);
            }
        }
    }
}

