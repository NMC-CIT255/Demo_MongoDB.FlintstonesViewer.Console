using Demo_NTier_DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_PresentationLayer
{
    public class Presenter
    {
        static CharacterBLL _charactersBLL;

        public Presenter(CharacterBLL characterBLL)
        {
            _charactersBLL = characterBLL;
            ManageApplicationLoop();
        }

        private void ManageApplicationLoop()
        {
            DisplayWelcomeScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        /// <summary>
        /// display main menu
        /// </summary>
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
                Console.WriteLine("\tE) Exit");
                Console.WriteLine();
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadKey().KeyChar;

                runApplicationLoop = ProcessMainMenuChoice(menuChoice);
            }

        }

        /// <summary>
        /// process main menu choice
        /// </summary>
        /// <param name="menuChoice">menu choice</param>
        /// <returns></returns>
        private bool ProcessMainMenuChoice(char menuChoice)
        {
            bool runApplicationLoop = true;

            switch (menuChoice)
            {
                case '1':
                    DisplayLoadCharactersFromDataFile();
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

                case 'e':
                case 'E':
                    runApplicationLoop = false;
                    break;

                default:
                    break;
            }

            return runApplicationLoop;
        }

        /// <summary>
        /// display list of character screen - ids and full name
        /// </summary>
        private void DisplayListOfCharacters()
        {
            DisplayHeader("List of Characters");

            List<Character> characters = _charactersBLL.GetAllCharacters(out DalErrorCode statusCode, out string message) as List<Character>;

            if (statusCode == DalErrorCode.GOOD)
            {
                DisplayCharacterListTable(characters);
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display character detail screen
        /// </summary>
        private void DisplayCharacterDetail()
        {
            Character character;

            DisplayHeader("Character Detail");

            List<Character> characters = _charactersBLL.GetAllCharacters(out DalErrorCode statusCode, out string message) as List<Character>;

            if (statusCode == DalErrorCode.GOOD)
            {
                DisplayCharacterListTable(characters);
            }
            else
            {
                Console.WriteLine(message);
            }

            Console.Write("Enter Id of Character to View:");
            int.TryParse(Console.ReadLine(), out int id);

            character = _charactersBLL.GetCharacterById(id, out statusCode, out message);

            if (statusCode == DalErrorCode.GOOD)
            {
                DisplayHeader("Character Detail");
                DisplayCharacterDetailTable(character);
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display add character screen
        /// </summary>
        private void DisplayAddCharacter()
        {
            Character character = new Character();

            // get current max id and increment for new id
            character.Id = _charactersBLL.NextIdNumber();

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
            Console.WriteLine("New Character To Add");
            Console.WriteLine($"\tId: {character.Id}");
            Console.WriteLine($"\tFirst Name: {character.FirstName}");
            Console.WriteLine($"\tLast Name: {character.LastName}");
            Console.WriteLine($"\tAge: {character.Age}");
            Console.WriteLine($"\tGender: {character.Gender}");

            _charactersBLL.AddCharacter(character, out DalErrorCode statusCode, out string message);

            if (statusCode == DalErrorCode.GOOD)
            {
                Console.WriteLine();
                Console.WriteLine("Character Added");
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display delete character screen
        /// </summary>
        private void DisplayDeleteCharacter()
        {
            DisplayHeader("Delete Character");

            List<Character> characters = _charactersBLL.GetAllCharacters(out DalErrorCode statusCode, out string message) as List<Character>;

            if (statusCode == DalErrorCode.GOOD)
            {
                DisplayCharacterListTable(characters);
            }
            else
            {
                Console.WriteLine(message);
            }

            Console.Write("Enter Id of Character to Delete:");
            int.TryParse(Console.ReadLine(), out int id);

            _charactersBLL.DeleteCharacter(id, out statusCode, out message);

            if (statusCode == DalErrorCode.GOOD)
            {
                Console.WriteLine("Character deleted.");
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display update character screen
        /// </summary>
        private void DisplayUpdateCharacter()
        {
            Character character;
            string userResponse;

            DisplayHeader("Update Character");

            List<Character> characters = _charactersBLL.GetAllCharacters(out DalErrorCode statusCode, out string message) as List<Character>;

            if (statusCode == DalErrorCode.GOOD)
            {
                DisplayCharacterListTable(characters);
            }
            else
            {
                Console.WriteLine(message);
            }

            Console.Write("Enter Id of Character to Update:");
            int.TryParse(Console.ReadLine(), out int id);

            character = characters.FirstOrDefault(c => c.Id == id);

            if (character != null)
            {
                DisplayHeader("Character Detail");
                Console.WriteLine("Current Character Information");
                DisplayCharacterDetailTable(character);
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
                    character.LastName = userResponse;
                }

                Console.Write("Enter Age:");
                userResponse = Console.ReadLine();
                if (userResponse != "")
                {
                    int.TryParse(userResponse, out int age);
                    character.Age = age;
                }

                Console.Write("Enter Gender:");
                userResponse = Console.ReadLine();
                if (userResponse != "")
                {
                    Enum.TryParse(userResponse.ToUpper(), out Character.GenderType gender);
                    character.Gender = gender;
                }

                _charactersBLL.UpdateCharacter(character, out statusCode, out message);

                if (statusCode == DalErrorCode.GOOD)
                {
                    Console.WriteLine("Character updated.");
                }
                else
                {
                    Console.WriteLine(message);
                }
            }
            else
            {
                Console.WriteLine($"No character has id {id}.");
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display load list of characters from data file screen (debug only)
        /// </summary>
        private void DisplayLoadCharactersFromDataFile()
        {
            DisplayHeader("Retrieve Characters from Data File");

            Console.WriteLine("Press any key to begin retrieving the data.");
            Console.ReadKey();

            List<Character> _characters = _charactersBLL.GetAllCharacters(out DalErrorCode statusCode, out string message) as List<Character>;

            if (statusCode == DalErrorCode.GOOD)
            {
                Console.WriteLine("Data retrieved.");
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        #region HELPER METHODS

        /// <summary>
        /// display details of a character table
        /// </summary>
        /// <param name="character">character</param>
        private void DisplayCharacterDetailTable(Character character)
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
        private void DisplayCharacterListTable(List<Character> characters)
        {
            if (characters != null)
            {
                StringBuilder columnHeader = new StringBuilder();

                columnHeader.Append("Id".PadRight(8));
                columnHeader.Append("Full Name".PadRight(25));

                Console.WriteLine(columnHeader.ToString());

                characters = characters.OrderBy(c => c.Id).ToList();

                foreach (Character character in characters)
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

        #endregion
    }
}
