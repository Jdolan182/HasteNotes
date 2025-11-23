using Avalonia.Controls;
using Avalonia.Interactivity;
using HasteNotes.ViewModels;

namespace HasteNotes.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        public SettingsWindow(SettingsViewModel vm) : this()
        {
            DataContext = vm;
        }

        // Optional: handle window closing if you need to prompt or validate
        private void Window_Closing(object? sender, WindowClosingEventArgs e)
        {
            // Example: Cancel closing if some validation fails
            // if (!vm.CanClose()) e.Cancel = true;
        }

        // Optional: handle buttons without commands (not recommended if using MVVM)
        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
                vm.SaveSettingsCommand.Execute(null);

            Close();
        }

        private void ResetButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
                vm.ResetDefaultsCommand.Execute(null);
        }
    }
}
