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

                Console.WriteLine("\t1) Retrieve Characters from Data File (Debug Only)");
                Console.WriteLine("\t2) Display Character List");
                Console.WriteLine("\t3) Display Character Detail");
                Console.WriteLine("\t4) Add Character");
                Console.WriteLine("\t5) Delete Character");
                Console.WriteLine("\t6) Update Character");
                Console.WriteLine("\t7) Save Characters to Data File (Debug Only)");
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
                    DisplayCharacterDetail();
                    break;

                case '4':
                    DisplayAddCharacter();
                    break;

                case '5':
                    DisplayDeleteCharacter();
                    break;

                case '6':
                    DisplayUpdateCharacter();
                    break;

                case '7':
                    DisplaySaveCharactersToDataFile();
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

        private void DisplayCharacterDetail()
        {
            Character character;

            DisplayHeader("Character Detail");

            DisplayCharacterTable();

            Console.Write("Enter Id of Character to View:");
            int.TryParse(Console.ReadLine(), out int id);

            character = _charactersBLL.GetCharacterById(id, out bool success, out string message);

            if (success)
            {
                DisplayHeader("Character Detail");
                DisplayCharacterInformation(character);
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// update character
        /// </summary>
        private void DisplayUpdateCharacter()
        {
            Character character;
            string userResponse;

            DisplayHeader("Update Character");

            DisplayCharacterTable();

            Console.Write("Enter Id of Character to Update:");
            int.TryParse(Console.ReadLine(), out int id);

            character = _characters.FirstOrDefault(c => c.Id == id);

            if (character != null)
            {
                DisplayHeader("Character Detail");
                Console.WriteLine("Current Character Information");
                DisplayCharacterInformation(character);
                Console.WriteLine();

                Console.WriteLine("Update each field or use the Enter key to keep the current information.");
                Console.WriteLine();

                Console.Write("Enter First Name:");
                userResponse = Console.ReadLine();
                if (userResponse != "")
                {
                    character.FirstName = userResponse;
                }

                Console.Write("Enter Last Name:");
                userResponse = Console.ReadLine();
                if (userResponse != "")
                {
                    character.LastName = Console.ReadLine();
                }

                Console.Write("Enter Age:");
                userResponse = Console.ReadLine();
                if (userResponse != "")
                {
                    int.TryParse(Console.ReadLine(), out int age);
                    character.Age = age;
                }

                Console.Write("Enter Gender:");
                userResponse = Console.ReadLine();
                if (userResponse != "")
                {
                    Enum.TryParse(Console.ReadLine().ToUpper(), out Character.GenderType gender);
                    character.Gender = gender;
                }
            }
            else
            {
                Console.WriteLine($"No character has id {id}.");
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// delete character
        /// </summary>
        private void DisplayDeleteCharacter()
        {
            DisplayHeader("Delete Character");

            DisplayCharacterTable();

            Console.Write("Enter Id of Character to Delete:");
            int.TryParse(Console.ReadLine(), out int id);

            _characters.Remove(_characters.FirstOrDefault(c => c.Id == id));

            DisplayContinuePrompt();
        }

        /// <summary>
        /// add character
        /// </summary>
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

            _charactersBLL.AddCharacter(character, out bool success, out string message);

            if (success)
            {
                Console.WriteLine("Character added.");
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// retrieve list of characters from data file
        /// </summary>
        private void DisplayRetrieveCharactersFromDataFile()
        {
            DisplayHeader("Retrieve Characters from Data File");

            Console.WriteLine("Press any key to begin retrieving the data.");
            Console.ReadKey();

            _characters = GetAllCharacters();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// get a list of characters ordered by id
        /// </summary>
        /// <returns>list of characters</returns>
        private List<Character> GetAllCharacters()
        {
            List<Character> characters = _charactersBLL.GetAllCharacters(out bool success, out string message) as List<Character>;
            _characters = characters.OrderBy(c => c.Id).ToList();

            if (!success)
            {
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
            }

            return characters;
        }

        /// <summary>
        /// save list of characters to data file
        /// </summary>
        private void DisplaySaveCharactersToDataFile()
        {
            bool success;
            string message;

            DisplayHeader("Save Characters to Data File");

            Console.WriteLine("Press any key to begin saving the data.");
            Console.ReadKey();

            _charactersBLL.SaveAllCharacters(_characters, out success, out message);

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

            _characters = GetAllCharacters();

            DisplayCharacterTable();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display the details of a character
        /// </summary>
        /// <param name="character">character</param>
        private void DisplayCharacterInformation(Character character)
        {
            Console.WriteLine($"\tName: {character.FirstName} {character.LastName}");
            Console.WriteLine($"\tId: {character.Id}");
            Console.WriteLine($"\tAge: {character.Age}");
            Console.WriteLine($"\tGender: {character.Gender}");
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
