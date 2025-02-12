namespace TaskFlow.Core.Models.Requests;

public class UpdateProjectRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
    public int CustomerId { get; set; }
    public int ProjectManagerId { get; set; }
    public int ServiceId { get; set; }
}
