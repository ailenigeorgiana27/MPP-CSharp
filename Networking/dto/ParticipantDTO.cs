namespace Networking.DTO;

[Serializable]
public class ParticipantDTO
{
    public Int64 id { get; }
    public Int32 age { get; }
    public String name { get; }
    public Int32 noProbe { get; }
    
    public ParticipantDTO(Int64 id, Int32 age, String name, Int32 noProbe)
    {
        this.id = id;
        this.age = age;
        this.name = name;
        this.noProbe = noProbe;
    }
}