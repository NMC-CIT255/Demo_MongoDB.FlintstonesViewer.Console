using Demo_NTier_DomainLayer;
using System.Collections.Generic;

namespace Demo_NTier_DataAccessLayer
{
    public interface IDataService
    {
        IEnumerable<Character> ReadAll(out DalErrorCode statusCode);
        void WriteAll(IEnumerable<Character> characters, out DalErrorCode statusCode);
    }
}
