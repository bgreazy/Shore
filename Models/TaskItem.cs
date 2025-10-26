using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitting.Models
{
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartTime { get; set; } // ISO 8601
        public string EndTime { get; set; }
        public string AssignedTo { get; set; } // UID or null
        public string Status { get; set; } // "available" or "assigned"
    }
}
