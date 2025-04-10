using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json.Serialization;


namespace calendar2.Models
{
    public class TaskModel
    {
        public string Name { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [JsonIgnore]
        public UIElement TaskUIElement { get; set; }
    }
}
