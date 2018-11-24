using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_DomainLayer
{
    public class Character
    {
        public enum GenderType { NOTSPECIFIED, MALE, FEMALE }

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
