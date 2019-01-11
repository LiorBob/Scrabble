using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


/* The following form is being shown when clicking "Pass" from
   the main game form */

namespace Scrabble
{
    public partial class frmPass : Form
    {
        // Buttons with same letters as in the main game form
        
        private List<Button> btnLetters;    
        private List<int> indicesLettersToReplace;
        private ToolTip toolTip;


        public frmPass(List<Button> btnLettersToReplace, int maxLettersToReplace)
        {
            InitializeComponent();

            toolTip = new ToolTip();
            toolTip.SetToolTip(this.chbSelectAll, "Select All");


            if (maxLettersToReplace > 7)
            {
                this.ClientSize = new System.Drawing.Size(268 + 30 * (maxLettersToReplace - 7), 100);
                this.btnPass.Location = new System.Drawing.Point(this.ClientSize.Width / 3, 57);
                this.btnCancel.Location = new System.Drawing.Point(this.ClientSize.Width / 3 + 83, 57);
                this.chbSelectAll.Location = new System.Drawing.Point(this.ClientSize.Width / 3 + 165, 67);
            }

            InitializeButtonLetters(btnLettersToReplace, maxLettersToReplace);
        }


        private void InitializeButtonLetters(List<Button> btnLettersToReplace, int maxLettersToReplace)
        {
            this.btnLetters = new List<Button>();
            this.indicesLettersToReplace = new List<int>();

            Letter letter = new Letter();

            int x = 30;


            
            /* Places the buttons on the form , with the same
               letters as in the main game form */

            for (int i = 0; i < maxLettersToReplace; i++)
            {
                Button button = new Button();

                button.Font = new Font("Cooper Black", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(177)));
                button.Location = new Point(x, 10);
                button.Size = new Size(30, 30);
                button.TabIndex = i;

                button.Text = btnLettersToReplace[i].Text;
                button.BackColor = letter.GetColor(button.Text[0]);
                button.Click += new EventHandler(this.btnLetters_Click);
                btnLetters.Add(button);
                this.Controls.Add(button);

                x += 30;
            }

        }


        private void btnLetters_Click(object sender, EventArgs e)
        {
            Button btnLetterToReplace = (Button) sender;

            if (!indicesLettersToReplace.Contains(btnLetterToReplace.TabIndex))
            {
                btnLetterToReplace.Font = new Font("Cooper Black", 12F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, ((byte)(177)));


                /* For every letter we select to replace , we add
                   its button index to indicesLettersToReplace List */

                indicesLettersToReplace.Add(btnLetterToReplace.TabIndex);

            }

            else       // letter was already selected - we can deselect it
            {
                btnLetterToReplace.Font = new Font("Cooper Black", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(177)));

                indicesLettersToReplace.Remove(btnLetterToReplace.TabIndex);
            }

        }


        private void btnPass_Click(object sender, EventArgs e)
        {
            this.Close();           // Simply closes the form
        }



        /* When we cancel , we remove all indices from
           indicesLettersToReplace (no letters to replace)
           and then close the form */ 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            indicesLettersToReplace.Clear();
            this.Close();
        }


        public List<int> GetIndicesLettersToReplace()
        {
            return indicesLettersToReplace;
        }


        private void chbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSelectAll.Checked)
            {
                toolTip.SetToolTip(this.chbSelectAll, "Deselect All");
            }

            else
            {
                toolTip.SetToolTip(this.chbSelectAll, "Select All");
            }


            foreach (Button button in btnLetters)
            {
                button.PerformClick();
            }

        }

    }

}