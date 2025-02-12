namespace TaskFlow.Core.Models.Requests;

public class UpdateStatusRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
