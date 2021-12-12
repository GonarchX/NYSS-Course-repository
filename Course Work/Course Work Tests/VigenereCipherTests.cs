using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Course_Work.Utils;

namespace Tests
{
    [TestClass]
    public class VigenereCipherTests
    {
        class TestData
        {
            public string EncodedText { get; set; }
            public string CodeWord { get; set; }
            public string DecodedText { get; set; }

            public TestData(string encodedText, string codeWord, string decodedText)
            {
                EncodedText = encodedText;
                CodeWord = codeWord;
                DecodedText = decodedText;
            }
        }

        static List<TestData> testData = new List<TestData>()
        {
            new TestData(
                encodedText: "Бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! у ъящэячэц ъэюоык, едщ бдв саэацкшгнбяр гчеа кчфцшубп цу ьгщпя вщвсящ, эвэчрысй юяуъщнщхо шпуъликугбз чъцшья с цощъвчщ ъфмес ю лгюлэ ёъяяр! с моыящш шпмоец щаярдш цяэубфъ аьгэотызуа дщ, щръ кй юцкъщчьуац уыхэцэ ясч юбюяуяг ыовзсгюамщщ. внютвж тхыч эядкъябе цн юкъль, мэсццогл шяьфыоэьь ть эщсщжнашанэ ыюцен, уёюяыцчан мах гъъьуун шпмоыъй ч яяьпщъхэтпык яущм бпйэае! чэьюмуд, оээ скфч саьбрвчёыа эядуцйт ъ уьгфщуяяёу фси а эацэтшцэч юпапёи, ьь уъубфмч ысь хффы ужц чьяцнааущ эгъщйаъф, ч п эиттпьк ярвчг гмубзньцы! щб ьшяо шачюрэсч FirstLineSoftware ц ешчтфщацдпбр шыыь, р ыоф ячцсвкрщве бттй а ядсецсцкюкх эшашёрэсуъ якжще увюгщр в# уфн ысвчюпжзцж! чй ёюычъ бщххыибй еьюхечр п хкъмэншёцч юятщвфцшчщ с хчю ъэ ч аачсюсчыщачрняун в шъюьэжцясиьццч агфуо ацаьяычсцы .Net, чэбф ыуюбпьщо с чыдпяхбцйг щктрж!",
                codeWord: "Скорпион",
                decodedText: "Поздравляю, ты получил исходный текст!!! в принципе понять, что тут используется шифр виженера не особо трудно, основная подсказка заключается именно в наличии ключа у этого шифра! в данной задаче особый интерес составляет то, как вы реализуете именно сам процесс расшифровки. теперь дело осталось за малым, доделать программу до логического конца, выполнить все условия задания и опубликовать свою работу! молодец, это были достаточно трудные и интересные два с половиной месяца, но впереди нас ждет еще множество открытий, и я надеюсь общих свершений! от лица компании FirstLineSoftware и университета итмо, я рад поздравить тебя с официальным окончанием наших курсов с# для начинающих! мы хотим пожелать успехов в дальнейшем погружении в мир ит и программирования с использованием стека технологий .Net, море терпения и интересных задач!"
            ),
            new TestData(
                encodedText: "К юга ёэч ъдщхьчиц ёрьчф ркахииэнцпюмт хьые??",
                codeWord: "курсики",
                decodedText: "А кто это придумал такой замечательный курс??"
                ),
            new TestData(
                encodedText: "Ооо збт су Дцуоьосоя!!",
                codeWord: "КодNYSS",
                decodedText: "Дак это же Александр!!"
                ),
            new TestData(
                encodedText: "Ъ0кхю1 я2яыi ь ряGLIтьхьх т4умё б3 и2fruпыьй?",
                codeWord: "к1у1р1с1и1111к",
                decodedText: "П0чем1 ц2фрi и анGLIйские б4квы н3 ш2fruются?"
                ),
            new TestData(
                encodedText: "Почему не шифрует буквы буквами, я же стараюсь",
                codeWord: "аааа",
                decodedText: "Почему не шифрует буквы буквами, я же стараюсь"
                ),
            new TestData(
                encodedText: "Почему не шифрует текст с помощью цифр",
                codeWord: "1234",
                decodedText: "Почему не шифрует текст с помощью цифр"
                ),
            new TestData(
                encodedText: "Почему не шифрует текст с помощью невалидных символов",
                codeWord: "1T2E3S4T",
                decodedText: "Почему не шифрует текст с помощью невалидных символов"
                ),
        };

        #region Encrypt tests

        [TestMethod]
        public void EncryptText_NotValidCodeWord_OnlyDigits()
        {
            string actual = VigenereCipher.EncryptText("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", "1251257");
            Assert.AreEqual("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", actual);
        }

        [TestMethod]
        public void EncryptText_NotValidCodeWord_OnlyLetters()
        {
            string actual = VigenereCipher.EncryptText("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", "asdgasdggads");
            Assert.AreEqual("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", actual);
        }

        [TestMethod]
        public void EncryptText_NotValidCodeWord_OnlyDigitsOrLetters()
        {
            string actual = VigenereCipher.EncryptText("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", "cbfd125had1252");
            Assert.AreEqual("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", actual);
        }

        [TestMethod]
        public void EncryptText_AllTests()
        {
            foreach (var testCase in testData)
            {
                string actual = VigenereCipher.EncryptText(testCase.DecodedText, testCase.CodeWord);
                Assert.AreEqual(testCase.EncodedText, actual);
            }
        }

        #endregion

        #region Decrypt tests

        [TestMethod]
        public void DecryptText_NotValidCodeWord_OnlyDigits()
        {
            string actual = VigenereCipher.DecryptText("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", "1251257");
            Assert.AreEqual("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", actual);
        }

        [TestMethod]
        public void DecryptText_NotValidCodeWord_OnlyLetters()
        {
            string actual = VigenereCipher.DecryptText("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", "asdgasdggads");
            Assert.AreEqual("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", actual);
        }

        [TestMethod]
        public void DecryptText_NotValidCodeWord_OnlyDigitsOrLetters()
        {
            string actual = VigenereCipher.DecryptText("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", "cbfd125had1252");
            Assert.AreEqual("Како36й-то текстик№:!, :6 ту2%!:т есть зн№а5к%и пр3пин@ния и все vse prochee, int3ресно справitsya li shifr", actual);
        }

        [TestMethod]
        public void DecryptText_AllTests()
        {
            foreach (var testCase in testData)
            {
                string actual = VigenereCipher.DecryptText(testCase.EncodedText, testCase.CodeWord);
                Assert.AreEqual(testCase.DecodedText, actual);
            }
        }

        #endregion

        #region Exceptions tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptText_NullAgrumentException()
        {
            VigenereCipher.EncryptText(null, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptText_NullAgrumentException()
        {
            VigenereCipher.DecryptText(null, "");
        }
        #endregion
    }
}