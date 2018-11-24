using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_DomainLayer;

namespace Demo_NTier_DataAccessLayer
{
    /// <summary>
    /// Character repository interface
    /// Note: DAL Error Codes are passed to the Business Layer where they are handled.
    /// </summary>
    public interface ICharacterRepository : IDisposable
    {
        IEnumerable<Character> GetAll(out DalErrorCode dalErrorCode);
        Character GetById(int id, out DalErrorCode dalErrorCode);
        void Insert(Character character, out DalErrorCode dalErrorCode);
        void Update(Character character, out DalErrorCode dalErrorCode);
        void Delete(int id, out DalErrorCode dalErrorCode);
    }
}
