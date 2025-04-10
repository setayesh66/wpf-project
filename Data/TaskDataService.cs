using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using calendar2.Models;

namespace calendar2.Data
{
    public static class TaskDataService
    {
        private const string FilePath = "tasks.json";

        public static Dictionary<string, List<TaskModel>> LoadAllTasks()
        {
            if (!File.Exists(FilePath)) return new Dictionary<string, List<TaskModel>>();
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Dictionary<string, List<TaskModel>>>(json)
                   ?? new Dictionary<string, List<TaskModel>>();
        }

        public static void SaveAllTasks(Dictionary<string, List<TaskModel>> allTasks)
        {
            string json = JsonSerializer.Serialize(allTasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        public static string GetWeekKey(DateTime date)
        {
            DateTime startOfWeek = date.AddDays(-(int)date.DayOfWeek);
            return startOfWeek.ToString("yyyy-MM-dd");
        }
    }
}
