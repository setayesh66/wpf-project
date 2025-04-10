using System;
using System.Collections.Generic; 
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json; 
using System.Threading.Tasks;
using System.Windows.Input;
using calendar2.Models;
using System.IO;
using calendar2.Data;



namespace calendar2.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event EventHandler TaskListChanged;
        public ObservableCollection<TaskModel> Tasks { get; set; } = new ObservableCollection<TaskModel>();
        public ObservableCollection<DateTime> WeekDates { get; set; } = new ObservableCollection<DateTime>();



        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    UpdateWeekDates();
                    LoadTasksForWeek(); 
                }
            }
        }


        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set { _selectedTask = value; OnPropertyChanged(); }
        }
        

        private string _newTask;
        public string NewTask
        {
            get => _newTask;
            set { _newTask = value; OnPropertyChanged(); }
        }


        private DayOfWeek _selectedDay;
        public DayOfWeek SelectedDay
        {
            get => _selectedDay;
            set { _selectedDay = value; OnPropertyChanged(); }
        }


        private TimeSpan _selectedStartTime = TimeSpan.FromHours(8);
        public TimeSpan SelectedStartTime
        {
            get => _selectedStartTime;
            set { _selectedStartTime = value; OnPropertyChanged(); }
        }


        private TimeSpan _selectedEndTime = TimeSpan.FromHours(9);
        public TimeSpan SelectedEndTime
        {
            get => _selectedEndTime;
            set { _selectedEndTime = value; OnPropertyChanged(); }
        }


        public ICommand AddTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public MainViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            UpdateTaskCommand = new RelayCommand(UpdateTask);

            SelectedDate = DateTime.Today;
            UpdateWeekDates(); 
            LoadTasksForWeek(); 
        }


        private void AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTask)) return;

            var task = new TaskModel
            {
                Name = NewTask,
                Day = SelectedDay,
                StartTime = SelectedStartTime,
                EndTime = SelectedEndTime
            };

            Tasks.Add(task);
            SaveTasksForWeek();
            NewTask = string.Empty;

            TaskListChanged?.Invoke(this, EventArgs.Empty); 
        }

        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                (App.Current.MainWindow as MainWindow)?.ScheduleGrid.Children.Remove(SelectedTask.TaskUIElement);

                Tasks.Remove(SelectedTask);
                SelectedTask = null;
                OnPropertyChanged(nameof(Tasks));
                SaveTasksForWeek();
                TaskListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateTask()
        {
            if (SelectedTask != null)
            {
                SelectedTask.Name = NewTask;
                SelectedTask.Day = SelectedDay;
                SelectedTask.StartTime = SelectedStartTime;
                SelectedTask.EndTime = SelectedEndTime;

                OnPropertyChanged(nameof(Tasks));
                (App.Current.MainWindow as MainWindow)?.UpdateTaskGrid();
                SaveTasksForWeek();
                TaskListChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private void UpdateWeekDates()
        {
            WeekDates.Clear();
            DateTime startOfWeek = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek);

            for (int i = 0; i < 7; i++)
            {
                WeekDates.Add(startOfWeek.AddDays(i));
            }

            OnPropertyChanged(nameof(WeekDates));
        }

        private void LoadTasksForWeek()
        {
            UpdateWeekDates();

            var allTasks = TaskDataService.LoadAllTasks();
            string weekKey = TaskDataService.GetWeekKey(SelectedDate);

            Tasks.Clear();

            if (allTasks.TryGetValue(weekKey, out var weekTasks))
            {
                foreach (var task in weekTasks)
                {
                    Tasks.Add(task);
                }
            }
            (App.Current.MainWindow as MainWindow)?.UpdateTaskGrid();
        }

        private void SaveTasksForWeek()
        {
            var allTasks = TaskDataService.LoadAllTasks();
            string weekKey = TaskDataService.GetWeekKey(SelectedDate);
            allTasks[weekKey] = Tasks.ToList();

            TaskDataService.SaveAllTasks(allTasks);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
