/*========================================================================================
|   SOURCE: dcstego.cs
|
|   AUTHOR: Jordan Hamade
|
|   DATE:   October 8, 2018
|
|   DESC:   This module holds function for handling user input and GUI. Contains 
|           auto-generated functions for form handling.
|========================================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace SteganographyAssignment
{
    public partial class dcstego : Form
    {

        private Bitmap realImage = null;
        private Bitmap carrierImage = null;
        private Bitmap decryptImage = null;
        private Bitmap extractedImage = null;
        private Bitmap newImage = null;

        public dcstego()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // hide any intial error handling labels for later use
            errorLabel.Hide();
            errorLabelPW.Hide();
        }

        //Upload Real Image button
        private void button1_Click(object sender, EventArgs e)
        {
            // open a bmp file
            OpenFileDialog OFD1 = new OpenFileDialog();
            OFD1.Filter = "Image Files (*.bmp)|*.bmp";
            if(OFD1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // display the image on the GUI
                realPicture.Image = Image.FromFile(OFD1.FileName);
            }
        }

        // Upload Carrier Image Button
        private void button2_Click(object sender, EventArgs e)
        {
            // open a bmp file
            OpenFileDialog OFD1 = new OpenFileDialog();
            OFD1.Filter = "Image Files (*.bmp)|*.bmp";
            if (OFD1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // display the image on the GUI
                carrierPicture.Image = Image.FromFile(OFD1.FileName);
            }
        }


        //Start Button
        private void button3_Click(object sender, EventArgs e)
        {
            // get the images we will use for processing
            realImage = (Bitmap)realPicture.Image;
            carrierImage = (Bitmap)carrierPicture.Image;

            // error handling for null images
            if (realImage == null || carrierImage == null)
            {
                // display an error message
                errorLabel.Text = "You must upload both a real image and a carrier image!";
                errorLabel.Show();
                return;
            }
            errorLabel.Hide();

            // ---------------------
            //Password handling if necessary
            if(passwordBox.Text.Length < 6)
            {
                errorLabelPW.Text = "Please enter a password > 6 characters";
                errorLabelPW.Show();
                return;
            }
            errorLabelPW.Hide();
            // ---------------------

            //Embed our real image within the carrier
            string key = passwordBox.Text;
            newImage = dcimage.hideImage(realImage, carrierImage, key);

            //if our carrier is too small for our image
            if(newImage == null)
            {
                errorLabel.Text = "Carrier image is too small!";
                errorLabel.Show();
                return;
            }

            //save our image
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Bitmap Image|*.bmp";
            if(save_dialog.ShowDialog() == DialogResult.OK)
            {
                newImage.Save(save_dialog.FileName, ImageFormat.Bmp);
            }
        }

        //Decrypt Image Button
        private void button4_Click(object sender, EventArgs e)
        {
            //Password handling if necessary
            if (passwordBox.Text.Length < 6)
            {
                errorLabelPW.Text = "Please enter a password > 6 characters";
                errorLabelPW.Show();
                return;
            }
            errorLabelPW.Hide();

            OpenFileDialog OFD1 = new OpenFileDialog();
            OFD1.Filter = "Image Files (*.bmp)|*.bmp";
            dcimage sh = new dcimage();
            if (OFD1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // decrypt our image
                decryptImage = (Bitmap)Image.FromFile(OFD1.FileName);

                //save our image
                SaveFileDialog save_dialog = new SaveFileDialog();
                save_dialog.Filter = "Bitmap Image|*.bmp";
                Bitmap emptyImage = new Bitmap(100, 100);
                if (save_dialog.ShowDialog() == DialogResult.OK)
                {
                    emptyImage.Save(save_dialog.FileName, ImageFormat.Bmp);
                }

                //extractedImage = (Bitmap) sh.ExtractImage(decryptImage);
                string key = passwordBox.Text;
                sh.revealImage(decryptImage, save_dialog.FileName, key);
            }
            else
            {
                return;
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
