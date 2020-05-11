namespace THRandomizer
{
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rngBullets = new System.Windows.Forms.CheckBox();
            this.rngDiffFlags = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.rngEntity = new System.Windows.Forms.CheckBox();
            this.rngTimers = new System.Windows.Forms.CheckBox();
            this.seed = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ignoreVar = new System.Windows.Forms.CheckBox();
            this.randMax = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bulletMax = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.randMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bulletMax)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(418, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(65, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(347, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ECL File";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Touhou 6-9",
            "Touhou 10-12.5",
            "Touhou 13+"});
            this.comboBox1.Location = new System.Drawing.Point(65, 41);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(185, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(65, 186);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(140, 37);
            this.button2.TabIndex = 4;
            this.button2.Text = "Randomize";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Game";
            // 
            // rngBullets
            // 
            this.rngBullets.AutoSize = true;
            this.rngBullets.Checked = true;
            this.rngBullets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rngBullets.Location = new System.Drawing.Point(65, 94);
            this.rngBullets.Name = "rngBullets";
            this.rngBullets.Size = new System.Drawing.Size(113, 17);
            this.rngBullets.TabIndex = 8;
            this.rngBullets.Text = "Randomize Bullets";
            this.rngBullets.UseVisualStyleBackColor = true;
            // 
            // rngDiffFlags
            // 
            this.rngDiffFlags.AutoSize = true;
            this.rngDiffFlags.Location = new System.Drawing.Point(65, 163);
            this.rngDiffFlags.Name = "rngDiffFlags";
            this.rngDiffFlags.Size = new System.Drawing.Size(150, 17);
            this.rngDiffFlags.TabIndex = 9;
            this.rngDiffFlags.Text = "Randomize Difficulty Flags";
            this.rngDiffFlags.UseVisualStyleBackColor = true;
            this.rngDiffFlags.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Console";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Location = new System.Drawing.Point(15, 255);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(431, 208);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // rngEntity
            // 
            this.rngEntity.AutoSize = true;
            this.rngEntity.Checked = true;
            this.rngEntity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rngEntity.Location = new System.Drawing.Point(65, 117);
            this.rngEntity.Name = "rngEntity";
            this.rngEntity.Size = new System.Drawing.Size(116, 17);
            this.rngEntity.TabIndex = 10;
            this.rngEntity.Text = "Randomize Entities";
            this.rngEntity.UseVisualStyleBackColor = true;
            // 
            // rngTimers
            // 
            this.rngTimers.AutoSize = true;
            this.rngTimers.Location = new System.Drawing.Point(65, 140);
            this.rngTimers.Name = "rngTimers";
            this.rngTimers.Size = new System.Drawing.Size(113, 17);
            this.rngTimers.TabIndex = 11;
            this.rngTimers.Text = "Randomize Timers";
            this.rngTimers.UseVisualStyleBackColor = true;
            // 
            // seed
            // 
            this.seed.Location = new System.Drawing.Point(65, 68);
            this.seed.Name = "seed";
            this.seed.Size = new System.Drawing.Size(185, 20);
            this.seed.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Seed";
            // 
            // ignoreVar
            // 
            this.ignoreVar.AutoSize = true;
            this.ignoreVar.Checked = true;
            this.ignoreVar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ignoreVar.Location = new System.Drawing.Point(226, 94);
            this.ignoreVar.Name = "ignoreVar";
            this.ignoreVar.Size = new System.Drawing.Size(150, 17);
            this.ignoreVar.TabIndex = 14;
            this.ignoreVar.Text = "Overwrite Variable Access";
            this.ignoreVar.UseVisualStyleBackColor = true;
            // 
            // randMax
            // 
            this.randMax.Location = new System.Drawing.Point(333, 116);
            this.randMax.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.randMax.Name = "randMax";
            this.randMax.Size = new System.Drawing.Size(83, 20);
            this.randMax.TabIndex = 15;
            this.randMax.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(223, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Limit Integers to:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(223, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Limit Bullet counts to:";
            // 
            // bulletMax
            // 
            this.bulletMax.Location = new System.Drawing.Point(333, 142);
            this.bulletMax.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.bulletMax.Name = "bulletMax";
            this.bulletMax.Size = new System.Drawing.Size(83, 20);
            this.bulletMax.TabIndex = 17;
            this.bulletMax.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 475);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bulletMax);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.randMax);
            this.Controls.Add(this.ignoreVar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.seed);
            this.Controls.Add(this.rngTimers);
            this.Controls.Add(this.rngEntity);
            this.Controls.Add(this.rngDiffFlags);
            this.Controls.Add(this.rngBullets);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Touhou Randomizer";
            ((System.ComponentModel.ISupportInitialize)(this.randMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bulletMax)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox rngBullets;
        private System.Windows.Forms.CheckBox rngDiffFlags;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox rngEntity;
        private System.Windows.Forms.CheckBox rngTimers;
        private System.Windows.Forms.TextBox seed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ignoreVar;
        private System.Windows.Forms.NumericUpDown randMax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown bulletMax;
    }
}

