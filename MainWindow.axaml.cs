using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HasteNotes
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ff9_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new Window
            {
                Title = "FF9 Selected",
                Width = 300,
                Height = 200,
                Content = new TextBlock { Text = "You selected FF9!" }
            };
            dialog.ShowDialog(this);
        }
    }

}