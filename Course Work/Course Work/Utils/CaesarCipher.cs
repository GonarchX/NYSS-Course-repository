using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Work.Utils
{
    /// <summary>
    /// Provides the ability to encrypt / decrypt text using the Caesar cipher
    /// </summary>
    public static class CaesarCipher
    {
        private static readonly string lowCaseAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private static readonly string upCaseAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        /// <summary>
        /// Encrypts the received text with the Caesar cipher 
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <param name="step">Alphabetical shift amount</param>
        /// <returns>Encrypted text</returns>
        public static string EncryptText(string text, uint step)
        {
            if (text == null)
            {
                throw new ArgumentNullException("Text shouldn't be null!");
            }

            char[] resultTest = text.ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                resultTest[i] = EncryptLetter(text[i], step);
            }

            return new string(resultTest);
        }

        /// <summary>
        /// Encrypts the received letter with the Caesar cipher 
        /// </summary>
        /// <param name="letter">Letter to encrypt</param>
        /// <param name="step">Alphabetical shfit amount</param>
        /// <returns>Encrypted letter</returns>
        public static char EncryptLetter(char letter, uint step)
        {
            int shift = (int)(step % lowCaseAlphabet.Length);

            if (lowCaseAlphabet.Contains(letter))
            {
                letter = lowCaseAlphabet[(lowCaseAlphabet.IndexOf(letter) + lowCaseAlphabet.Count() + shift) % lowCaseAlphabet.Count()];
            }
            else if (upCaseAlphabet.Contains(letter))
            {
                letter = upCaseAlphabet[(upCaseAlphabet.IndexOf(letter) + upCaseAlphabet.Count() + shift) % upCaseAlphabet.Count()];
            }
            return letter;
        }

        /// <summary>
        /// Decrypts the received text with the Caesar cipher 
        /// </summary>
        /// <param name="text">Text to decrypt</param>
        /// <param name="step">Alphabetical shift amount</param>
        /// <returns>Decrypted text</returns>
        public static string DecryptText(string text, uint step)
        {
            if (text == null)
            {
                throw new ArgumentNullException("Text shouldn't be null!");
            }

            char[] resultTest = text.ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                resultTest[i] = DecryptLetter(text[i], step);
            }

            return new string(resultTest);
        }

        /// <summary>
        /// Dencrypts the received letter with the Caesar cipher 
        /// </summary>
        /// <param name="letter">Letter to dencrypt</param>
        /// <param name="step">Alphabetical shfit amount</param>
        /// <returns>Dencrypted letter</returns>
        public static char DecryptLetter(char letter, uint step)
        {
            int shift = (int)(step % lowCaseAlphabet.Length);
            shift = -shift;

            if (lowCaseAlphabet.Contains(letter))
            {
                letter = lowCaseAlphabet[(lowCaseAlphabet.IndexOf(letter) + lowCaseAlphabet.Count() + shift) % lowCaseAlphabet.Count()];
            }
            else if (upCaseAlphabet.Contains(letter))
            {
                letter = upCaseAlphabet[(upCaseAlphabet.IndexOf(letter) + upCaseAlphabet.Count() + shift) % upCaseAlphabet.Count()];
            }

            return letter;
        }
    }
}
