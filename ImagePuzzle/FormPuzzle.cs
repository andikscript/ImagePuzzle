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
        Timer timer1 = new Timer();
        int counter = 60;
        ObjectDesign objectDesign = new ObjectDesign("01:00", "00:00", "Moves Made : 0", "Paused", "Resume");
        
        public FormPuzzle()
        {
            InitializeComponent();
            listOriginalPictureList.AddRange(new Bitmap[] {Properties.Resources._1,
                Properties.Resources._2, Properties.Resources._3, Properties.Resources._4,
                Properties.Resources._5, Properties.Resources._6, Properties.Resources._7,
                Properties.Resources._8, Properties.Resources._9, Properties.Resources._null});
            timer1.Tick += new EventHandler(timer1_tick);
            timer1.Interval = 1000;
            labelTime.Text = objectDesign.labelTimeStart;
            buttonPaused.Enabled = false;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e) { }


        private void Form1_Load(object sender, EventArgs e)
        {
            shuffle();
        }

        private void timer1_tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
                timer1.Stop();
            labelTime.Text = counter.ToString().Length == 1 ? "00:0" + counter.ToString() :
                "00:" + counter.ToString();
            if (labelTime.Text == objectDesign.labelTimeEnd)
            {
                inMoves = 0;
                counter = 60;
                labelMovesMade.Text = objectDesign.labelMovesMade;
                labelTime.Text = objectDesign.labelTimeStart;
                MessageBox.Show("Waktu Habis\nCoba Kembali", "Picture Puzzle-Image");
                shuffle();
            }
        }

        internal bool checkWin()
        {
            int i;
            for (i = 0; i < 8; i++)
            {
                if ((gbPuzzleBox.Controls[i] as PictureBox).Image != listOriginalPictureList[i]) break;
            }

            if (i == 8) return true;
            else return false;
        }

        internal void shuffle()
        {
            buttonPaused.Enabled = false;
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
            var YesOrNo = new DialogResult();
            YesOrNo = MessageBox.Show("Apakah Anda Ingin Merestart nya??",
                    "Picture Puzzle-Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (YesOrNo == DialogResult.Yes || labelTime.Text == objectDesign.labelTimeEnd)
            {
                shuffle();
                timer1.Stop();
                labelTime.Text = objectDesign.labelTimeStart;
                counter = 60;
                labelMovesMade.Text = objectDesign.labelMovesMade;
                inMoves = 0;
            }
        }

        private void switchPictureBox(object sender, EventArgs e)
        {
            if (labelTime.Text == objectDesign.labelTimeStart)
            {
                timer1.Start();
                buttonPaused.Enabled = true;
            };
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
                        timer1.Stop();
                        (gbPuzzleBox.Controls[8] as PictureBox).Image = listOriginalPictureList[8];
                        MessageBox.Show("Congratulation...\nYour Picture Puzzle Game Win\n" +
                            "Time Elapsed : " + "00:" + counter.ToString() +
                            "\nTotal Moves Made : " + inMoves + " Picture Puzzle Game");
                        inMoves = 0;
                        counter = 60;
                        labelMovesMade.Text = objectDesign.labelMovesMade;
                        labelTime.Text = objectDesign.labelTimeStart;
                        shuffle();
                    }
                }
            }
        }

        private void buttonPaused_Click(object sender, EventArgs e)
        {
            if (buttonPaused.Text == objectDesign.buttonPaused)
            {
                timer1.Stop();
                gbPuzzleBox.Visible = false;
                buttonPaused.Text = objectDesign.buttonResume;
            }
            else
            {
                timer1.Start();
                gbPuzzleBox.Visible = true;
                buttonPaused.Text = objectDesign.buttonPaused;
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
