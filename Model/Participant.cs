using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Participant : Entity<long>
    {
        public String Name { get; set; }
        public int Age { get; set; }



        public Participant(long id, String name, int age) : base(id)
        {
            Name = name;
            Age = age;

        }

        public override string ToString()
            => $"Participant {{ name: {Name}, age: {Age}}}";

        public override bool Equals(object obj)
        {
            if (obj is Participant other)
            {
                return Id == other.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}
