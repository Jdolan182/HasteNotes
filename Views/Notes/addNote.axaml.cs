using System;
using Avalonia.Controls;
using HasteNotes.ViewModels;

namespace HasteNotes.Views
{
    public partial class AddNoteWindow : Window
    {
        public AddNoteWindow()
        {
            InitializeComponent();

            // Subscribe to DataContext changes
            this.DataContextChanged += AddNoteWindow_DataContextChanged;
        }

        private void AddNoteWindow_DataContextChanged(object? sender, EventArgs e)
        {
            // Avalonia 0.10+ exposes DataContext directly
            if (this.DataContext is AddNoteViewModel vm)
            {
                vm.RequestClose += r =>
                {
                    this.Close(r);
                };
            }
        }
    }
}
