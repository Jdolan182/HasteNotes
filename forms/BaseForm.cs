namespace HasteNotes.forms
{
    public partial class BaseForm : Form
    {


        public BaseForm()
        {
            InitializeComponent();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var gameSelection = new GameSelection();
            gameSelection.StartPosition = FormStartPosition.Manual;
            gameSelection.Location = this.Location;
            this.Hide();
            gameSelection.Show();
            this.Dispose();
        }
    }
}
