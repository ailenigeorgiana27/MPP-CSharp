namespace Networking.DTO;

[Serializable]
public class UserDTO
{
    public Int64 ID { get; set; }
    
    public String Username { get; set; }
    
    public String Password { get; set; }

    public UserDTO(long ID, String Username, String Password)
    {
        this.ID = ID;
        this.Username = Username;
        this.Password = Password;
        
    }
}