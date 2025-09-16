interface ISocket
{
    bool Start(ISocket socket);
    bool Stop(ISocket socket);   
    bool Reconnect(ISocket socket);
    string GetMessage<T>(string message);
    void WaitHandle();

}