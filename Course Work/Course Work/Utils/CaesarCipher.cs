using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Work.Utils
{
    class CaesarCipher
    {
        private static string lowCaseAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private static string upCaseAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        public static string Encrypt(string s, uint step)
        {
            char[] changedString = s.ToCharArray();
            int shift = (int)(step % 33);

            shift %= lowCaseAlphabet.Length;

            for (int i = 0; i < s.Length; i++)
            {
                if (lowCaseAlphabet.Contains(s[i]))
                {
                    changedString[i] = lowCaseAlphabet[(lowCaseAlphabet.IndexOf(s[i]) + lowCaseAlphabet.Count() + shift) % lowCaseAlphabet.Count()];
                }
                else if (upCaseAlphabet.Contains(s[i]))
                {
                    changedString[i] = upCaseAlphabet[(upCaseAlphabet.IndexOf(s[i]) + upCaseAlphabet.Count() + shift) % upCaseAlphabet.Count()];
                }
            }
            return new string(changedString);
        }

        public static string Decrypt(string s, uint step)
        {
            char[] changedString = s.ToCharArray();
            int shift = (int)(step % 33);

            shift %= lowCaseAlphabet.Length;

            for (int i = 0; i < s.Length; i++)
            {
                if (lowCaseAlphabet.Contains(s[i]))
                {
                    changedString[i] = lowCaseAlphabet[(lowCaseAlphabet.IndexOf(s[i]) + lowCaseAlphabet.Count() + shift) % lowCaseAlphabet.Count()];
                }
                else if (upCaseAlphabet.Contains(s[i]))
                {
                    changedString[i] = upCaseAlphabet[(upCaseAlphabet.IndexOf(s[i]) + upCaseAlphabet.Count() + shift) % upCaseAlphabet.Count()];
                }
            }
            return new string(changedString);
        }
    }
}
