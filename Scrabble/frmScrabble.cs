using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;



namespace Scrabble
{
    public partial class frmScrabble : Form
    {
        private const int BOARD_SIZE = 15 * 15;
        private const int MAX_CONSECUTIVE_PASSES = 5;

        private int MAX_LETTERS_CAN_HOLD;
        private int BINGO_BONUS;

        private List<Button> wordPutOnBoard;
        private List<Button> lettersPutOnBoard;

        private List<String> dictionary;
        private List<String> firstWordDictionary;   // Added in ver. 1.8

        private List<int> freeSquares;              // Added in ver. 1.8

        private int freeSquaresIndex;               // Added in ver. 1.8


        private int numOfLettersRemaining = 99;
        private int numOfConsecutivePasses = 0;

        private int score;

        private bool isFirstMove = true;
        private bool isSpeakLegalWords = true;  // Added in ver. 1.8
        private bool isCircleBonuses = true;  // Added in ver. 1.8
        private bool isSortLettersAscending = true;  // Added in ver. 1.8


        private List<List<Button>> historyLettersPutOnBoard = new List<List<Button>>();

        private int historyBackCounter = 0;
        string buttonsLettersWord;



        public frmScrabble()
        {
            frmSplash splashForm = new frmSplash();
            splashForm.ShowDialog();

            frmOptions optionsForm = new frmOptions();
            optionsForm.ShowDialog();


            MAX_LETTERS_CAN_HOLD = optionsForm.GetMaxLettersCanHold();
            BINGO_BONUS = MAX_LETTERS_CAN_HOLD * 5;

            InitializeComponent();

            InitializeDictionaryAndLegend();
            InitializeButtonLetters();
            InitializeBoardSquares();
            InitializeLettersSurfaces();

            InitializeLettersValuesGridView();  // Added in ver. 1.8

        }


        private void InitializeDictionaryAndLegend()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("Scrabble.Resources.english.txt");

            TextReader dic = new StreamReader(stream);

            picLegend.Image = Image.FromStream(assembly.GetManifestResourceStream("Scrabble.Resources.Legend.bmp"));


            string word;

            dictionary = new List<String>();
            firstWordDictionary = new List<String>();


            /* Creates the dictionary , so we can use binary search .
               firstWordDictionary is used for First Word Hint */

            while ((word = dic.ReadLine()) != null)
            {
                dictionary.Add(word.ToUpper());  // Uppercase letters


                if (word.Length <= MAX_LETTERS_CAN_HOLD)
                {
                    firstWordDictionary.Add(word.ToUpper());
                }

            }

        }



        private void frmScrabble_Load(object sender, EventArgs e)
        {
            wordPutOnBoard = new List<Button>();
            lettersPutOnBoard = new List<Button>();
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            SkipToLastMove();
            

            if (isFirstMove)
            {
                FirstMove firstMove = new FirstMove(wordPutOnBoard, lblBoardSquares[BOARD_SIZE / 2], squaresBonuses, dictionary, isSpeakLegalWords);

                if (!firstMove.CheckStillFirstMove())
                {
                    score = firstMove.GetScore();
                    CheckBingoAndProceedNextMove();

                    isFirstMove = false;


                    // The following line was added in ver. 1.8

                    historyBackToolStripButton.Enabled = true;

                }

            }

            else
            {
                NonFirstMove nonFirstMove = new NonFirstMove(lettersPutOnBoard, wordPutOnBoard, lblBoardSquares, squaresBonuses, dictionary, score, isSpeakLegalWords);

                if (nonFirstMove.CheckMoveOK())
                {
                    score = nonFirstMove.GetScore();

                    CheckBingoAndProceedNextMove();
                }

            }

        }



