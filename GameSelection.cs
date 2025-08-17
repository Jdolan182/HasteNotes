using HasteNotes.Forms;

namespace HasteNotes
{
    public partial class GameSelection : Form
    {
        public GameSelection()
        {
            InitializeComponent();
        }

        private void ff9_Click(object sender, EventArgs e)
        {
            var notesForm = new NotesForm("Final Fantasy IX");
            notesForm.StartPosition = FormStartPosition.Manual;
            notesForm.Location = this.Location;
            this.Hide();
            notesForm.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

    }

}
