namespace TaskFlow.Core.Models.Requests;

public class CreateStatusRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
