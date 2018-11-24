using Demo_NTier_DataAccessLayer;
using Demo_NTier_DomainLayer;
using Demo_NTier_PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            ICharacterRepository characterRepository = new MongoDbCharacterRepository();

            //
            // use seed data to test the MongoDB data service
            //  
            //dataService.WriteAll(GenerateListOfCharacters(), out MongoDbStatusCode statusCode);
            //if (statusCode == MongoDbStatusCode.GOOD)
            //{
            //    CharacterBLL characterBLL = new CharacterBLL(dataService);
            //    Presenter presenter = new Presenter(characterBLL);
            //}
            //else
            //{
            //    Console.WriteLine("There was an error connecting to data file.");
            //}

            //
            // application startup
            //
            CharacterBLL characterBLL = new CharacterBLL(characterRepository);
            Presenter presenter = new Presenter(characterBLL);
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
