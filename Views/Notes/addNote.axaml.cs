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
