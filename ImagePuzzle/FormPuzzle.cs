using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ImagePuzzle
{
    public partial class FormPuzzle : Form
    {
        int inNullSliceIndex;
        int inMoves = 0;
        List<Bitmap> listOriginalPictureList = new List<Bitmap>();
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();    
        public FormPuzzle()
        {
            InitializeComponent();
            listOriginalPictureList.AddRange(new Bitmap[] {Properties.Resources._1,
                Properties.Resources._2, Properties.Resources._3, Properties.Resources._4,
                Properties.Resources._5, Properties.Resources._6, Properties.Resources._7,
                Properties.Resources._8, Properties.Resources._9, Properties.Resources._null});
            labelMovesMade.Text += inMoves;
            labelTimeElapsed.Text = "00:00:00"; 
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            shuffle();
        }

        internal bool checkWin()
        {
            int i;
            for(i = 0; i < 8; i++)
            {
                if ((gbPuzzleBox.Controls[i] as PictureBox).Image != listOriginalPictureList[i]) break;
            }

            if (i == 8) return true;
            else return false;
        }

        internal void shuffle()
        {
            do
            {
                int j;
                // 8 tidak diikutkan karena index terakhir  
                List<int> Indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9 });
                Random r = new Random();
                for (int i = 0; i < 9; i++)
                {
                    Indexes.Remove((j = Indexes[r.Next(0, Indexes.Count)]));
                    ((PictureBox)gbPuzzleBox.Controls[i]).Image = listOriginalPictureList[j];
                    // untuk menyimpan kotak box picture yang kosong
                    if (j == 9) inNullSliceIndex = i;
                }
            } while (checkWin());
        }

        private void buttonShuffle_Click(object sender, EventArgs e)
        {
            DialogResult YesOrNo = new DialogResult();
            if (labelTimeElapsed.Text == "00:00:00")
            {
                YesOrNo = MessageBox.Show("Apakah Anda Ingin Merestart nya??", 
                    "Picture Puzzle-Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (YesOrNo == DialogResult.Yes || labelTimeElapsed.Text == "00:00:00")
            {
                shuffle();
                timer.Reset();
                labelTimeElapsed.Text = "00:00:00";
                inMoves = 0;
                labelMovesMade.Text = "0";
            } 
        }

        private void switchPictureBox(object sender, EventArgs e)
        {
            if (labelTimeElapsed.Text == "00:00:00") timer.Start();
            int inPictureBoxIndex = gbPuzzleBox.Controls.IndexOf(sender as Control);
            if (inNullSliceIndex != inPictureBoxIndex)
            {
                List<int> FourBrothers = new List<int>(new int[] { ((inPictureBoxIndex % 3 == 0) ? -1 : 
                    inPictureBoxIndex - 1), inPictureBoxIndex - 3, ((inPictureBoxIndex % 3 == 2) ? -1 :
                    inPictureBoxIndex + 1), inPictureBoxIndex + 3});
                if (FourBrothers.Contains(inNullSliceIndex))
                {
                    ((PictureBox)gbPuzzleBox.Controls[inNullSliceIndex]).Image =
                        ((PictureBox)gbPuzzleBox.Controls[inPictureBoxIndex]).Image;
                    ((PictureBox)gbPuzzleBox.Controls[inPictureBoxIndex]).Image =
                        listOriginalPictureList[9];
                    inNullSliceIndex = inPictureBoxIndex;
                    labelMovesMade.Text = "Moves Made : " + (++inMoves);
                    if (checkWin())
                    {
                        timer.Stop();
                        (gbPuzzleBox.Controls[8] as PictureBox).Image = listOriginalPictureList[8];
                        MessageBox.Show("Congratulation...\nYour Picture Puzzle Game Win\n" +
                            "Time Elapsed : " + timer.Elapsed.ToString().Remove(8) +
                            "\nTotal Moves Made : " + inMoves + " Picture Puzzle Game");
                        inMoves = 0;
                        labelMovesMade.Text = "Moves Made : 0";
                        labelTimeElapsed.Text = "00:00:00";
                        timer.Reset();
                        shuffle();
                    }
                }
            }
        }

        private void updateTimeElapsed(object sender, EventArgs e)
        {
            if (timer.Elapsed.ToString() != "00:00:00")
                labelTimeElapsed.Text = timer.Elapsed.ToString().Remove(8);
            
            if (timer.Elapsed.ToString() == "00:00:00")
                buttonPaused.Enabled = false;
            else
                buttonPaused.Enabled = true;

            if (timer.Elapsed.Minutes.ToString() == "1")
            {
                timer.Reset();
                inMoves = 0;
                labelMovesMade.Text = "Moves Made : 0";
                labelTimeElapsed.Text = "00:00:00";
                buttonPaused.Enabled = false;
                MessageBox.Show("Waktu Habis\nCoba Kembali", "Picture Puzzle-Image");
                shuffle();
            }
        }

        private void buttonPaused_Click(object sender, EventArgs e)
        {
            if (buttonPaused.Text == "Paused")
            {
                timer.Stop();
                gbPuzzleBox.Visible = false;
                buttonPaused.Text = "Resume";
            } else
            {
                timer.Start();
                gbPuzzleBox.Visible = true;
                buttonPaused.Text = "Paused";
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            var YesOrNo = new DialogResult();
            YesOrNo = MessageBox.Show("Apakah Anda Ingin Keluar??",
                    "Picture Puzzle-Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (YesOrNo == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}
