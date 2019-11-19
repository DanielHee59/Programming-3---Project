using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AT3
{
    public partial class MusicPlayer : Form
    {
        public MusicPlayer()
        {
            InitializeComponent();
            displayMusic();
            axWindowsMediaPlayer1.uiMode = "none"; //Make everything disapper on media player
        }

        LinkedList<string> musicCollection = new LinkedList<string>();
        string song = "";


        private void displayMusic()
        {
            //Clear listBox
            lstMusic.Items.Clear();

            //Display the total number of songs
            tbTotalSongs.Text = numberOfSongs().ToString();

            //Display LinkedList
            foreach (var music in musicCollection)
            {
                lstMusic.Items.Add(music);
            }

        }

        private int numberOfSongs()
        {
            return musicCollection.Count; //Count the total number of song added to the listBox
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();

                if (op.ShowDialog() == DialogResult.OK)
                {
                    song = op.FileName; //this display the path name of the file added to the listBox
                    musicCollection.AddLast(song.ToString());
                    displayMusic();
                }
            }
            catch
            {
                MessageBox.Show("No song added");
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                // Test whether Windows Media Player is in the playing state. 
                if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    axWindowsMediaPlayer1.URL = song; //find song path
                    axWindowsMediaPlayer1.Ctlcontrols.play(); //play song from selected path
                }

            }
            catch
            {
                MessageBox.Show("No Song");
            }

        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
            catch
            {
                MessageBox.Show("No Song");
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                song = musicCollection.First.Value; //play the first song from the current list
                axWindowsMediaPlayer1.URL = song;
            }
            catch
            {
                MessageBox.Show("No Song");
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                song = musicCollection.Last.Value; //play the last song from the current list
                axWindowsMediaPlayer1.URL = song;
            }
            catch
            {
                MessageBox.Show("No Song");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                song = musicCollection.Find(song).Previous.Value; //play the previous song from the current list
                axWindowsMediaPlayer1.URL = song;

            }
            catch
            {
                MessageBox.Show("No Song");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                song = musicCollection.Find(song).Next.Value; //play the next song from the current list
                axWindowsMediaPlayer1.URL = song;
            }
            catch
            {
                MessageBox.Show("No Song");
            }
        }

        //When listBox is clicked twice, data will be erase
        private void LstMusic_DoubleClick(object sender, EventArgs e)
        {
            musicCollection.Clear(); //Clear LinkedList
            lstMusic.Items.Clear(); //Clear ListBox
            tbTotalSongs.Text = numberOfSongs().ToString();
        }


        //Save Music path to musicCollection.dat file
        public void serializeMusic()
        {

            //Open a stream for writing 
            //Sava the list to "musicCollection.dat"
            FileStream writerFileStream = new FileStream("musicCollection.dat", FileMode.Create);
            //Use it to serialize the data to the stream
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                bf.Serialize(writerFileStream, musicCollection);
            }
            catch (SerializationException e) //Catch an exception
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                writerFileStream.Close();
            }
        }

        //Load music file path from musicCollection.dat file
        public void deserializeMusic()
        {

            if (File.Exists("musicCollection.dat"))
            {
                musicCollection = null;

                //Open the file containing the data that you want to deserialize
                FileStream readFileStream = new FileStream("musicCollection.dat", FileMode.Open);

                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    //Desrialize the List from the file
                    musicCollection = (LinkedList<string>)bf.Deserialize(readFileStream);

                    foreach (var t in musicCollection)
                    {
                        lstMusic.Items.Add(t.ToString()); //Add saved song from dat file to ListBox
                        song = t; //Add song back to song variable, so  it can be played when form is loaded
                    }


                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    readFileStream.Close();
                }
            }
            else
            {
                MessageBox.Show("Files does not exist", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //When form is closing, it will save all music file path into musicCollection.dat
        private void MusicPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            serializeMusic();
        }

        //When form is load, it will load all music file path from musicCollection.dat
        private void MusicPlayer_Load(object sender, EventArgs e)
        {
            deserializeMusic();
            tbTotalSongs.Text = numberOfSongs().ToString();

        }

        //Full Screen Button
        private void BtnFullScreen_Click(object sender, EventArgs e)
        {
            try
            {
                if (axWindowsMediaPlayer1.URL.Length > 0)
                {
                    axWindowsMediaPlayer1.fullScreen = true;
                }
                else
                {
                    MessageBox.Show("No Song is Playing!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        //Stop button
        private void BtnStop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void TrackBarMusic_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBarMusic.Value;
        }


    }
}
