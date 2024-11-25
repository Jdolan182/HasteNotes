using HasteNotes.forms.FF9;

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
            var ff9 = new FF9();
            ff9.StartPosition = FormStartPosition.Manual;
            ff9.Location = this.Location;
            this.Hide();
            ff9.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

    }

}
