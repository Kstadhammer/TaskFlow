using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Services;

public interface IStatusService : IBaseService<Status, CreateStatusRequest, UpdateStatusRequest>
{
    // Add any status-specific methods here if needed
}
