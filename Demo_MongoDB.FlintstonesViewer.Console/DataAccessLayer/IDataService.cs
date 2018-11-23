using System.Collections.Generic;

namespace Demo_NTier_PresentationLayer
{
    public interface IDataService
    {
        IEnumerable<Character> ReadAll(out MongoDbStatusCode statusCode);
        void WriteAll(IEnumerable<Character> characters, out MongoDbStatusCode statusCode);
    }
}
