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
        List<Character> _characters;

        public CharacterBLL(IDataService dataservice)
        {
            _dataService = dataservice;
        }

        /// <summary>
        /// get IEnumberable of all characters sorted by Id
        /// </summary>
        /// <param name="success">operation status</param>
        /// <param name="message">error message</param>
        /// <returns></returns>
        public IEnumerable<Character> GetAllCharacters(out bool success, out string message)
        {
            _characters = null;
            success = false;
            message = "";
            try
            {
                _characters = _dataService.ReadAll() as List<Character>;
                _characters.OrderBy(c => c.Id);

                if (_characters.Count > 0)
                {
                    success = true;
                }
                else
                {
                    message = "It appears there is no data in the file.";
                }
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the data file.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return _characters;
        }

        /// <summary>
        /// save list of characters to data file
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public void SaveAllCharacters(List<Character> characters, out bool success, out string message)
        {
            _characters = null;
            success = false;
            message = "";
            try
            {
                _dataService.WriteAll(characters);
                success = true;
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the data file.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
        }

        /// <summary>
        /// get character by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="success">success</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public Character GetCharacterById(int id, out bool success, out string message)
        {
            success = false;
            message = "";

            _characters = GetAllCharacters(out success, out message) as List<Character>;
            Character character = _characters.FirstOrDefault(c => c.Id == id);

            if (character != null)
            {
                success = true;
            }
            else
            {
                message = $"No character has id {id}.";
                success = false;
            }

            return character;
        }

        public void AddCharacter(Character character, out bool success, out string message)
        {
            success = false;
            message = "";

            _characters = GetAllCharacters(out success, out message) as List<Character>;

            _characters.Add(character);

            SaveAllCharacters(_characters, out success, out message);
        }
    }
}
