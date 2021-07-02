using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenClosed
{
    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Huge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            if (name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }
            Name = name;
            Color = color;
            Size = size;
        }
    }

    #region BadFilter
    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
            {
                if (p.Size == size && p.Color == color)
                {
                    yield return p;
                }
            }
        }
    }
    #endregion

    public interface ISpecification<T>
    {
        bool isSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public bool isSatisfied(Product t)
        {
            return t.Color == color;
        }
    }  
    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool isSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool isSatisfied(T t)
        {
            return first.isSatisfied(t) && second.isSatisfied(t);
        }
    }
    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
            {
                if (spec.isSatisfied(i))
                {
                    yield return i;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Blue, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Huge);
            var ball = new Product("ball", Color.Blue, Size.Large);
            var car = new Product("car", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house, ball, car };

            var pf = new ProductFilter();
            Console.WriteLine("Small products Blue (old):");
            foreach (var p in pf.FilterBySizeAndColor(products, Size.Small, Color.Blue))
            {
                Console.WriteLine($"{p.Name} is {p.Size} and {p.Color}");
            }

            var bf = new BetterFilter();
            //Console.WriteLine("Small products Blue (new):");
            //foreach (var p in bf.Filter(products, new ColorSpecification(Color.Blue)))
            //{
            //    Console.WriteLine($"{p.Name} is {p.Size} and {p.Color}");
            //}

            Console.WriteLine($"Large blue items");
            foreach (var p in bf.Filter(products, new AndSpecification<Product>(
                    new ColorSpecification(Color.Blue),
                    new SizeSpecification(Size.Large)
                    )))
            {
                Console.WriteLine($"{p.Name} is {p.Size} and {p.Color}");
            }            
            Console.ReadKey();

        }
    }
}
