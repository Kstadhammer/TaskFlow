namespace TaskFlow.Core.Models.Requests;

public class UpdateServiceRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal HourlyRate { get; set; }
    public bool IsActive { get; set; }
}
