namespace Networking.JsonProtocol;

public enum ResponseType
{
    OK, ERROR, Participant_Inscris
}

[Serializable]
public class Response
{
    public ResponseType Type { get; }
    public Object Data { get; set; }
    
    public Response(ResponseType type, Object data)
    {
        this.Type = type;
        this.Data = data;
    }
    

    
}