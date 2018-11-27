using Demo_NTier_DataAccessLayer;
using Demo_NTier_DomainLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_PresentationLayer
{
    public class CharacterBLL
    {
        ICharacterRepository _characterRepository;

        public CharacterBLL(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        /// <summary>
        /// get IEnumberable of all characters sorted by Id
        /// </summary>
        /// <param name="dalErrorCode">DAL error code</param>
        /// <param name="message">error message</param>
        /// <returns></returns>
        public IEnumerable<Character> GetAllCharacters(out DalErrorCode dalErrorCode, out string message)
        {
            List<Character> characters = null;
            message = "";

            using (_characterRepository)
            {
                characters = _characterRepository.GetAll(out dalErrorCode) as List<Character>;

                if (dalErrorCode == DalErrorCode.GOOD)
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
            }

            return characters;
        }

        /// <summary>
        /// get character by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="dalErrorCode">DAL error code</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public Character GetCharacterById(int id, out DalErrorCode dalErrorCode, out string message)
        {
            message = "";
            Character character;

            using (_characterRepository)
            {
                character = _characterRepository.GetById(id, out dalErrorCode);

                if (dalErrorCode == DalErrorCode.GOOD)
                {
                    if (character == null)
                    {
                        message = $"No character has id {id}.";
                        dalErrorCode = DalErrorCode.ERROR;
                    }
                }
                else
                {
                    message = "An error occurred connecting to the database.";
                }
            }

            return character;
        }

        /// <summary>
        /// add a character to the data file
        /// </summary>
        /// <param name="character">character</param>
        /// <param name="dalErrorCode">DAL error code</param>
        /// <param name="message">message</param>
        public void AddCharacter(Character character, out DalErrorCode dalErrorCode, out string message)
        {
            message = "";

            _characterRepository.Insert(character, out dalErrorCode);

            if (dalErrorCode == DalErrorCode.ERROR)
            {
                message = "There was an error connecting to the data file.";
            }
        }

        /// <summary>
        /// delete a character from the data file
        /// </summary>
        /// <param name="character">character</param>
        /// <param name="dalErrorCode">status code</param>
        /// <param name="message">message</param>
        public void DeleteCharacter(int id, out DalErrorCode dalErrorCode, out string message)
        {
            message = "";

            using (_characterRepository)
            {
                if (CharacterDocumentExists(id, out dalErrorCode))
                {
                    _characterRepository.Delete(id, out dalErrorCode);

                    if (dalErrorCode == DalErrorCode.ERROR)
                    {
                        message = "There was an error connecting to the data file.";
                    }
                }
                else
                {
                    message = $"Character with id {id} does not exist.";
                    dalErrorCode = DalErrorCode.ERROR;
                }
            }
        }

        private bool CharacterDocumentExists(int id, out DalErrorCode dalErrorCode)
        {
            using (_characterRepository)
            {
                if (_characterRepository.GetById(id, out dalErrorCode) != null)
                {
                    return true;
                }
                else
                {
                    dalErrorCode = DalErrorCode.ERROR;
                    return false;
                }
            }
        }

        /// <summary>
        /// update a character in the data file
        /// </summary>
        /// <param name="character">character</param>
        /// <param name="dalErrorCode">status code</param>
        /// <param name="message">message</param>
        public void UpdateCharacter(Character character, out DalErrorCode dalErrorCode, out string message)
        {
            message = "";

            using (_characterRepository)
            {
                if (CharacterDocumentExists(character.Id, out dalErrorCode))
                {
                    _characterRepository.Update(character, out dalErrorCode);

                    if (dalErrorCode == DalErrorCode.ERROR)
                    {
                        message = "There was an error connecting to the data file.";
                    }
                }
                else
                {
                    message = $"Character with id {character.Id} does not exist.";
                    dalErrorCode = DalErrorCode.ERROR;
                }
            }
        }

        /// <summary>
        /// generate the next id increment
        /// </summary>
        /// <returns>id value</returns>
        public int NextIdNumber()
        {
            int nextIdNumber = 0;

            List<Character> characters = _characterRepository.GetAll(out DalErrorCode statusCode) as List<Character>;

            if (statusCode == DalErrorCode.GOOD)
            {
                nextIdNumber = characters.Max(c => c.Id) + 1;
            }

            return nextIdNumber;
        }
    }
}
