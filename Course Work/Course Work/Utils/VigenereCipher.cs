using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Work.Utils
{
    public class VigenereCipher
    {
        private static string lowCaseAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private static string upCaseAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

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
