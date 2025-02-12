using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces.Repositories
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<IEnumerable<Project>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Project>> GetByProjectManagerIdAsync(int projectManagerId);
        Task<IEnumerable<Project>> GetByStatusIdAsync(int statusId);
        Task<IEnumerable<Project>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> ProjectNumberExistsAsync(string projectNumber);
    }
}
