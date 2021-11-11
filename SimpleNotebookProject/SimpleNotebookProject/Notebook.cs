using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotebookProject
{
    public class Notebook
    {
        public Dictionary<int, Note> allNotes = new Dictionary<int, Note>();

        private static void Greetings()
        {
            Console.WriteLine("Добро пожаловать в нашу записную книжку!");
            Console.WriteLine("\t- для создания новой записи введите команду: create.");
            Console.WriteLine("\t- для просмотра записи введите команду: show.");
            Console.WriteLine("\t- для редактирования записи введите команду: edit.");
            Console.WriteLine("\t- для удаления записи введите команду: del.");
            Console.WriteLine("\t- для просмотра списка всех записей введите команду: all.");
            Console.WriteLine("\t- для выхода из программы введите команду: exit.");
        }

        public void Action()
        {            
            while (true)
            {
                Greetings();
                bool isCorrectInput;

                do
                {
                    isCorrectInput = true;
                    Console.Write("Введите команду: ");

                    string inputResult = Console.ReadLine();
                    switch (inputResult)
                    {
                        case "create":
                            CreateNote();
                            break;
                        case "show":
                            ReadNote();
                            break;
                        case "edit":
                            UpdateNote();
                            break;
                        case "del":
                            DeleteNote();
                            break;
                        case "all":
                            ShowAllNotes();
                            break;
                        case "exit":
                            Console.WriteLine("Пока-пока!");
                            return;
                        default:
                            Console.Clear();
                            Console.Write("Данной команды не найдено! Попробуйте ещё раз: ");
                            isCorrectInput = false;
                            break;
                    }
                } while (!isCorrectInput);
            }
        }

        private void CreateNote()
        {
            Note note = new Note();
            note.Surname = ReadUntilValidationPass("Surname");
            note.Name = ReadUntilValidationPass("Name");
            note.SecondName = ReadUntilValidationPass("SecondName");
            note.Phone = ReadUntilValidationPass("Phone");
            note.Country = ReadUntilValidationPass("Country");
            note.DateOfBirth = ReadUntilValidationPass("DateOfBirth");
            note.Organization = ReadUntilValidationPass("Organization");
            note.Position = ReadUntilValidationPass("Position");
            note.Remark = ReadUntilValidationPass("Remark");
            note.Id = allNotes.Count();
            allNotes.Add(note.Id, note);
        }

        private void ReadNote()
        {
            Console.Write("Введите Id записи: ");

            int inputId;
            bool isCorrectId = Int32.TryParse(Console.ReadLine(), out inputId);

            if (isCorrectId)
            {
                if (allNotes.ContainsKey(inputId))
                {
                    Console.WriteLine(allNotes[inputId]);
                }
                else
                {
                    Console.WriteLine("Данной записи не найдено!");
                }
            }
            else
            {
                Console.WriteLine("Введен некорректный идентификатор!");
            }
        }

        private void UpdateNote()
        {
            Console.Write("Укажите ID записи для редактирования: ");

            if (!Int32.TryParse(Console.ReadLine(), out int inputId))
            {
                Console.WriteLine("Введен некорректный идентификатор!");
                return;
            }

            if (!allNotes.ContainsKey(inputId))
            {
                Console.WriteLine("Данной записи не найдено!");
                return;
            }

            Console.WriteLine(allNotes[inputId]);

            string userChoice;
            do
            {
                Console.Write("Какое поле необходимо отредактировать?\n" +
                    "\t1 - Фамилия\n" +
                    "\t2 - Имя\n" +
                    "\t3 - Отчество\n" +
                    "\t4 - Телефон\n" +
                    "\t5 - Страна\n" +
                    "\t6 - Дата рождения\n" +
                    "\t7 - Организация\n" +
                    "\t8 - Должность\n" +
                    "\t9 - Примечание\n" +
                    "Введите цифру для выбора или cancel для завершения редактирования: ");

                bool isCorrectChoice;
                do
                {
                    userChoice = Console.ReadLine();
                    isCorrectChoice = true;

                    switch (userChoice)
                    {
                        case "1":
                            allNotes[inputId].Surname = ReadUntilValidationPass("Surname");
                            break;
                        case "2":
                            allNotes[inputId].Name = ReadUntilValidationPass("Name");
                            break;
                        case "3":
                            allNotes[inputId].SecondName = ReadUntilValidationPass("SecondName");
                            break;
                        case "4":
                            allNotes[inputId].Phone = ReadUntilValidationPass("Phone");
                            break;
                        case "5":
                            allNotes[inputId].Country = ReadUntilValidationPass("Country");
                            break;
                        case "6":
                            allNotes[inputId].DateOfBirth = ReadUntilValidationPass("DateOfBirth");
                            break;
                        case "7":
                            allNotes[inputId].Organization = ReadUntilValidationPass("Organization");
                            break;
                        case "8":
                            allNotes[inputId].Position = ReadUntilValidationPass("Position");
                            break;
                        case "9":
                            allNotes[inputId].Remark = ReadUntilValidationPass("Remark");
                            break;
                        case "cancel":
                            return;
                        default:
                            Console.Write("Команда не найдена! Введите ещё раз: ");
                            isCorrectChoice = false;
                            break;
                    }
                }
                while (isCorrectChoice == false);


                Console.WriteLine("Поле изменено! Продолжить редактирование записи? (yes/no): ");
                userChoice = Console.ReadLine();
                while (userChoice != "yes" && userChoice != "no")
                {
                    Console.Write("Пожалуйста введите yes или no: ");
                    userChoice = Console.ReadLine();
                    Console.Clear();
                }
            }
            while (userChoice == "yes");
        }

        private void DeleteNote()
        {
            Console.Write("Введите Id записи для удаления: ");

            bool isCorrectId = Int32.TryParse(Console.ReadLine(), out int inputId);

            if (isCorrectId)
            {
                if (!allNotes.ContainsKey(inputId))
                {
                    Console.WriteLine("Данной записи не найдено!");
                }
                else
                {
                    allNotes.Remove(inputId);
                    Console.WriteLine($"Запись {inputId} удалена!");
                }
            }
            else
            {
                Console.WriteLine("Введен некорректный идентификатор!");
            }
        }

        private void ShowAllNotes()
        {
            if (allNotes.Count == 0) 
                Console.WriteLine("В вашей записной книжке еще нету записей!");

            foreach (var item in allNotes)
            {
                Console.WriteLine(item.Value.ToShortString());
            }
        }

        private string ReadUntilValidationPass(string notePropertyName)
        {
            Console.Write($"Введите {notePropertyName}: ");

            while (true)
            {
                string inputString = Console.ReadLine();

                if (Note.fieldsValidation[notePropertyName].
                    TryValidate(inputString, out string result))
                {
                    if (string.IsNullOrEmpty(inputString)) return null;
                    return inputString;
                }
                else Console.WriteLine(result);
            }
        }
    }
}