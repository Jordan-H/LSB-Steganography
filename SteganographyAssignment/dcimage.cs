/*========================================================================================
|   SOURCE: dcimage.cs
|
|   AUTHOR: Jordan Hamade
|
|   DATE:   October 8, 2018
|
|   DESC:   This module holds functions for image processing
|========================================================================================*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyAssignment
{
    class dcimage
    {
        /*----------------------------------------------------------------------------------------
        |   FUNCTION: public static Bitmap hideImage(Bitmap real, Bitmap carrier, string key)
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
        |   DESC:   Function to be called by the GUI to pass our data to the image manipulating 
        |           function
        |----------------------------------------------------------------------------------------*/
        public static Bitmap hideImage(Bitmap real, Bitmap carrier, string key)
        {
            return dcutils.EmbedImage(real, carrier, key);
        }

        /*----------------------------------------------------------------------------------------
        |   FUNCTION: public void revealImage(Bitmap carrier, string path, string key)
        |                   carrier: the image we will be extracting from
        |                   path: the path where we will save the image to
        |                   key: the encryption key we are using
        |
        |   AUTHOR: Jordan Hamade
        |
        |   DATE:   October 8, 2018
        |
        |   RETURN: None
        |
        |   DESC:   Function to be called by the GUI to pass our data to the image manipulating 
        |           function
        |----------------------------------------------------------------------------------------*/
        public void revealImage(Bitmap carrier, string path, string key)
        {
            dcutils dc = new dcutils();
            dc.ExtractImage(carrier, path, key);
        }
    }
}
