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
                var characterCollection = GetCharacterColletion();

                _characters = characterCollection.FindSync(Builders<Character>.Filter.Empty).ToList();

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
            Character character = null;

            try
            {
                var characterCollection = GetCharacterColletion();

                character = characterCollection.Find(Builders<Character>.Filter.Eq("Id", id)).SingleOrDefault() as Character;

                dalErrorCode = DalErrorCode.GOOD;
            }
            catch (Exception)
            {
                dalErrorCode = DalErrorCode.ERROR;
            }

            return character; ;
        }

        public void Insert(Character character, out DalErrorCode dalErrorCode)
        {
            try
            {
                var characterCollection = GetCharacterColletion();

                characterCollection.InsertOne(character);

                dalErrorCode = DalErrorCode.GOOD;
            }
            catch (Exception)
            {
                dalErrorCode = DalErrorCode.ERROR;
            }
        }

        public void Update(Character character, out DalErrorCode dalErrorCode)
        {
            try
            {
                var characterCollection = GetCharacterColletion();

                //characterList.Find(Builders<Character>.Filter.Eq("id", character.Id)). UpdateOne;
                characterCollection.FindOneAndReplace(Builders<Character>.Filter.Eq("Id", character.Id), character);

                dalErrorCode = DalErrorCode.GOOD;
            }
            catch (Exception)
            {
                dalErrorCode = DalErrorCode.ERROR;
            }
        }

        public void Delete(int id, out DalErrorCode dalErrorCode)
        {
            try
            {
                var characterCollection = GetCharacterColletion();

                var result = characterCollection.DeleteOne(Builders<Character>.Filter.Eq("Id", id));

                dalErrorCode = DalErrorCode.GOOD;
            }
            catch (Exception)
            {
                dalErrorCode = DalErrorCode.ERROR;
            }
        }

        private IMongoCollection<Character> GetCharacterColletion()
        {
            var client = new MongoClient(MongoDbDataSettings.connectionString);
            IMongoDatabase database = client.GetDatabase(MongoDbDataSettings.databaseName);
            IMongoCollection<Character> characterCollection = database.GetCollection<Character>(MongoDbDataSettings.characterCollectionName);

            return characterCollection;
        }

        public void Dispose()
        {
            _characters = null;
        }
    }
}
