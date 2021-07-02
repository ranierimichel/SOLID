using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple
{
    public enum Relationship
    {
        Parent,
        Child,
        Sibling,
        Brother
    }

    public class Person
    {
        public string Name;
        //public DateTime DateOfBirth;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
        IEnumerable<Person> FindAllBrothersOf(string name);
    }

    // Low-lvl part of the system
    public class Relationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations
          = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }  
        public void AddParentAndBrother(Person parent, Person brother)
        {
            relations.Add((parent, Relationship.Brother, brother));
            relations.Add((brother, Relationship.Brother, parent));
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations.Where(
                x => x.Item1.Name == name &&
                x.Item2 == Relationship.Parent
            ).Select(r => r.Item3);

            //foreach (var r in relations.Where(
            //   x => x.Item1.Name == name &&
            //        x.Item2 == Relationship.Parent))
            //{
            //    yield return r.Item3;
            //}
        }

        public IEnumerable<Person> FindAllBrothersOf(string name)
        {
            return relations.Where(
                x => x.Item1.Name == name &&
                x.Item2 == Relationship.Brother
                ).Select(r => r.Item3);
        }

        public override string ToString()
        {
            string result = $"Complete list: {Environment.NewLine}";
            foreach (var i in relations)
            {
                result += $" {i.Item1.Name, -10} {i.Item2, -10} {i.Item3.Name} {Environment.NewLine}";
            }
            return result;
        }
        //public List<(Person, Relationship, Person)> Relations => relations;
        
    }

    class Research
    {
        // High-lvl part of the system
        public Research(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("Ranieri"))
            {
                Console.WriteLine($"Ranieri has a child called {p.Name}");
            }
            
            foreach (var p in browser.FindAllChildrenOf("Joana"))
            {
                Console.WriteLine($"Joana has a child called {p.Name}");
            }  
            
            foreach (var p in browser.FindAllBrothersOf("Ranieri"))
            {
                Console.WriteLine($"Ranieri has a brother called {p.Name}");
            }
        }

        //public Research(Relationships relationships)
        //{
        //    var relations = relationships.Relations;
        //    foreach (var r in relations.Where(
        //        x => x.Item1.Name == "Ranieri" &&
        //             x.Item2 == Relationship.Parent))
        //    {
        //        Console.WriteLine($"Ranieri has a child called {r.Item3.Name}");
        //    }
        //}


        static void Main(string[] args)
        {
            var parent = new Person { Name = "Ranieri" };
            var child = new Person { Name = "Antonia" };
            var brother1 = new Person { Name = "Pier" };
            var brother2 = new Person { Name = "Wendel" };
            var child1 = new Person { Name = "Ranieri" };
            var parent1 = new Person { Name = "Joana" };

            var relationships = new Relationships();
            relationships.AddParentAndBrother(parent, brother1);
            relationships.AddParentAndBrother(parent, brother2);
            relationships.AddParentAndChild(parent, child);
            relationships.AddParentAndChild(parent1, child1);
            Console.WriteLine($"{relationships}");

            new Research(relationships);
            

        }
    }
}
