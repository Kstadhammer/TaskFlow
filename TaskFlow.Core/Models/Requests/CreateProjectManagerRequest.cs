namespace TaskFlow.Core.Models.Requests;

public class CreateProjectManagerRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Department { get; set; } = null!;
    public string Title { get; set; } = null!;
}