        private void CheckBingoAndProceedNextMove()
        {
            if (wordPutOnBoard.Count == MAX_LETTERS_CAN_HOLD)
            {
                score += BINGO_BONUS;

                ToolTip toolTipBingoBonus = new ToolTip();
                toolTipBingoBonus.BackColor = Color.Orange;

                toolTipBingoBonus.ToolTipIcon = ToolTipIcon.Info;
                toolTipBingoBonus.Show("Bingo !\n" + BINGO_BONUS + " points bonus", lblBoardSquares[0], 563, 380, 3000);

            }


            lblScore.Text = "Score :   " + score;

            numOfConsecutivePasses = 0;
            btnFirstWordHint.Visible = false;   // can't use hint anymore
            this.Controls.Remove(lstBestWords);

            DisableButtonsOfWordPut();


            if (numOfLettersRemaining > 0)
            {
                AddLettersToSurface();
            }

            else
            {
                if (GetEmptyLabelsLocations().Count == MAX_LETTERS_CAN_HOLD)
                {
                    MessageBox.Show("Game Over\nFinal score :   " + score, Application.ProductName);
                    this.Close();
                }
            }


            numOfLettersRemaining -= wordPutOnBoard.Count;

            if (numOfLettersRemaining < 0)
            {
                numOfLettersRemaining = 0;
            }

            lblLettersRemaining.Text = "Letters remaining :  " + numOfLettersRemaining;


            foreach (Button button in wordPutOnBoard)
            {
                lettersPutOnBoard.Add(button);
            }


            List<Button> lettersPutOnBoardHistory = new List<Button>(lettersPutOnBoard);

            historyLettersPutOnBoard.Add(lettersPutOnBoardHistory);


            wordPutOnBoard.Clear();    // to start with the next word on board

        }




        /* For all the buttons of the word put on board , we detach
           the MouseDown event , using the line
           button.MouseDown -= new MouseEventHandler(this.btnLetters_MouseDown);
           so these buttons aren't draggable anymore */

        private void DisableButtonsOfWordPut()
        {
            foreach (Button button in wordPutOnBoard)
            {
                button.MouseDown -= new MouseEventHandler(this.btnLetters_MouseDown);
            }
        } 


        private void AddLettersToSurface()
        {
            List<Point> emptyLabelsLocations = GetEmptyLabelsLocations();

            AddNewLettersToSurface(emptyLabelsLocations);
        }



        /* Returns a List of empty labels locations : 
           these empty locations will be filled later  with the
           new buttons letters on letters surface */

        private List<Point> GetEmptyLabelsLocations()
        {
            List<Point> emptyLabelsLocations = new List<Point>();


            foreach (Label label in lblLettersSurfaces)
            {
                bool isEmptyLabel = true;


                foreach (Button button in btnLetters)
                {
                    if (button.Location == label.Location)
                    {
                        isEmptyLabel = false;   // label has button on it
                        break;
                    }
                }

                if (isEmptyLabel)
                {
                    emptyLabelsLocations.Add(label.Location);
                }

            }

            return emptyLabelsLocations;

        }



        /* Fills the empty labels locations with the new
           random-generated letters , generated in this method .
           The new letters locations are taken from the
           already-filled  emptyLabelsLocations List */

