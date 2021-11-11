using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotebookProject
{
    public class Note
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public string Organization { get; set; }
        public string Position { get; set; }
        public string Remark { get; set; }
        public int Id { get; set; }

        public static Dictionary<string, Validation> fieldsValidation;
        public static char[] validWordSymbols = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя -".ToCharArray();
        public static char[] validNumberSymbols = "0123456789".ToCharArray();
        public static char[] validDateSymbols = "0123456789.".ToCharArray();
        public static char[] allValidSymbols = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789,.?!()\\+-=№@'\"&$;:^ ".ToCharArray();

        static Note()
        {
            fieldsValidation = new Dictionary<string, Validation>
            {
                ["Name"] = new Validation(true, 1, 20, validWordSymbols),
                ["Surname"] = new Validation(true, 1, 20, validWordSymbols),
                ["SecondName"] = new Validation(false, 0, 20, validWordSymbols),
                ["Phone"] = new Validation(true, 5, 11, validNumberSymbols),
                ["Country"] = new Validation(false, 0, 20, validWordSymbols),
                ["DateOfBirth"] = new Validation(false, 10, 10, validDateSymbols),
                ["Organization"] = new Validation(false, 0, 20, validWordSymbols),
                ["Position"] = new Validation(false, 0, 20, validWordSymbols),
                ["Remark"] = new Validation(false, 0, 200, allValidSymbols),
                ["Id"] = new Validation(true, 1, 10, validNumberSymbols)    
            };
        }

        public override string ToString()
        {
            return $"\n\tID: {Id}" +
                    $"\n\tФамилия: {Surname}" +
                    $"\n\tИмя: {Name}" +
                    $"\n\tОтчество: {SecondName}" +
                    $"\n\tНомер телефона: {Phone}" +
                    $"\n\tСтрана: {Country}" +
                    $"\n\tДата рождения: {DateOfBirth}" +
                    $"\n\tОрганизация: {Organization}" +
                    $"\n\tДолжность: {Position}" +
                    $"\n\tПримечание: {Remark}";
        }
        public string ToShortString()
        {
            return $"{Id} {Surname} {Name} {Phone}";
        }
    }
}
