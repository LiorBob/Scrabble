using System;
using System.Windows.Forms;


namespace Scrabble
{
    public partial class frmOptions : Form
    {
        private const int MAX_LETTER_VALUE = 9999;


        public frmOptions()
        {
            InitializeComponent();
            InitializeButtonLetters();

            udcLetterNewValue.Maximum = MAX_LETTER_VALUE;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public int GetMaxLettersCanHold()
        {
            return (int)udcMaxLettersCanHold.Value;
        }


        private void udcLetterNewValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isLastCharacterDigit = false;

            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
            {
                isLastCharacterDigit = true;
            }


            if (udcLetterNewValue.Value.ToString().Length == MAX_LETTER_VALUE.ToString().Length) 
            {
                if (isLastCharacterDigit)
                {
                    e.Handled = true;
                    udcLetterNewValue.Value = udcLetterNewValue.Minimum;
                }
            }


            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLetter.Tag = (int)udcLetterNewValue.Value;

                UpdateLetterToolTip();
            }


            ResizeUpDownControl();
            
        }


        private void udcLetterNewValue_ValueChanged(object sender, EventArgs e)
        {
            ResizeUpDownControl();
        }


        private void ResizeUpDownControl()
        {
            if (udcLetterNewValue.Value.ToString().Length <= MAX_LETTER_VALUE.ToString().Length)
            {
                udcLetterNewValue.Width = 26 + udcLetterNewValue.Value.ToString().Length * 8;
            }

        }


        private void btnLetterRandomValue_Click(object sender, EventArgs e)
        {
            btnLetter.Tag = new Random().Next((int)udcLetterNewValue.Minimum, MAX_LETTER_VALUE + 1);

            UpdateLetterToolTip();
        }


        void UpdateLetterToolTip()
        {
            btnLetterRandomValue.Visible = false;
            lblEnterNewValue.Visible = false;
            udcLetterNewValue.Visible = false;


            // shows the new value of the letter , as its tooltip text

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnLetter, btnLetter.Tag.ToString());

            btnLetter.Focus();
        }

    }

}