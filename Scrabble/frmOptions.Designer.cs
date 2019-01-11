using System;
using System.Drawing;
using System.Windows.Forms;


namespace Scrabble
{
    partial class frmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.lblMaxLettersCanHold = new System.Windows.Forms.Label();
            this.udcMaxLettersCanHold = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbxMaxLetters = new System.Windows.Forms.GroupBox();
            this.lblEnterNewValue = new System.Windows.Forms.Label();
            this.udcLetterNewValue = new System.Windows.Forms.NumericUpDown();
            this.btnLetterRandomValue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udcMaxLettersCanHold)).BeginInit();
            this.gbxMaxLetters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udcLetterNewValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaxLettersCanHold
            // 
            this.lblMaxLettersCanHold.AutoSize = true;
            this.lblMaxLettersCanHold.Location = new System.Drawing.Point(18, 64);
            this.lblMaxLettersCanHold.Name = "lblMaxLettersCanHold";
            this.lblMaxLettersCanHold.Size = new System.Drawing.Size(154, 13);
            this.lblMaxLettersCanHold.TabIndex = 0;
            this.lblMaxLettersCanHold.Text = "Max letters can hold at a time : ";
            // 
            // udcMaxLettersCanHold
            // 
            this.udcMaxLettersCanHold.Location = new System.Drawing.Point(178, 62);
            this.udcMaxLettersCanHold.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.udcMaxLettersCanHold.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udcMaxLettersCanHold.Name = "udcMaxLettersCanHold";
            this.udcMaxLettersCanHold.Size = new System.Drawing.Size(36, 20);
            this.udcMaxLettersCanHold.TabIndex = 1;
            this.udcMaxLettersCanHold.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(85, 174);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 39);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbxMaxLetters
            // 
            this.gbxMaxLetters.Controls.Add(this.udcMaxLettersCanHold);
            this.gbxMaxLetters.Controls.Add(this.lblMaxLettersCanHold);
            this.gbxMaxLetters.Location = new System.Drawing.Point(10, 3);
            this.gbxMaxLetters.Name = "gbxMaxLetters";
            this.gbxMaxLetters.Size = new System.Drawing.Size(240, 160);
            this.gbxMaxLetters.TabIndex = 3;
            this.gbxMaxLetters.TabStop = false;
            // 
            // lblEnterNewValue
            // 
            this.lblEnterNewValue.AutoSize = true;
            this.lblEnterNewValue.Location = new System.Drawing.Point(302, 187);
            this.lblEnterNewValue.Name = "lblEnterNewValue";
            this.lblEnterNewValue.Size = new System.Drawing.Size(0, 13);
            this.lblEnterNewValue.TabIndex = 5;
            // 
            // udcLetterNewValue
            // 
            this.udcLetterNewValue.Location = new System.Drawing.Point(323, 185);
            this.udcLetterNewValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udcLetterNewValue.Name = "udcLetterNewValue";
            this.udcLetterNewValue.Size = new System.Drawing.Size(30, 20);
            this.udcLetterNewValue.TabIndex = 6;
            this.udcLetterNewValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udcLetterNewValue.Visible = false;
            this.udcLetterNewValue.ValueChanged += new System.EventHandler(this.udcLetterNewValue_ValueChanged);
            this.udcLetterNewValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.udcLetterNewValue_KeyPress);
            // 
            // btnLetterRandomValue
            // 
            this.btnLetterRandomValue.Location = new System.Drawing.Point(360, 130);
            this.btnLetterRandomValue.Name = "btnLetterRandomValue";
            this.btnLetterRandomValue.Size = new System.Drawing.Size(120, 30);
            this.btnLetterRandomValue.TabIndex = 7;
            this.btnLetterRandomValue.UseVisualStyleBackColor = true;
            this.btnLetterRandomValue.Visible = false;
            this.btnLetterRandomValue.Click += new System.EventHandler(this.btnLetterRandomValue_Click);
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 221);
            this.ControlBox = false;
            this.Controls.Add(this.btnLetterRandomValue);
            this.Controls.Add(this.udcLetterNewValue);
            this.Controls.Add(this.lblEnterNewValue);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbxMaxLetters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowInTaskbar = false;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.udcMaxLettersCanHold)).EndInit();
            this.gbxMaxLetters.ResumeLayout(false);
            this.gbxMaxLetters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udcLetterNewValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private void InitializeButtonLetters()
        {
            btnLetters = new Button[26];

            int x = 300;
            int y = 10;

            char letter = 'A';


            for (int i = 0; i < btnLetters.Length; i++)
            {
                btnLetters[i] = new Button();
                btnLetters[i].Text = letter.ToString();
                btnLetters[i].Location = new Point(x, y);
                btnLetters[i].Size = new Size(30, 30);
                btnLetters[i].Click += new EventHandler(btnLetters_Click);

                this.Controls.Add(btnLetters[i]);

                btnLetters[i].BringToFront();


                x += 30;

                if (i % 6 == 5)
                {
                    x = 300;
                    y += 30;
                }

                letter++;
            }


            btnLetters[0].Tag = 1;
            btnLetters[1].Tag = 2;
            btnLetters[2].Tag = 1;
            btnLetters[3].Tag = 1;
            btnLetters[4].Tag = 1;
            btnLetters[5].Tag = 3;
            btnLetters[6].Tag = 1;
            btnLetters[7].Tag = 2;
            btnLetters[8].Tag = 1;
            btnLetters[9].Tag = 5;
            btnLetters[10].Tag = 3;
            btnLetters[11].Tag = 1;
            btnLetters[12].Tag = 1;
            btnLetters[13].Tag = 1;
            btnLetters[14].Tag = 1;
            btnLetters[15].Tag = 2;
            btnLetters[16].Tag = 5;
            btnLetters[17].Tag = 1;
            btnLetters[18].Tag = 1;
            btnLetters[19].Tag = 1;
            btnLetters[20].Tag = 1;
            btnLetters[21].Tag = 4;
            btnLetters[22].Tag = 4;
            btnLetters[23].Tag = 5;
            btnLetters[24].Tag = 3;
            btnLetters[25].Tag = 5;


            for (int i = 0; i < btnLetters.Length; i++)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(btnLetters[i], btnLetters[i].Tag.ToString());
            }

        }


        public static Button[] GetButtonsLetters()
        {
            return btnLetters;
        }


        private void btnLetters_Click(object sender, EventArgs e)
        {
            btnLetter = (Button)sender;

            btnLetterRandomValue.Visible = true;
            lblEnterNewValue.Visible = true;
            udcLetterNewValue.Visible = true;

            btnLetterRandomValue.Text = "Random value for " + btnLetter.Text;
            lblEnterNewValue.Text = btnLetter.Text + "=";

            udcLetterNewValue.Value = Decimal.Parse(btnLetter.Tag.ToString());

        }


        private System.Windows.Forms.Label lblMaxLettersCanHold;
        private System.Windows.Forms.NumericUpDown udcMaxLettersCanHold;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbxMaxLetters;
        private System.Windows.Forms.Label lblEnterNewValue;
        private static Button[] btnLetters;
        private Button btnLetter;
        private NumericUpDown udcLetterNewValue;
        private Button btnLetterRandomValue;

    }

}


