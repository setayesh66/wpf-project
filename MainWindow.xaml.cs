using calendar2.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using calendar2.ViewModels;
using System.Text.Json.Serialization;

namespace calendar2
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; } = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            ViewModel.TaskListChanged += (s, e) => UpdateTaskGrid(); 
            DataContext = ViewModel;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddTaskCommand.Execute(null);
            UpdateTaskGrid();
        }

        public void UpdateTaskGrid()
        {
            UIElement[] preservedElements = ScheduleGrid.Children.Cast<UIElement>()
                                             .Where(el => !(el is Border))
                                             .ToArray();

            ScheduleGrid.Children.Clear();

            foreach (var element in preservedElements)
            {
                ScheduleGrid.Children.Add(element);
            }

            foreach (var task in ViewModel.Tasks)
            {
                if (task.TaskUIElement != null)
                {
                    ScheduleGrid.Children.Remove(task.TaskUIElement);
                }

                Border taskBlock = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#978fcf")),
                    //Background = System.Windows.Media.Brushes.LightPink,
                    BorderBrush = System.Windows.Media.Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Child = new TextBlock
                    {
                        Text = task.Name,
                        FontSize = 13,
                        FontFamily = new System.Windows.Media.FontFamily("Times New Roman"),
                        TextWrapping = TextWrapping.Wrap,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(4)
                    }
                };
                taskBlock.MouseDown += (sender, e) =>
                {
                    ViewModel.SelectedTask = task;
                    HighlightSelectedTask(taskBlock);
                };

                int column = (int)task.Day + 2;
                int startRow = (int)(task.StartTime.TotalMinutes / 60 - 8) + 3; 
                int rowSpan = Math.Max(1, (int)((task.EndTime.TotalMinutes - task.StartTime.TotalMinutes) / 60));

                Grid.SetColumn(taskBlock, column);
                Grid.SetRow(taskBlock, startRow);
                Grid.SetRowSpan(taskBlock, rowSpan);

                ScheduleGrid.Children.Add(taskBlock);

                task.TaskUIElement = taskBlock;
            }
        }

        private void HighlightSelectedTask(Border selectedBorder)
        {
            foreach (var element in ScheduleGrid.Children)
            {
                if (element is Border border)
                {
                    border.BorderBrush = System.Windows.Media.Brushes.Black;
                }
            }
            selectedBorder.BorderBrush = System.Windows.Media.Brushes.Red;
        }
    }
}
