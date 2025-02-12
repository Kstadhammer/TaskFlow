using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskFlow.Core.Entities;

public class Status : Base
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Project> Projects { get; set; } = [];
}
