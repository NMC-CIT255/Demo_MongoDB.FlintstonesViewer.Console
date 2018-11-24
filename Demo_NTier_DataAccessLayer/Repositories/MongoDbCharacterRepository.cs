using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_DomainLayer;
using MongoDB.Driver;

namespace Demo_NTier_DataAccessLayer
{
    public class MongoDbCharacterRepository : ICharacterRepository
    {
        List<Character> _characters;

        public MongoDbCharacterRepository()
        {
            _characters = new List<Character>();
        }

        public IEnumerable<Character> GetAll(out DalErrorCode dalErrorCode)
        {
            try
            {
                var client = new MongoClient(MongoDbDataSettings.connectionString);
                IMongoDatabase database = client.GetDatabase(MongoDbDataSettings.databaseName);
                IMongoCollection<Character> characterList = database.GetCollection<Character>(MongoDbDataSettings.characterCollectionName);

                _characters = characterList.Find(Builders<Character>.Filter.Empty).ToList();

                dalErrorCode = DalErrorCode.GOOD;
            }
            catch (Exception)
            {
                dalErrorCode = DalErrorCode.ERROR;
            }

            return _characters;
        }

        public Character GetById(int id, out DalErrorCode dalErrorCode)
        {
            throw new NotImplementedException();
        }

        public void Insert(Character character, out DalErrorCode dalErrorCode)
        {
            throw new NotImplementedException();
        }

        public void Update(Character character, out DalErrorCode dalErrorCode)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, out DalErrorCode dalErrorCode)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _characters = null;
        }
    }
}
