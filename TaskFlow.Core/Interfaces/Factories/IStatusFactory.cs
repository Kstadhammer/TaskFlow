using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Factories;

public interface IStatusFactory : IBaseFactory<Status, CreateStatusRequest, UpdateStatusRequest>
{
    // Add any status-specific factory methods here if needed
}
