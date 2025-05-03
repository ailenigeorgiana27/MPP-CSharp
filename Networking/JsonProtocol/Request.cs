namespace Networking.JsonProtocol;

public enum RequestType
{
    Login, Logout, Register, Filter, Load
}

[Serializable]
public class Request
{
    public RequestType Type { get; }
    public Object Data { get; }

    public Request(RequestType type, Object data)
    {
        this.Type = type;
        this.Data = data;
    }
    
    
}
