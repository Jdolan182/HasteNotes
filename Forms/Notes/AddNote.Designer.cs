namespace HasteNotes.Forms.Notes
{
    partial class AddNote
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            noteTitle = new TextBox();
            cancelButton = new Button();
            addButton = new Button();
            noteTextBox = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F);
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(108, 30);
            label1.TabIndex = 0;
            label1.Text = "Note Title";
            // 
            // noteTitle
            // 
            noteTitle.Location = new Point(12, 45);
            noteTitle.Name = "noteTitle";
            noteTitle.Size = new Size(742, 23);
            noteTitle.TabIndex = 1;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(643, 423);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(111, 37);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // addButton
            // 
            addButton.Location = new Point(516, 423);
            addButton.Name = "addButton";
            addButton.Size = new Size(111, 37);
            addButton.TabIndex = 3;
            addButton.Text = "Add";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // noteTextBox
            // 
            noteTextBox.Location = new Point(12, 157);
            noteTextBox.Multiline = true;
            noteTextBox.Name = "noteTextBox";
            noteTextBox.Size = new Size(742, 247);
            noteTextBox.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 16F);
            label2.Location = new Point(12, 124);
            label2.Name = "label2";
            label2.Size = new Size(61, 30);
            label2.TabIndex = 5;
            label2.Text = "Note";
            // 
            // AddNote
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(766, 472);
            Controls.Add(label2);
            Controls.Add(noteTextBox);
            Controls.Add(addButton);
            Controls.Add(cancelButton);
            Controls.Add(noteTitle);
            Controls.Add(label1);
            Name = "AddNote";
            Text = "AddNote";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox noteTitle;
        private Button cancelButton;
        private Button addButton;
        private TextBox noteTextBox;
        private Label label2;
    }
}