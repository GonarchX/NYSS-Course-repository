using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Work.Utils
{
    /// <summary>
    /// Provides the ability to encrypt / decrypt text using the Vigenere cipher
    /// </summary>
    public static class VigenereCipher
    {
        private static readonly string lowCaseAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private static readonly string upCaseAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        /// <summary>
        /// Encrypts the received text with the Vigenere cipher 
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <param name="step">Alphabetical shift amount</param>
        /// <returns>Encrypted text</returns>
        public static string EncryptText(string text, string codeWord)
        {
            if (text == null)
            {
                throw new ArgumentNullException("Text shouldn't be null!");
            }
            else if (codeWord == null)
            {
                throw new ArgumentNullException("Code word shouldn't be null!");
            }

            codeWord = codeWord.ToLower();
            char[] resultText = text.ToCharArray();
            if (!codeWord.Any(x => lowCaseAlphabet.Contains(x))) return text;

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                if (lowCaseAlphabet.Contains(codeWord[j % codeWord.Length]))
                {
                    if (lowCaseAlphabet.Contains(text[i]))
                    {
                        resultText[i] = CaesarCipher.EncryptLetter(text[i], (uint)lowCaseAlphabet.IndexOf(codeWord[j % codeWord.Length]));
                        j++;
                    }
                    else if (upCaseAlphabet.Contains(text[i]))
                    {
                        resultText[i] = CaesarCipher.EncryptLetter(text[i], (uint)lowCaseAlphabet.IndexOf(codeWord[j % codeWord.Length]));
                        j++;
                    }
                }
                else
                {
                    i--;
                    j++;
                }
            }
            return new string(resultText);
        }

        /// <summary>
        /// Decrypts the received text with the Vigenere cipher 
        /// </summary>
        /// <param name="text">Text to decrypt</param>
        /// <param name="step">Alphabetical shift amount</param>
        /// <returns>Decrypted text</returns>
        public static string DecryptText(string text, string codeWord)
        {
            if (text == null)
            {
                throw new ArgumentNullException("Text shouldn't be null!");
            }
            else if (codeWord == null)
            {
                throw new ArgumentNullException("Code word shouldn't be null!");
            }

            codeWord = codeWord.ToLower();
            char[] resultText = text.ToCharArray();
            if (!codeWord.Any(x => lowCaseAlphabet.Contains(x))) return text;

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                if (lowCaseAlphabet.Contains(codeWord[j % codeWord.Length]))
                {
                    if (lowCaseAlphabet.Contains(text[i]))
                    {
                        resultText[i] = CaesarCipher.DecryptLetter(text[i], (uint)lowCaseAlphabet.IndexOf(codeWord[j % codeWord.Length]));
                        j++;
                    }
                    else if (upCaseAlphabet.Contains(text[i]))
                    {
                        resultText[i] = CaesarCipher.DecryptLetter(text[i], (uint)lowCaseAlphabet.IndexOf(codeWord[j % codeWord.Length]));
                        j++;
                    }
                }
                else
                {
                    i--;
                    j++;
                }
            }
            return new string(resultText);
        }
    }
}
