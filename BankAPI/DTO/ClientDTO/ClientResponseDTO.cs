using BankAPI.Enum;

namespace BankAPI.DTO.ClientDTO;

public class ClientResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } =  string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public ClientStatus Status { get; set; }
}