using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HasteNotes.Models;
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

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
                vm.SaveSettingsCommand.Execute(null);

            if (OwnerViewModel != null)
                OwnerViewModel.IsEditing = false;
            this.Close();
        }

        private void ResetButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
                vm.ResetDefaultsCommand.Execute(null);
        }

        private async void OpenFilePicker_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is DefaultNoteFile file)
            {
                if (DataContext is SettingsViewModel vm)
                {
                    try
                    {
                        await vm.SelectDefaultFile(file);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("File picker error: " + ex);
                    }
                }
            }
        }
    }
}
