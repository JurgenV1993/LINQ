using POCO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinqSamples
{
    public class Program
    {
        private static List<Person> peoples;
        private static List<Pet> pets;
        private static List<Employee> listStudents;
        public static void Main(string[] args)
        {
            setListofObject();
            LinqAgregate();
            //JoinKeyResults();
            //CompositeKeyResults();
            //NumberOfPetsPerOwner();
            //GroupJoin();
            Console.Read();
        }
        private static void setListofObject()
        {
            peoples = new List<Person>()
            {
               new Person {Id=1 ,FirstName = "Magnus", LastName = "Hedlund" },
               new Person {Id=2 ,FirstName = "Terry", LastName = "Adams" },
               new Person {Id=3 ,FirstName = "Charlotte", LastName = "Weiss" },
               new Person {Id=4 ,FirstName = "Arlene", LastName = "Huff" }
            };
            pets = new List<Pet>()
            {
                new Pet{ Name="Barley", Owner = "Terry",OwnerId=2},
                new Pet{ Name="Boots",  Owner = "Terry",OwnerId=2},
                new Pet{ Name="Whiskers",Owner = "Charlotte",OwnerId=3},
                new Pet{ Name="Blue Moon",Owner = "Rui",OwnerId=5},
                new Pet{ Name="Daisy",Owner = "Magnus",OwnerId=1}
            };
            listStudents = new List<Employee>()
            {
                new Employee{Id= 101,Name = "Preety", Salary = 10000, Department = "IT"},
                new Employee{Id= 102,Name = "Priyanka", Salary = 15000, Department = "Sales"},
                new Employee{Id= 103,Name = "James", Salary = 50000, Department = "Sales"},
                new Employee{Id= 104,Name = "Hina", Salary = 20000, Department = "IT"},
                new Employee{Id= 105,Name = "Anurag", Salary = 30000, Department = "IT"},
            };
        }

        private static void LinqAgregate()
        {
            var query = listStudents.GroupBy(l => l.Department)
                       .Select(g =>
                       {
                           var results = g.Aggregate(new EmployeeStatistics(),
                                        (acc, c) => acc.Accumulate(c),
                                         acc => acc.Compute());
                           return new
                           {
                               Name = g.Key,
                               Minimum = results.Min,
                               Maximum = results.Max,
                               Counting=results.Counting
                               //Average = results.Avg
                           };
                       }).OrderByDescending(r => r.Maximum);

            foreach (var s in query) 
            {
                Console.WriteLine("Departament"+s.Name );
                Console.WriteLine(s.Counting);
                //Console.WriteLine("Min" + s.Minimum);
                Console.WriteLine("Max" + s.Maximum);
                //Console.WriteLine("Avg" + s.Average);
            }
        }
        private static void GroupJoin()
        {
            Console.WriteLine("Group Joins");
            Console.WriteLine("");
            var query = peoples.GroupJoin(pets,
                    person => person.FirstName,
                    pet => pet.Owner,
                    (person, petCollection) => new
                    {
                        person.FirstName,
                        Pets= petCollection.Select(pet=> pet.Name)
                    });
            foreach (var owner in query)
            {
                Console.WriteLine($" Person : {owner.FirstName}");
                foreach (var p  in owner.Pets) 
                {
                    Console.WriteLine("Owns" + p);
                }
            }
        }

        private static void NumberOfPetsPerOwner()
        {
            var query = pets.GroupBy(p => p.Owner);

            foreach (var data in query)
            {
                Console.WriteLine($"\"{data.Key}\" is owned by {data.Count()}");
            }
        }
        private static void JoinKeyResults()
        {
            var query = peoples.Join(pets,
                    peaple => peaple.FirstName,
                    pet => pet.Owner,
                    (peaple, pet) => new
                    {
                        peaple.FirstName,
                        pet.Name
                    });
            //Taking just 2 values for test
            foreach (var data in query.Take(2))
            {
                Console.WriteLine($"\"{data.Name}\" owns {data.FirstName} pets");
            }
        }
        //The name of the Keys must be the same FirstName and Id
        private static void CompositeKeyResults()
        {
            Console.WriteLine("***************CompositeKey**********************");
            Console.WriteLine("");
            var query = peoples.Join(pets,
                    p => new { p.FirstName, p.Id },
                    pet => new { FirstName = pet.Owner, Id = pet.OwnerId },
                    (p, pet) => new
                    {
                        p.FirstName,
                        pet.Name
                    });

            foreach (var data in query)
            {
                Console.WriteLine($"\"{data.Name}\" is owned by {data.FirstName}");
            }
        }


        public class EmployeeStatistics 
        {
            public EmployeeStatistics()
            {
                int Max = Int32.MinValue;
                int Min = Int32.MaxValue;
            }
            
            public EmployeeStatistics Accumulate(Employee employee)
            {
                Total = Total + employee.Salary;
                Counting = Counting + 1;
                Max = Math.Max(Max, employee.Salary);
                //Min = Math.Min(Min, employee.Salary);
                return this;
            }

            public EmployeeStatistics Compute()
            {
                //Avg = Total / Count;
                return this;
            }
            public int Min { get; set; }
            public int Max { get; set; }
            public double Avg { get; set; }
            public int Total { get; set; }
            public int Counting { get; set; }
        }
        //JOIN ne Link do te bejme nje bashkim ndermjet cars dhe manufacturers
        //bashkimi behet ne Manufacturer dhe ne Name
        //manufacturers jane 50 kurse makinat jane rreth 200 cope
        //cars jane me shume 
        //Ne lidhje me performancen duhet qe te vendosim brenda kllapave ate qe ka me pak vlera 
        //Pra Inner Sequence duhet te kete me pak vlera se outer sequence

        /*var query2 = cars.Join(manufacturers,
                    c => c.Manufacturer,
                    m => m.Name, (c, m) => new
                    {
                        m.Headquarters,
                        c.Name,
                        c.Combined
                    });  
         */
    }
}
