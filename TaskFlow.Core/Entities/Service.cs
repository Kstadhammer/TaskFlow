using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskFlow.Core.Entities
{
    public class Service : Base
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal HourlyRate { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Project> Projects { get; set; } = [];
    }
}