        private void AddNewLettersToSurface(List<Point> emptyLabelsLocations)
        {
            Letter letter = new Letter();


            for (int i = 0; i < Math.Min(wordPutOnBoard.Count, numOfLettersRemaining); i++)
            {
                btnLetters.Remove(wordPutOnBoard[i]);


                Button btnNewLetter = new Button();

                btnNewLetter.Font = new Font("Cooper Black", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(177)));
                btnNewLetter.Location = emptyLabelsLocations[i];
                btnNewLetter.Size = new Size(30, 30);
                btnNewLetter.TabIndex = wordPutOnBoard[i].TabIndex;


                char randomLetter = (char)randomLetters.Next('A', 'Z' + 2);

                if (randomLetter == 'Z' + 1)
                {
                    randomLetter = ' ';
                }

                
                btnNewLetter.Text = randomLetter.ToString();
                btnNewLetter.BackColor = letter.GetColor(randomLetter);
                btnNewLetter.MouseDown += new MouseEventHandler(btnLetters_MouseDown);

                btnLetters.Add(btnNewLetter);
                this.Controls.Add(btnNewLetter);


                // Displays the new letter on letters surface

                btnNewLetter.BringToFront();

            }

        }



        private void btnPass_Click(object sender, EventArgs e)
        {
            if (numOfLettersRemaining == 0)     // can't pass anymore
            {
                numOfConsecutivePasses++;

                if (numOfConsecutivePasses == MAX_CONSECUTIVE_PASSES)
                {
                    MessageBox.Show("Game Over\nFinal score :   " + score, Application.ProductName);
                    this.Close();
                }

                return;
            }



            /* If there are no empty labels on the letters surface ,
               then sort btnLetters by their X-coordinates :
               this sort makes the letters on frmPass to appear in
               the same order as they appear on the letters surface */

            if (GetEmptyLabelsLocations().Count == 0)
            {
                btnLetters.Sort(delegate(Button b1, Button b2) { return b1.Location.X.CompareTo(b2.Location.X); });
            }



            /* Passes the buttons letters to frmPass constructor,
               because the same letters must appear on frmPass */
            
            frmPass passForm = new frmPass(btnLetters, MAX_LETTERS_CAN_HOLD);
            passForm.ShowDialog();

            List<int> indicesLettersToReplace = passForm.GetIndicesLettersToReplace();

            

            /* If at least one letter has been replaced , and we
               passed MAX_CONSECUTIVE_PASSES times , then the game
               is over */

            if (indicesLettersToReplace.Count > 0)
            {
                numOfConsecutivePasses++;

                if (numOfConsecutivePasses == MAX_CONSECUTIVE_PASSES)
                {
                    MessageBox.Show("Game Over\nFinal score :   " + score, Application.ProductName);
                    this.Close();
                }
            }


            Letter letter = new Letter(); 



            /* The following foreach statement actually replaces
               the letters of the buttons we selected on frmPass */ 

            foreach (int indice in indicesLettersToReplace)
            {
                char randomLetter = (char)randomLetters.Next('A', 'Z' + 2);

                if (randomLetter == 'Z' + 1)
                {
                    randomLetter = ' ';
                }

                this.btnLetters[indice].Text = randomLetter.ToString();
                this.btnLetters[indice].BackColor = letter.GetColor(randomLetter);
            }


            if (!isFirstMove) return;   


            // Following code is executed only if we're in the first move

            btnFirstWordHint.Visible = true;
            this.Controls.Remove(lstBestWords);

        }



        private void btnFirstWordHint_Click(object sender, EventArgs e)
        {
            List<String> hintDictionary = new List<String>();
            buttonsLettersWord = "";

            int numOfWildCards = 0;
           

            foreach (Button button in btnLetters)
            {
                if (button.Text == " ")
                {
                    numOfWildCards++;
                }


                if ((button.BackColor == Color.White) && (button.Text != " "))
                {
                    buttonsLettersWord += button.Text + "*";
                }

                else
                {
                    buttonsLettersWord += button.Text + "!";
                }

            }


            foreach (string word in firstWordDictionary)
            {
                string buttonsWord = buttonsLettersWord;
                int numOfLettersFound = 0;


                foreach (char ch in word)
                {
                    if (buttonsWord.Contains(ch.ToString()))
                    {
                        buttonsWord = buttonsWord.Remove(buttonsWord.IndexOf(ch), 1);
                        numOfLettersFound++;
                    }
                }

                if (numOfLettersFound + numOfWildCards >= word.Length)
                {
                    hintDictionary.Add(word);
                }

            }


            lstBestWords = new ListBox();

            lstBestWords.Location = new Point(600, 320);
            lstBestWords.DoubleClick += new EventHandler(this.lstBestWords_DoubleClick);

            
            new FirstWordHint(hintDictionary, buttonsLettersWord, lstBestWords, BINGO_BONUS);
            
            btnFirstWordHint.Visible = false;

        }



        private void lstBestWords_DoubleClick(object sender, EventArgs e)
        {
            string bestWordSelected;


            // Treats the case of selecting a blank line in list box

            if (lstBestWords.SelectedItem == null)
            {
                return;
            }


            bestWordSelected = lstBestWords.SelectedItem.ToString();            



            /* If no word can be created from given letters ,
               then perform Pass button click */

            if (!bestWordSelected.Contains("\t"))
            {
                btnPass.PerformClick();
                return;
            }


            /* Code until wordPutOnBoard.Clear() , inclusive ,
               treats the case when there are letters on the board 
               and the user double-clicked the list box to use hint */

            int i = 0;

            List<Point> emptyLabelsLocations = GetEmptyLabelsLocations();


            foreach (Button button in wordPutOnBoard)
            {
                button.Location = emptyLabelsLocations[i];
                i++;
            }


            wordPutOnBoard.Clear();

            
            bestWordSelected = bestWordSelected.Split('\t')[1];


            if (bestWordSelected.Length < 9)
            {
                i = 112;
            }

            else if (bestWordSelected.Length < 13)
            {
                i = 108;
            }

            else
            {
                i = 105;
            }



            string buttonsWord = buttonsLettersWord;
            string lettersDoNotAppearOnButtons = "";


            foreach (char ch in bestWordSelected)
            {
                if (!buttonsWord.Contains(ch.ToString()))
                {
                    lettersDoNotAppearOnButtons += ch;
                }

                else
                {
                    buttonsWord = buttonsWord.Remove(buttonsWord.IndexOf(ch), 1);
                }

            }


            int j = 0;
            
            foreach (Button button in btnLetters)
            {
                /* Treats the case  there is at least 1 empty
                   wild card on btnLetters  and all the letters in
                   the best word selected are contained in the
                   letters of btnLetters */

                if (lettersDoNotAppearOnButtons == "")
                {
                    break;
                }


                if (button.Text == " ")
                {
                    button.Text = lettersDoNotAppearOnButtons[j].ToString();
                    button.Refresh();

                    j++;
                }
            }


            foreach (char ch in bestWordSelected)
            {
                foreach (Button button in btnLetters)
                {
                    if ((button.Text[0] == ch) && (!wordPutOnBoard.Contains(button)))
                    {
                        btnLetterToDrag = button;

                        lblBoardSquares_DragDrop(lblBoardSquares[i], null);

                        i++;

                        break;

                    }

                }

            }

            
            btnSubmit.PerformClick();   // Actually uses the hint

        }



        private void speakLegalWordsToolStripButton_Click(object sender, EventArgs e)
        {
            const string speakMessage = "Speak Legal Words : ";

            Assembly assembly = Assembly.GetExecutingAssembly();


            isSpeakLegalWords = !isSpeakLegalWords;


            if (isSpeakLegalWords)
            {
                speakLegalWordsToolStripButton.ToolTipText = speakMessage + "Yes";
                speakLegalWordsToolStripButton.Image = Image.FromStream(assembly.GetManifestResourceStream("Scrabble.Resources.Speak.bmp"));
            }

            else
            {
                speakLegalWordsToolStripButton.ToolTipText = speakMessage + "No";
                speakLegalWordsToolStripButton.Image = Image.FromStream(assembly.GetManifestResourceStream("Scrabble.Resources.SpeakOff.bmp"));
            }

        }



        private void backgroundColorToolStripButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();


            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorDialog.Color;
                backgroundColorToolStripButton.ForeColor = colorDialog.Color;
            }

        }



        private void circleBonusesToolStripButton_Click(object sender, EventArgs e)
        {
            const string circleMessage = "Bonuses Shape : ";
            string bonus = "";

            isCircleBonuses = !isCircleBonuses;


            if (isCircleBonuses)
            {
                circleBonusesToolStripButton.ToolTipText = circleMessage + "Circle";
            }

            else
            {
                circleBonusesToolStripButton.ToolTipText = circleMessage + "Rectangle";
            }



            foreach (Label label in lblBoardSquares)
            {
                if (squaresBonuses.ContainsKey(label.Location))
                {
                    squaresBonuses.TryGetValue(label.Location, out bonus);


                    switch (bonus)
                    {
                        case "2w":

                            if (isCircleBonuses)
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, 30, 30));
                                label.CreateGraphics().FillEllipse(new SolidBrush(Color.CornflowerBlue), new Rectangle(6, 6, 15, 15));
                            }

                            else
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(Color.CornflowerBlue), new Rectangle(0, 0, 30, 30));
                            }

                            label.CreateGraphics().DrawString("2w", this.Font, new SolidBrush(Color.White), 5, 7);

                            break;


                        case "3w":

                            if (isCircleBonuses)
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, 30, 30));
                                label.CreateGraphics().FillEllipse(new SolidBrush(Color.Blue), new Rectangle(6, 6, 15, 15));
                            }

                            else
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(Color.Blue), new Rectangle(0, 0, 30, 30));
                            }

                            label.CreateGraphics().DrawString("3w", this.Font, new SolidBrush(Color.White), 5, 7);

                            break;


                        case "2L":

                            if (isCircleBonuses)
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, 30, 30));
                                label.CreateGraphics().FillEllipse(new SolidBrush(Color.Gray), new Rectangle(6, 6, 15, 15));
                            }

                            else
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0, 0, 30, 30));
                            }

                            label.CreateGraphics().DrawString("2L", this.Font, new SolidBrush(Color.White), 5, 7);

                            break;


                        case "3L":

                            if (isCircleBonuses)
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, 30, 30));
                                label.CreateGraphics().FillEllipse(new SolidBrush(Color.Black), new Rectangle(6, 6, 15, 15));
                            }

                            else
                            {
                                label.CreateGraphics().FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 30, 30));
                            }

                            label.CreateGraphics().DrawString("3L", this.Font, new SolidBrush(Color.White), 5, 7);

                            break;

                    }

                }
                
            }

        }



        private void sortLettersToolStripButton_Click(object sender, EventArgs e)
        {
            const string sortOrderMessage = "Sort Letters By Order : ";


            if (isSortLettersAscending)
            {
                btnLetters.Sort(delegate(Button b1, Button b2) { return b1.Text.CompareTo(b2.Text); });

                sortLettersToolStripButton.ToolTipText = sortOrderMessage + "Descending";
                sortLettersToolStripButton.Text = "za";
            }

            else
            {
                btnLetters.Sort(delegate(Button b1, Button b2) { return b2.Text.CompareTo(b1.Text); });

                sortLettersToolStripButton.ToolTipText = sortOrderMessage + "Ascending";
                sortLettersToolStripButton.Text = "az";
            }


            /* The following for statement actually displays the
               letters in sorted order (ascending or descending) */

            for (int i = 0; i < btnLetters.Count; i++)
            {
                btnLetters[i].Location = lblLettersSurfaces[i].Location;
            }


            /* As we did in lstBestWords_DoubleClick , because we
               sort btnLetters (including the buttons of wordPutOnBoard) */
            
            wordPutOnBoard.Clear();     


            isSortLettersAscending = !isSortLettersAscending;

        }



        private void shuffleLettersToolStripButton_Click(object sender, EventArgs e)
        {
            List<int> randomIndices = new List<int>();
            Random randIndices = new Random();

            int randomIndice;



            for (int i = 0; i < btnLetters.Count; i++)
            {
                do
                {
                    randomIndice = randIndices.Next(btnLetters.Count);
                }

                while (randomIndices.Contains(randomIndice));

                randomIndices.Add(randomIndice);

            }


            for (int i = 0; i < btnLetters.Count; i++)
            {
                btnLetters[randomIndices[i]].Location = lblLettersSurfaces[i].Location;
            }


            /* As we did in lstBestWords_DoubleClick , because we
               shuffle btnLetters (including the buttons of wordPutOnBoard) */

            wordPutOnBoard.Clear();

        }



        // View previous move in moves history

        private void historyBackToolStripButton_Click(object sender, EventArgs e)
        {
            historyNextToolStripButton.Enabled = true;


            foreach (Button button in lettersPutOnBoard)
            {
                this.Controls.Remove(button);
            }


            historyBackCounter++;


            if (historyBackCounter < historyLettersPutOnBoard.Count)
            {
                foreach (Button button in historyLettersPutOnBoard[historyLettersPutOnBoard.Count - 1 - historyBackCounter])
                {
                    this.Controls.Add(button);
                    button.BringToFront();
                }
            }

            else
            {
                historyBackToolStripButton.Enabled = false;
            }

        }



        // view next move in moves history

        private void historyNextToolStripButton_Click(object sender, EventArgs e)
        {
            historyBackToolStripButton.Enabled = true;

            historyBackCounter--;


            if (historyBackCounter >= 0)
            {
                foreach (Button button in historyLettersPutOnBoard[historyLettersPutOnBoard.Count - 1 - historyBackCounter])
                {
                    this.Controls.Add(button);
                    button.BringToFront();
                }


                if (historyBackCounter == 0)
                {
                    historyNextToolStripButton.Enabled = false;
                }

            }

        }



        private void lettersValuesToolStripButton_Click(object sender, EventArgs e)
        {
            dgvLettersValues.Visible = !dgvLettersValues.Visible;


            // First row of grid  will not be selected when grid is shown

            dgvLettersValues.Rows[0].Selected = false;
        }



        /* Skips to last move ,  used when trying to drag letters
           or submit a word  while viewing history of moves */

        private void SkipToLastMove()
        {
            while (historyNextToolStripButton.Enabled)
            {
                historyNextToolStripButton.PerformClick();
            }
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnSubmit.PerformClick();
                return true;
            }


            if (keyData == Keys.Right)
            {
                // Right arrow image

                lblLabelToPut.Image = Image.FromFile("RightArrow.bmp");

                freeSquaresIndex = 0;

                freeSquares = GetFreeSquaresTillEndOfRow(lblLabelToPut.TabIndex - 1);
                return true;
            }


            if (keyData == Keys.Down)
            {
                // Down arrow image

                lblLabelToPut.Image = Image.FromFile("DownArrow.bmp");

                freeSquaresIndex = 0;

                freeSquares = GetFreeSquaresTillEndOfColumn(lblLabelToPut.TabIndex - 15);
                return true;
            }



            /* The user didn't press nor the right arrow neither
               the down arrow , so don't let play using keyboard */

            if (lblLabelToPut.Image == null)
            {
                return false;
            }



            /* The key pressed is not a character between A and Z ,
               so don't accept it */

            if ((keyData < Keys.A) || (keyData > Keys.Z))
            {
                return false; 
            }



            /* The user typed more letters than the size of 
               freeSquares ;  it means that the user exceeds
               the bottommost corner , or the rightmost cornet ,
               when using the keyboard ;  don't allow that */

            if (freeSquaresIndex >= freeSquares.Count)
            {
                return false;
            }



            foreach (Button button in btnLetters)
            {
                if (button.Text == keyData.ToString().ToUpper())
                {
                    if (!wordPutOnBoard.Contains(button))
                    {
                        btnLetterToDrag = button;


                        int nextFreeSquare = freeSquares[freeSquaresIndex];

                        lblBoardSquares_DragDrop(lblBoardSquares[nextFreeSquare], null);
                        freeSquaresIndex++;

                        break;
                    }

                }

                else
                {
                    if (button.Text == " ")
                    {
                        button.Text = keyData.ToString().ToUpper();

                        btnLetterToDrag = button;


                        int nextFreeSquare = freeSquares[freeSquaresIndex];

                        lblBoardSquares_DragDrop(lblBoardSquares[nextFreeSquare], null);
                        freeSquaresIndex++;

                        break;
                    }

                }

            }


            return true;            

        }



        private List<int> GetFreeSquaresTillEndOfRow(int i)
        {
            List<int> freeSquaresTillEndOfRow = new List<int>();


            List<Button> buttonsPutOnBoard = new List<Button>(wordPutOnBoard);

            buttonsPutOnBoard.AddRange(lettersPutOnBoard);



            do
            {
                i++;

                bool isBlankSquare = true;


                foreach (Button button in buttonsPutOnBoard)
                {
                    if (lblBoardSquares[i].Location == button.Location)
                    {
                        isBlankSquare = false;

                        break;
                    }

                }


                if (isBlankSquare)
                {
                    freeSquaresTillEndOfRow.Add(i);
                }

            }

            while (i % 15 != 14);


            return freeSquaresTillEndOfRow;

        }



        private List<int> GetFreeSquaresTillEndOfColumn(int i)
        {
            List<int> freeSquaresTillEndOfColumn = new List<int>();


            List<Button> buttonsPutOnBoard = new List<Button>(wordPutOnBoard);

            buttonsPutOnBoard.AddRange(lettersPutOnBoard);



            do
            {
                i += 15;

                bool isBlankSquare = true;


                foreach (Button button in buttonsPutOnBoard)
                {
                    if (lblBoardSquares[i].Location == button.Location)
                    {
                        isBlankSquare = false;

                        break;
                    }

                }

                if (isBlankSquare)
                {
                    freeSquaresTillEndOfColumn.Add(i);
                }

            }

            while (i / 15 != 14);


            return freeSquaresTillEndOfColumn;

        }

    }

}