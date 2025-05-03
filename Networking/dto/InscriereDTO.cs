using Model;

namespace Networking.DTO;

[Serializable]
public class InscriereDTO
{
    public Int64 id { get; }
    public Int32 age { get; }
    public String name { get; }
    public Int64[] probaIds { get; }
    
    public InscriereDTO(Int64 id, Int32 age, String name, Int64[] probaIds)
    {
        this.id = id;
        this.age = age;
        this.name = name;
        this.probaIds = probaIds;
    }
    
}