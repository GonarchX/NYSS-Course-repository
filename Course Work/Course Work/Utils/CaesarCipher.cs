using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Work.Utils
{
    public class CaesarCipher
    {
        private static string lowCaseAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private static string upCaseAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

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
