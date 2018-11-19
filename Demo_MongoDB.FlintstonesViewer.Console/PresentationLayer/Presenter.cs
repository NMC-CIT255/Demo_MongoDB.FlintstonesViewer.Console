using System;
using System.Collections.Generic;
using System.Linq;
using Demo_FileIO_NTier.BusinessLogicLayer;
using Demo_FileIO_NTier.Models;
using System.Text;
using System.Threading.Tasks;

namespace Demo_FileIO_NTier.PresentationLayer
{
    class Presenter
    {
        static CharacterBLL _charactersBLL;
        static List<Character> _characters;

        public Presenter(CharacterBLL characterBLL)
        {
            _charactersBLL = characterBLL;
            _characters = new List<Character>();

            ManageApplicationLoop();
        }

        private void ManageApplicationLoop()
        {
            DisplayWelcomeScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        private void DisplayMainMenu()
        {
            char menuChoice;
            bool runApplicationLoop = true;

            while (runApplicationLoop)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Main Menu");
                Console.WriteLine();

                Console.WriteLine("\t1) Retrieve Characters from Data File");
                Console.WriteLine("\t2) Display Characters");
                Console.WriteLine("\t3) Add Character");
                Console.WriteLine("\t4) Delete Character");
                Console.WriteLine("\t5) Update Character");
                Console.WriteLine("\t6) Save Characters to Data File");
                Console.WriteLine("\tE) Exit");
                Console.WriteLine();
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadKey().KeyChar;

                runApplicationLoop = ProcessMainMenuChoice(menuChoice);
            }

        }

        private bool ProcessMainMenuChoice(char menuChoice)
        {
            bool runApplicationLoop = true;

            switch (menuChoice)
            {
                case '1':
                    DisplayRetrieveCharactersFromDataFile();
                    break;

                case '2':
                    DisplayListOfCharacters();
                    break;

                case '3':
                    DisplayAddCharacter();
                    break;

                case '4':
                    DisplayDeleteCharacter();
                    break;

                case '5':
                    DisplayUpdateCharacter();
                    break;

                case '6':
                    DisplaySaveCharactersFromDataFile();
                    break;

                case 'e':
                case 'E':
                    runApplicationLoop = false;
                    break;

                default:
                    break;
            }

            return runApplicationLoop;
        }

        private void DisplayUpdateCharacter()
        {
            throw new NotImplementedException();
        }

        private void DisplayDeleteCharacter()
        {
            DisplayHeader("Delete Character");

            DisplayCharacterTable();

            Console.Write("Enter Id of Character to Delete:");
            int.TryParse(Console.ReadLine(), out int id);

            _characters.Remove(_characters.FirstOrDefault(c => c.Id == id));

            DisplayContinuePrompt();
        }

        private void DisplayAddCharacter()
        {
            Character character = new Character();

            // get current max id and increment for new id
            character.Id = _characters.Max(c => c.Id) + 1;

            DisplayHeader("Add Character");

            Console.Write("Enter First Name:");
            character.FirstName = Console.ReadLine();
            Console.Write("Enter Last Name:");
            character.LastName = Console.ReadLine();
            Console.Write("Enter Age:");
            int.TryParse(Console.ReadLine(), out int age);
            character.Age = age;
            Console.Write("Enter Gender:");
            Enum.TryParse(Console.ReadLine().ToUpper(), out Character.GenderType gender);
            character.Gender = gender;

            Console.WriteLine();
            Console.WriteLine("New Character Added");
            Console.WriteLine($"\tId: {character.Id}");
            Console.WriteLine($"\tFirst Name: {character.FirstName}");
            Console.WriteLine($"\tLast Name: {character.LastName}");
            Console.WriteLine($"\tAge: {character.Age}");
            Console.WriteLine($"\tGender: {character.Gender}");

            _characters.Add(character);

            DisplayContinuePrompt();
        }

        private void DisplayRetrieveCharactersFromDataFile()
        {
            bool success;
            string message;

            DisplayHeader("Retrieve Characters from Data File");

            Console.WriteLine("Press any key to begin retrieving the data.");
            Console.ReadKey();

            List<Character> characters = _charactersBLL.RetrieveCharacters(out success, out message) as List<Character>;
            _characters = characters.OrderBy(c => c.Id).ToList();

            if (success)
            {
                Console.WriteLine();
                Console.WriteLine("Data retrieved.");
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        private void DisplaySaveCharactersFromDataFile()
        {
            bool success;
            string message;

            DisplayHeader("Save Characters to Data File");

            Console.WriteLine("Press any key to begin saving the data.");
            Console.ReadKey();            

            _charactersBLL.SaveCharacters(_characters, out success, out message);

            if (success)
            {
                Console.WriteLine();
                Console.WriteLine("Data saved.");
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }


        /// <summary>
        /// display a list of character ids and full name
        /// </summary>
        private void DisplayListOfCharacters()
        {
            DisplayHeader("List of Characters");

            DisplayCharacterTable();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display a table with id and full name columns
        /// </summary>
        /// <param name="characters">characters</param>
        private void DisplayCharacterTable()
        {
            if (_characters != null)
            {
                StringBuilder columnHeader = new StringBuilder();

                columnHeader.Append("Id".PadRight(8));
                columnHeader.Append("Full Name".PadRight(25));

                Console.WriteLine(columnHeader.ToString());

                _characters = _characters.OrderBy(c => c.Id).ToList();

                foreach (Character character in _characters)
                {
                    StringBuilder characterInfo = new StringBuilder();

                    characterInfo.Append(character.Id.ToString().PadRight(8));
                    characterInfo.Append(character.FullName().PadRight(25));

                    Console.WriteLine(characterInfo.ToString());
                }
            }
            else
            {
                Console.WriteLine("No characters exist currently.");
            }
        }

        /// <summary>
        /// display page header
        /// </summary>
        /// <param name="headerText">text for header</param>
        static void DisplayHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"\t\t{headerText}");
            Console.WriteLine();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display Welcome Screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tWelcome to the Flintstone Viewer");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Display Closing Screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using our application.");

            DisplayContinuePrompt();
        }

    }
}
