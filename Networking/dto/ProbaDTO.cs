namespace Networking.DTO;

[Serializable]
public class ProbaDTO
{
    public Int64 ID { get; set; }
    
    public Int32 Distanta {get; set;}
    
    public String Stil {get; set;}
    
    public Int32 Participants {get; set;}

    public ProbaDTO(Int64 ID, Int32 Distanta, String Stil, Int32 Participants)
    {
        this.ID = ID;
        this.Distanta = Distanta;
        this.Stil = Stil;
        this.Participants = Participants;
    }
}