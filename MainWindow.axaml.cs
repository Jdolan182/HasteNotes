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

        private void OpenNotes(string gameTitle)
        {
            var notes = new Notes
            { 
                DataContext = new NotesViewModel(gameTitle),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            notes.Closed += (_, __) => { this.Show(); this.Activate(); };

            this.Hide();
            notes.Show();

        }

        private void Ff9_Click(object? sender, RoutedEventArgs e)
                => OpenNotes("Final Fantasy IX");
    }

}