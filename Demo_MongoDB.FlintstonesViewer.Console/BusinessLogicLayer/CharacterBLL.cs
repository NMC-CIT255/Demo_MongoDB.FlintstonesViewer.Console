using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_FileIO_NTier.DataAccessLayer;
using Demo_FileIO_NTier.Models;

namespace Demo_FileIO_NTier.BusinessLogicLayer
{
    class CharacterBLL
    {
        IDataService _dataService;
        //List<Character> _characters;

        public CharacterBLL(IDataService dataservice)
        {
            _dataService = dataservice;
        }

        /// <summary>
        /// get IEnumberable of all characters sorted by Id
        /// </summary>
        /// <param name="statusCode">operation status</param>
        /// <param name="message">error message</param>
        /// <returns></returns>
        public IEnumerable<Character> GetAllCharacters(out MongoDbStatusCode statusCode, out string message)
        {
            List<Character> characters = null;
            message = "";
            characters = _dataService.ReadAll(out statusCode) as List<Character>;

            if (statusCode == MongoDbStatusCode.GOOD)
            {
                if (characters != null)
                {
                    characters.OrderBy(c => c.Id);
                }
            }
            else
            {
                message = "An error occurred connecting to the database.";
            }

            return characters;
        }

        /// <summary>
        /// get character by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="statusCode">status code</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public Character GetCharacterById(int id, out MongoDbStatusCode statusCode, out string message)
        {
            message = "";
            Character character = null;

            List<Character> characters = _dataService.ReadAll(out statusCode) as List<Character>;

            if (statusCode == MongoDbStatusCode.GOOD)
            {
                character = characters.FirstOrDefault(c => c.Id == id);

                if (character == null)
                {
                    message = $"No character has id {id}.";
                    statusCode = MongoDbStatusCode.ERROR;
                }
            }

            return character;
        }

        /// <summary>
        /// add a character to the data file
        /// </summary>
        /// <param name="character">character</param>
        /// <param name="statusCode">status code</param>
        /// <param name="message">message</param>
        public void AddCharacter(Character character, out MongoDbStatusCode statusCode, out string message)
        {
            message = "";

            List<Character> characters = _dataService.ReadAll(out statusCode) as List<Character>;

            if (statusCode == MongoDbStatusCode.GOOD)
            {
                if (characters != null)
                {
                    characters.Add(character);
                }
            }

            _dataService.WriteAll(characters, out statusCode);

            if (statusCode == MongoDbStatusCode.ERROR)
            {
                message = "There was an error connecting to the data file.";
            }
        }

        /// <summary>
        /// delete a character from the data file
        /// </summary>
        /// <param name="character">character</param>
        /// <param name="statusCode">status code</param>
        /// <param name="message">message</param>
        internal void DeleteCharacter(int id, out MongoDbStatusCode statusCode, out string message)
        {
            message = "";

            List<Character> characters = _dataService.ReadAll(out statusCode) as List<Character>;

            if (statusCode == MongoDbStatusCode.GOOD)
            {
                if (characters.Exists(c => c.Id == id))
                {
                    characters.Remove(characters.FirstOrDefault(c => c.Id == id));
                    _dataService.WriteAll(characters, out statusCode);
                    if (statusCode == MongoDbStatusCode.ERROR)
                    {
                        message = "There was an error connecting to the data file.";
                    }
                }
                else
                {
                    message = $"Character with id {id} does not exist.";
                    statusCode = MongoDbStatusCode.ERROR;
                }
            }
            else
            {
                message = "There was an error connecting to the data file.";
            }
        }

        /// <summary>
        /// update a character in the data file
        /// </summary>
        /// <param name="character">character</param>
        /// <param name="statusCode">status code</param>
        /// <param name="message">message</param>
        public void UpdateCharacter(Character character, out MongoDbStatusCode statusCode, out string message)
        {
            message = "";

            List<Character> characters = _dataService.ReadAll(out statusCode) as List<Character>;

            if (statusCode == MongoDbStatusCode.GOOD)
            {
                if (characters != null)
                {
                    if (characters.Exists(c => c.Id == character.Id))
                    {
                        characters.Remove(characters.FirstOrDefault(c => c.Id == character.Id));
                        characters.Add(character);
                        _dataService.WriteAll(characters, out statusCode);
                        if (statusCode == MongoDbStatusCode.ERROR)
                        {
                            message = "There was an error connecting to the data file.";
                        }
                    }
                    else
                    {
                        message = "Unable to locate character in data file.";
                        statusCode = MongoDbStatusCode.ERROR;
                    }
                }
            }
        }

        public int NextIdNumber()
        {
            int nextIdNumber = 0;

            List<Character> characters = _dataService.ReadAll(out MongoDbStatusCode statusCode) as List<Character>;

            if (statusCode == MongoDbStatusCode.GOOD)
            {
                nextIdNumber = characters.Max(c => c.Id) + 1;
            }

            return nextIdNumber;
        }
    }
}
