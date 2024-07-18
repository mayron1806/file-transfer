using Domain;

namespace API.Dto;

public class GetMeDto {

    public int Id { get; set; }
    public required string Name { get; set; }

    public required string Email { get; set; }
    
    public static GetMeDto Map(User user) => new() { Id = user.Id, Name = user.Name, Email = user.Email };
}