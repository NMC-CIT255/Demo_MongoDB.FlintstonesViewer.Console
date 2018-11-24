using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using Demo_NTier_DomainLayer;

namespace Demo_NTier_DataAccessLayer
{
    public class MongoDBSimpleDataService : IDataService
    {

        static string _connectionString;

        /// <summary>
        /// read the mongoDb collection and load a list of character objects
        /// </summary>
        /// <returns>list of characters</returns>
        public IEnumerable<Character> ReadAll(out DalErrorCode statusCode)
        {
            List<Character> characters = new List<Character>();

            try
            {
                var client = new MongoClient(_connectionString);
                IMongoDatabase database = client.GetDatabase("cit255");
                IMongoCollection<Character> characterList = database.GetCollection<Character>("flintstone_characters");

                characters = characterList.Find(Builders<Character>.Filter.Empty).ToList();

                statusCode = DalErrorCode.GOOD;
            }
            catch (Exception)
            {
                statusCode = DalErrorCode.ERROR;
            }

            return characters;
        }

        /// <summary>
        /// write the current list of characters to the mongoDb collection
        /// </summary>
        /// <param name="characters">list of characters</param>
        public void WriteAll(IEnumerable<Character> characters, out DalErrorCode statusCode)
        {
            try
            {
                var client = new MongoClient(_connectionString);
                IMongoDatabase database = client.GetDatabase("cit255");
                IMongoCollection<Character> characterList = database.GetCollection<Character>("flintstone_characters");

                //
                // delete all documents in the collection to reset the collection
                //
                characterList.DeleteMany(Builders<Character>.Filter.Empty);

                characterList.InsertMany(characters);

                statusCode = DalErrorCode.GOOD;
            }
            catch (Exception)
            {
                statusCode = DalErrorCode.ERROR;
            }
        }

        public MongoDBSimpleDataService()
        {
            _connectionString = MongoDbDataSettings.connectionString;
        }
    }
}
