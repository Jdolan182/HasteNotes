namespace HasteNotes.Forms
{ 
    partial class NotesForm
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            notesPrev = new Button();
            notesNext = new Button();
            label2 = new Label();
            noteText = new Label();
            noteTitle = new Label();
            label1 = new Label();
            addNotesButton = new Button();
            editNotesButton = new Button();
            label6 = new Label();
            notesLabel = new Label();
            bossNotesPrev = new Button();
            bossNotesNext = new Button();
            label3 = new Label();
            editBossNotesButton = new Button();
            label8 = new Label();
            label4 = new Label();
            bossHP = new Label();
            hpLabel = new Label();
            bossName = new Label();
            bossesNotesLabel = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Control;
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(771, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadToolStripMenuItem, exportToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(108, 22);
            loadToolStripMenuItem.Text = "Load";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(108, 22);
            exportToolStripMenuItem.Text = "Export";
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(notesPrev);
            splitContainer1.Panel1.Controls.Add(notesNext);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(noteText);
            splitContainer1.Panel1.Controls.Add(noteTitle);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(addNotesButton);
            splitContainer1.Panel1.Controls.Add(editNotesButton);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(notesLabel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(bossNotesPrev);
            splitContainer1.Panel2.Controls.Add(bossNotesNext);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(editBossNotesButton);
            splitContainer1.Panel2.Controls.Add(label8);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(bossHP);
            splitContainer1.Panel2.Controls.Add(hpLabel);
            splitContainer1.Panel2.Controls.Add(bossName);
            splitContainer1.Panel2.Controls.Add(bossesNotesLabel);
            splitContainer1.Size = new Size(771, 459);
            splitContainer1.SplitterDistance = 385;
            splitContainer1.TabIndex = 1;
            // 
            // notesPrev
            // 
            notesPrev.BackColor = SystemColors.Control;
            notesPrev.Cursor = Cursors.Hand;
            notesPrev.FlatAppearance.BorderSize = 0;
            notesPrev.FlatStyle = FlatStyle.Flat;
            notesPrev.Location = new Point(-2, 432);
            notesPrev.Name = "notesPrev";
            notesPrev.Size = new Size(75, 23);
            notesPrev.TabIndex = 15;
            notesPrev.Text = "Prev";
            notesPrev.UseVisualStyleBackColor = false;
            notesPrev.Click += NotesPrev_Click;
            // 
            // notesNext
            // 
            notesNext.BackColor = SystemColors.Control;
            notesNext.Cursor = Cursors.Hand;
            notesNext.FlatAppearance.BorderSize = 0;
            notesNext.FlatStyle = FlatStyle.Flat;
            notesNext.Location = new Point(305, 432);
            notesNext.Name = "notesNext";
            notesNext.Size = new Size(75, 23);
            notesNext.TabIndex = 14;
            notesNext.Text = "Next";
            notesNext.UseVisualStyleBackColor = false;
            notesNext.Click += NotesNext_Click;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.Fixed3D;
            label2.Location = new Point(-2, 431);
            label2.Name = "label2";
            label2.Size = new Size(387, 2);
            label2.TabIndex = 13;
            // 
            // noteText
            // 
            noteText.AutoSize = true;
            noteText.Font = new Font("Segoe UI", 14F);
            noteText.Location = new Point(11, 119);
            noteText.Name = "noteText";
            noteText.Size = new Size(0, 25);
            noteText.TabIndex = 12;
            // 
            // noteTitle
            // 
            noteTitle.AutoSize = true;
            noteTitle.Font = new Font("Segoe UI", 18F);
            noteTitle.Location = new Point(12, 40);
            noteTitle.Name = "noteTitle";
            noteTitle.Size = new Size(0, 32);
            noteTitle.TabIndex = 11;
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Location = new Point(-2, 107);
            label1.Name = "label1";
            label1.Size = new Size(387, 2);
            label1.TabIndex = 10;
            // 
            // addNotesButton
            // 
            addNotesButton.BackColor = SystemColors.Control;
            addNotesButton.Cursor = Cursors.Hand;
            addNotesButton.FlatAppearance.BorderSize = 0;
            addNotesButton.FlatStyle = FlatStyle.Flat;
            addNotesButton.Location = new Point(224, 6);
            addNotesButton.Name = "addNotesButton";
            addNotesButton.Size = new Size(75, 23);
            addNotesButton.TabIndex = 9;
            addNotesButton.Text = "Add";
            addNotesButton.UseVisualStyleBackColor = false;
            addNotesButton.Click += AddNotesButton_Click;
            // 
            // editNotesButton
            // 
            editNotesButton.BackColor = SystemColors.Control;
            editNotesButton.Cursor = Cursors.Hand;
            editNotesButton.FlatAppearance.BorderSize = 0;
            editNotesButton.FlatStyle = FlatStyle.Flat;
            editNotesButton.Location = new Point(305, 8);
            editNotesButton.Name = "editNotesButton";
            editNotesButton.Size = new Size(75, 23);
            editNotesButton.TabIndex = 8;
            editNotesButton.Text = "Edit";
            editNotesButton.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            label6.BorderStyle = BorderStyle.Fixed3D;
            label6.Location = new Point(0, 38);
            label6.Name = "label6";
            label6.Size = new Size(387, 2);
            label6.TabIndex = 5;
            // 
            // notesLabel
            // 
            notesLabel.AutoSize = true;
            notesLabel.Font = new Font("Segoe UI", 12F);
            notesLabel.Location = new Point(12, 8);
            notesLabel.Name = "notesLabel";
            notesLabel.Size = new Size(51, 21);
            notesLabel.TabIndex = 0;
            notesLabel.Text = "Notes";
            // 
            // bossNotesPrev
            // 
            bossNotesPrev.BackColor = SystemColors.Control;
            bossNotesPrev.Cursor = Cursors.Hand;
            bossNotesPrev.FlatAppearance.BorderSize = 0;
            bossNotesPrev.FlatStyle = FlatStyle.Flat;
            bossNotesPrev.Location = new Point(2, 432);
            bossNotesPrev.Name = "bossNotesPrev";
            bossNotesPrev.Size = new Size(75, 23);
            bossNotesPrev.TabIndex = 16;
            bossNotesPrev.Text = "Prev";
            bossNotesPrev.UseVisualStyleBackColor = false;
            // 
            // bossNotesNext
            // 
            bossNotesNext.BackColor = SystemColors.Control;
            bossNotesNext.Cursor = Cursors.Hand;
            bossNotesNext.FlatAppearance.BorderSize = 0;
            bossNotesNext.FlatStyle = FlatStyle.Flat;
            bossNotesNext.Location = new Point(307, 432);
            bossNotesNext.Name = "bossNotesNext";
            bossNotesNext.Size = new Size(75, 23);
            bossNotesNext.TabIndex = 16;
            bossNotesNext.Text = "Next";
            bossNotesNext.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.BorderStyle = BorderStyle.Fixed3D;
            label3.Location = new Point(-4, 431);
            label3.Name = "label3";
            label3.Size = new Size(389, 2);
            label3.TabIndex = 11;
            // 
            // editBossNotesButton
            // 
            editBossNotesButton.BackColor = SystemColors.Control;
            editBossNotesButton.Cursor = Cursors.Hand;
            editBossNotesButton.FlatAppearance.BorderSize = 0;
            editBossNotesButton.FlatStyle = FlatStyle.Flat;
            editBossNotesButton.Location = new Point(294, 9);
            editBossNotesButton.Name = "editBossNotesButton";
            editBossNotesButton.Size = new Size(75, 23);
            editBossNotesButton.TabIndex = 10;
            editBossNotesButton.Text = "Edit";
            editBossNotesButton.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            label8.BorderStyle = BorderStyle.Fixed3D;
            label8.Location = new Point(-5, 38);
            label8.Name = "label8";
            label8.Size = new Size(387, 2);
            label8.TabIndex = 6;
            // 
            // label4
            // 
            label4.BorderStyle = BorderStyle.Fixed3D;
            label4.Location = new Point(-7, 107);
            label4.Name = "label4";
            label4.Size = new Size(389, 2);
            label4.TabIndex = 4;
            // 
            // bossHP
            // 
            bossHP.AutoSize = true;
            bossHP.Font = new Font("Segoe UI", 14F);
            bossHP.Location = new Point(58, 70);
            bossHP.Name = "bossHP";
            bossHP.Size = new Size(0, 25);
            bossHP.TabIndex = 3;
            // 
            // hpLabel
            // 
            hpLabel.AutoSize = true;
            hpLabel.Font = new Font("Segoe UI", 14F);
            hpLabel.Location = new Point(12, 70);
            hpLabel.Name = "hpLabel";
            hpLabel.Size = new Size(40, 25);
            hpLabel.TabIndex = 2;
            hpLabel.Text = "HP:";
            // 
            // bossName
            // 
            bossName.AutoSize = true;
            bossName.Font = new Font("Segoe UI", 18F);
            bossName.Location = new Point(10, 38);
            bossName.Name = "bossName";
            bossName.Size = new Size(0, 32);
            bossName.TabIndex = 1;
            // 
            // bossesNotesLabel
            // 
            bossesNotesLabel.AutoSize = true;
            bossesNotesLabel.Font = new Font("Segoe UI", 12F);
            bossesNotesLabel.Location = new Point(12, 8);
            bossesNotesLabel.Name = "bossesNotesLabel";
            bossesNotesLabel.Size = new Size(57, 21);
            bossesNotesLabel.TabIndex = 0;
            bossesNotesLabel.Text = "Bosses";
            // 
            // BaseForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(771, 483);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            Name = "BaseForm";
            Text = "HasteNotes";
            TopMost = true;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private SplitContainer splitContainer1;
        private Label notesLabel;
        private Label bossesNotesLabel;
        private Label bossName;
        private Label hpLabel;
        private Label bossHP;
        private Label label4;
        private Label label6;
        private Label label8;
        private Button editNotesButton;
        private Button addNotesButton;
        private Button editBossNotesButton;
        private Label noteTitle;
        private Label label1;
        private Label noteText;
        private Label label2;
        private Label label3;
        private Button notesPrev;
        private Button notesNext;
        private Button bossNotesPrev;
        private Button bossNotesNext;
    }
}