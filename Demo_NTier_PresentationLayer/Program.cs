using Demo_NTier_DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_PresentationLayer
{
    //
    // TODO: refactoring into multiple projects and this project needs to become
    //       a C# .NET Framework Library
    //
    class Program
    {
        static void Main(string[] args)
        {
            //IDataService dataService = new MongoDBSimpleDataService();

            //
            // Required to test the MongoDB data service
            // refresh MongoDB collection 
            //
            //dataService.WriteAll(GenerateListOfCharacters(), out MongoDbStatusCode statusCode);
            //MongoDbStatusCode statusCode = MongoDbStatusCode.GOOD;

            //if (statusCode == MongoDbStatusCode.GOOD)
            //{
            //    CharacterBLL characterBLL = new CharacterBLL(dataService);
            //    Presenter presenter = new Presenter(characterBLL);
            //}
            //else
            //{
            //    Console.WriteLine("There was an error connecting to data file.");
            //}
        }

        private static List<Character> GenerateListOfCharacters()
        {
            List<Character> characters = new List<Character>()
            {
                new Character()
                {
                    Id = 1,
                    LastName = "Flintstone",
                    FirstName = "Fred",
                    Age = 28,
                    Gender = Character.GenderType.MALE
                },
                new Character()
                {
                    Id = 2,
                    LastName = "Rubble",
                    FirstName = "Barney",
                    Age = 28,
                    Gender = Character.GenderType.FEMALE
                },
                new Character()
                {
                    Id = 3,
                    LastName = "Flintstone",
                    FirstName = "Wilma",
                    Age = 27,
                    Gender = Character.GenderType.FEMALE
                },
                new Character()
                {
                    Id = 4,
                    LastName = "Flintstone",
                    FirstName = "Pebbles",
                    Age = 1,
                    Gender = Character.GenderType.FEMALE
                },
                new Character()
                {
                    Id = 5,
                    LastName = "Rubble",
                    FirstName = "Betty",
                    Age = 26,
                    Gender = Character.GenderType.FEMALE
                },
                new Character()
                {
                    Id = 6,
                    LastName = "Rubble",
                    FirstName = "Bamm-Bamm",
                    Age = 2,
                    Gender = Character.GenderType.MALE
                },
                                new Character()
                {
                    Id = 7,
                    LastName = "",
                    FirstName = "Dino",
                    Age = 7,
                    Gender = Character.GenderType.FEMALE
                }
            };

            return characters;
        }
    }
}
