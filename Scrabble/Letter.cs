using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Scrabble
{
    class Letter
    {
        private Color ORANGE = Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(197)))), ((int)(((byte)(24)))));
        private Color BLUE = Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(150)))), ((int)(((byte)(240)))));
        private Color GREEN = Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(196)))), ((int)(((byte)(67)))));
        private Color LIGHTGREEN = Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(230)))), ((int)(((byte)(95)))));
        private Color PURPLE = Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(111)))), ((int)(((byte)(243)))));


        private char name;
        private int points;
        private Color color;
        private List<Letter> abcLetters;


        public Letter()
        {
            abcLetters = new List<Letter>();
            SetLettersAttributes();
        }


        public Letter(char name, int points, Color color)
        {
            this.name = name;
            this.points = points;
            this.color = color;
        }


        private void SetLettersAttributes()
        {
            Button[] btnLetters = frmOptions.GetButtonsLetters();


            foreach (Button button in btnLetters)
            {
                switch ((int) button.Tag)
                {
                    case 1:

                        abcLetters.Add(new Letter(button.Text[0], (int) button.Tag, ORANGE));
                        break;

                    case 2:

                        abcLetters.Add(new Letter(button.Text[0], (int)button.Tag, BLUE));
                        break;

                    case 3:

                        abcLetters.Add(new Letter(button.Text[0], (int)button.Tag, GREEN));
                        break;

                    case 4:

                        abcLetters.Add(new Letter(button.Text[0], (int)button.Tag, LIGHTGREEN));
                        break;

                    case 5:

                        abcLetters.Add(new Letter(button.Text[0], (int)button.Tag, PURPLE));
                        break;


                    default:

                        abcLetters.Add(new Letter(button.Text[0], (int)button.Tag, Color.OrangeRed));
                        break;

                }

            }

            abcLetters.Add(new Letter(' ', 0, Color.White));

        }


        public Color GetColor(char letter)
        {
            Color letterColor = new Color();

            foreach (Letter lett in abcLetters)
            {
                if (lett.name == letter)
                {
                    letterColor = lett.color;
                    break;
                }
            }

            return letterColor;
        }


        public int GetPoints(char letter)
        {
            int letterPoints = 0;

            foreach (Letter lett in abcLetters)
            {
                if (lett.name == letter)
                {
                    letterPoints = lett.points;
                    break;
                }
            }

            return letterPoints;
        }

    }

}
