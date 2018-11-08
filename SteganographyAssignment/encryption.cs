/*========================================================================================
|   SOURCE: encryption.cs
|
|   AUTHOR: Alex Zielinski
|
|   DATE:   October 8, 2018
|
|   DESC:   This module holds functions that deal with data encryption and decryption
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
    class encryption
    {
        /*----------------------------------------------------------------------------------------
        |   FUNCTION: public static byte[] encrypt(Bitmap bmp, string key)
        |                   bmp: bitmap image to encrypt
        |                   key: The encryption key to use
        |
        |   AUTHOR: Alex Zielinski
        |
        |   DATE:   October 8, 2018
        |
        |   RETURN: byte array of encrypted BMP bytes
        |
        |   DESC:   Function simply takes the bitmap image from 'bmp' and encrypts the image bytes
        |           using 'key' and returns the encrypted byte array
        |----------------------------------------------------------------------------------------*/
        public static byte[] encrypt(Bitmap bmp, string key)
        {
            // get byte array of encryption key
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            // convert bitmap into byte array
            ImageConverter converter = new ImageConverter();
            byte[] bytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

            // byte array to hold cipher
            byte[] cipherBytes = new byte[bytes.Length];

            // go through bytes and encrypt using XOR cipher
            for (int i = 0; i < bytes.Length; i++)
                cipherBytes[i] = (byte)(bytes[i] ^ keyBytes[i % keyBytes.Length]);

            return cipherBytes;
        }


        /*----------------------------------------------------------------------------------------
        |   FUNCTION: public static byte[] decrypt(Byte[] bytes, string key)
        |                   bytes: bitmap byte array to decrypt
        |                   key: The decryptionkey to use
        |
        |   AUTHOR: Alex Zielinski
        |
        |   DATE:   October 8, 2018
        |
        |   RETURN: byte array of decrypted BMP bytes
        |
        |   DESC:   Function simply takes the byte array of a bitmap image from 'bytes' and 
        |           decrypts the encrypted bytes using 'key' and returns the decrypted byte array
        |----------------------------------------------------------------------------------------*/
        public static byte[] decrypt(Byte[] bytes, string key)
        {
            // get byte array of encryption key
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            // holds to decrypted bytes
            byte[] decryptBytes = new byte[bytes.Length];

            // decrypt
            for (int i = 0; i < bytes.Length; i++)
                decryptBytes[i] = (byte)(bytes[i] ^ keyBytes[i % keyBytes.Length]);

            return decryptBytes;
        }
    }
}
