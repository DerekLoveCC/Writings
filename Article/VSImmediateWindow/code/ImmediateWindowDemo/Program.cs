using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImmediateWindowDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var persons = new List<Person>
            {
                new Person{Name="zach", Age=30, Gender="man"},
                new Person{Name="bob", Age=18, Gender="man"},
                new Person{Name="alice", Age=19, Gender="woman"},
                new Person{Name="james", Age=16, Gender="man"},
                new Person{Name="july", Age=20, Gender="woman"},
            };

            var firstPerson = persons.FirstOrDefault();

            try
            {
                MethodWhichThrowException();
            }
            catch
            {
            }

            GetPersonsAgeUnder18(persons);
        }

        private static IList<Person> GetPersonsAgeUnder18(List<Person> persons)
        {
            return persons.Where(p => p.Age < 18).ToList();
        }

        private static void MethodWhichThrowException()
        {
            throw new NotImplementedException();
        }

        private static int count = 0;

        private static void IncreaeCount()
        {
            count++;
        }
    }
}
