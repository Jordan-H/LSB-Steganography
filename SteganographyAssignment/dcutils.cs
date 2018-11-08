/*========================================================================================
|   SOURCE: dcutils.cs
|
|   AUTHOR: Jordan Hamade
|
|   DATE:   October 8, 2018
|
|   DESC:   This module holds functions for manipulating an image
|========================================================================================*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace SteganographyAssignment
{
    class dcutils
    {
        //The state we maintain when embeding an image
        public enum State
        {
            Hiding,
            Filling_With_Zeros
        };


        /*----------------------------------------------------------------------------------------
        |   FUNCTION: public static Bitmap EmbedImage(Bitmap real, Bitmap carrier, string key)
        |                   real: bitmap file we will be hiding
        |                   carrier: the image we will be hiding the picture within
        |                   key: the encryption key we are using
        |
        |   AUTHOR: Jordan Hamade
        |
        |   DATE:   October 8, 2018
        |
        |   RETURN: the carrier image with the real picture embeded within
        |
        |   DESC:   Function takes the real bitmap, encrypts the bytes with the specified key,
        |           and then hides the bytes within the least significant bit of the carrier image
        |----------------------------------------------------------------------------------------*/
        public static Bitmap EmbedImage(Bitmap real, Bitmap carrier, string key)
        {
            // Initially going to hide the image
            State state = State.Hiding;
            long pixelElementIndex = 0;

            // RGB values for a pixel
            int R = 0, G = 0, B = 0; 
            int bitIndex = 0;
            ImageConverter converter = new ImageConverter();

            // get encrypted byte array of image to hide
            byte[] bytes = encryption.encrypt(real, key);

            BitArray bits = new BitArray(bytes);

            // The max size of bits we can fit
            int max = carrier.Height * carrier.Width * 3;

            // Our image is too big for the carrier
            if(max < bits.Length)
            {
                return null;
            }

            // looping through every pixel in our carrier image
            for (int i = 0; i < carrier.Height; i++)
            {
                for(int j = 0; j < carrier.Width; j++)
                {
                    Color pixel = carrier.GetPixel(j, i); 

                    // initially set the LSB for each RGB byte to 0
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    // loop through 3 bits at a time to handle each RGB value respectively
                    for(int n = 0; n < 3; n++)
                    {
                        switch(pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if(state == State.Hiding)
                                    {
                                        // if the bit in our real image is 1, increment
                                        if (bits.Get(bitIndex))
                                        {
                                            R++;
                                        }
                                    }
                                }break;
                            case 1:
                                {
                                    if(state == State.Hiding)
                                    {
                                        if (bits.Get(bitIndex))
                                        {
                                            G++;
                                        }
                                    }
                                }break;
                            case 2:
                                {
                                    if(state == State.Hiding)
                                    {
                                        if (bits.Get(bitIndex))
                                        {
                                            B++;
                                        }
                                    }
                                }break;
                        }
                        bitIndex++;
                        pixelElementIndex++;

                        // determine if we have finished writing our real image to the carrier
                        if (bitIndex >= bits.Length)
                        {
                            // pad the rest of the image's LSB's with 0's
                            state = State.Filling_With_Zeros;
                        }

                        if (n == 2)
                        {
                            // Write every pixel after we modify it
                            carrier.SetPixel(j, i, Color.FromArgb(R, G, B));
                        }
                    }
                }
            }

            return carrier;
        }

        /*----------------------------------------------------------------------------------------
        |   FUNCTION: public void ExtractImage(Bitmap bmp, string path, string key)
        |                   bmp: bitmap file we will be extracting an image from
        |                   path: file path to save the image we are extracting
        |                   key: the encryption key we are using
        |
        |   AUTHOR: Jordan Hamade
        |
        |   DATE:   October 8, 2018
        |
        |   RETURN: None
        |
        |   DESC:   Function takes in a supposed carrier image, decrypts it with the provided key,
        |           and then writes to the file in the path given
        |----------------------------------------------------------------------------------------*/
        public void ExtractImage(Bitmap bmp, string path, string key)
        {
            int colorUnitIndex = 0;
            BitArray bits;
            bool validData = false;
            ArrayList list = new ArrayList();
            ArrayList tempList = new ArrayList();
            byte[] bytes;
            string eightBits = String.Empty;
            int value = 0;

            // looping through every pixel in our image
            for(int i = 0; i < bmp.Height; i++)
            {
                for(int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);
                    
                    // loop through every RGB pixel respectively
                    for(int n = 0; n < 3; n++)
                    {
                        switch(colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    // get the LSB and add it to our list of bits
                                    value = pixel.R % 2;
                                    tempList.Add(value);
                                }break;
                            case 1:
                                {
                                    value = pixel.G % 2;
                                    tempList.Add(value);
                                }
                                break;
                            case 2:
                                {
                                    value = pixel.B % 2;
                                    tempList.Add(value);
                                }
                                break;
                        }
                        colorUnitIndex++;

                        // check to see that we are not mindlessly adding padded 0's
                        if (value != 0)
                        {
                            validData = true;
                        }
                    }

                    // add to our real list of bits if the data is meaningful
                    if (validData)
                    {
                        validData = false;
                        list.AddRange(tempList);
                        tempList.Clear();
                    }
                }
            }

            bits = new BitArray(list.Count);

            // convert our list of 1's and 0's into a BitArray structure
            for(int x = 0; x < list.Count; x++)
            {
                bits.Set(x, ((int)list[x] != 0));
            }

            // convert our bits into a byte array
            bytes = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(bytes, 0);

            // decrypt
            byte[] hiddenBytes = encryption.decrypt(bytes, key);

            // save bytes to the file
            fileStreamImage(hiddenBytes, path);
        }

        /*----------------------------------------------------------------------------------------
        |   FUNCTION: private async void fileStreamImage(byte[] byteArrayIn, string path)
        |                   byteArrayIn: an array of bytes we will write to a file
        |                   path: file path pointing to where we will save our bytes
        |
        |   AUTHOR: Jordan Hamade
        |
        |   DATE:   October 8, 2018
        |
        |   RETURN: None
        |
        |   DESC:   Asynchronously save our data into a file, truncating anything previously in it
        |----------------------------------------------------------------------------------------*/
        private async void fileStreamImage(byte[] byteArrayIn, string path)
        {
            // creating our filestream object, specifying in Truncate mode
            using(FileStream fs = File.Open(path, FileMode.Truncate))
            {
                // seek to the beginning of the file
                fs.Seek(0, SeekOrigin.Begin);

                // write asynchronously
                await fs.WriteAsync(byteArrayIn, 0, byteArrayIn.Length);
            }
        }
    }
}
