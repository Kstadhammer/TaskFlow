namespace TaskFlow.Core.Models.Requests;

public class CreateServiceRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal HourlyRate { get; set; }
    public bool IsActive { get; set; } = true;
}
