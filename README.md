<h1>Steganography</h1>

This project displays an example of [steganography](https://en.wikipedia.org/wiki/Steganography).

The project is built using Visual Studio 2017 and should be built before run.

The program is executed by uploading a real image that will be hidden and a carrier image that we will use for hiding.
This implementation of steganography uses the least significant bit for storing data meaning that the LSB for each
pixel's R, G, and B value will encode one bit of the real image.

A password is used as well to encrypt the data by XORing the bits of the real image with our password that can be 
defined in the password field of the program.

