using Avalonia.Controls;
using Avalonia.Interactivity;
using HasteNotes.ViewModels;
using HasteNotes.Views;

namespace HasteNotes
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenNotes(string gameTitle, bool hasBosses)
        {
            var viewModel = new NotesViewModel(gameTitle, hasBosses);
            var notes = new Notes
            {
                DataContext = viewModel,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            notes.Show();
            this.Close();
        }

        private void GameButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string gameTitle)
            {
                bool hasBosses = gameTitle != "Haste Notes";
                OpenNotes(gameTitle, hasBosses);
            }
        }
    }

}