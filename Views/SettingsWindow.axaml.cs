using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HasteNotes.ViewModels;

namespace HasteNotes.Views
{
    public partial class SettingsWindow : Window
    {
        public NotesViewModel? OwnerViewModel { get; set; }
        public SettingsWindow()
        {
            InitializeComponent();

            this.Closing += SettingsWindow_Closing;
        }

        public SettingsWindow(SettingsViewModel vm) : this()
        {
            DataContext = vm;
        }
        private void SettingsWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            // Always set IsEditing to false when the window closes
            if (OwnerViewModel != null)
                OwnerViewModel.IsEditing = false;
        }

        // Optional: handle buttons without commands (not recommended if using MVVM)
        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
                vm.SaveSettingsCommand.Execute(null);


            Debug.WriteLine("Settings saved, closing window.");
            if (OwnerViewModel != null)
                OwnerViewModel.IsEditing = false;

            // Close the settings window
            this.Close();
        }

        private void ResetButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
                vm.ResetDefaultsCommand.Execute(null);
        }
    }
}
