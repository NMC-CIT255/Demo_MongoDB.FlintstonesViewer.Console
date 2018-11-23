using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Demo_NTier_PresentationLayer

{
    public class MongoDBSimpleDataService : IDataService
    {

        static string _connectionString;

        /// <summary>
        /// read the mongoDb collection and load a list of character objects
        /// </summary>
        /// <returns>list of characters</returns>
        public IEnumerable<Character> ReadAll(out MongoDbStatusCode statusCode)
        {
            List<Character> characters = new List<Character>();

            try
            {
                var client = new MongoClient(_connectionString);
                IMongoDatabase database = client.GetDatabase("cit255");
                IMongoCollection<Character> characterList = database.GetCollection<Character>("flintstone_characters");

                characters = characterList.Find(Builders<Character>.Filter.Empty).ToList();

                statusCode = MongoDbStatusCode.GOOD;
            }
            catch (Exception)
            {
                statusCode = MongoDbStatusCode.ERROR;
            }

            return characters;
        }

        /// <summary>
        /// write the current list of characters to the mongoDb collection
        /// </summary>
        /// <param name="characters">list of characters</param>
        public void WriteAll(IEnumerable<Character> characters, out MongoDbStatusCode statusCode)
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

                statusCode = MongoDbStatusCode.GOOD;
            }
            catch (Exception)
            {
                statusCode = MongoDbStatusCode.ERROR;
            }
        }

        public MongoDBSimpleDataService()
        {
            _connectionString = DataSettings.connectionString;
        }
    }
}
