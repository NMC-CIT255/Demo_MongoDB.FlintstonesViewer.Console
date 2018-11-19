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
        public IEnumerable<Character> RetrieveCharacters(out bool success, out string message)
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


        public void SaveCharacters(List<Character> characters, out bool success, out string message)
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
    }
}
