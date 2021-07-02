using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionPrinciple
{
    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle()
        {

        }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }        
    }

    public class Square : Rectangle
    {
        public override int Width 
        { get => base.Width; set => base.Width = base.Height = value; }

        public override int Height 
        { get => base.Height; set => base.Height = base.Width = value; }
    }
    class Program
    {
        static public int Area(Rectangle r) => r.Width * r.Height;

        static void Main(string[] args)
        {
            _ = new Rectangle(5, 5);

            Square sq = new Square
            {
                Width = 4
            };

            Console.WriteLine($"{sq} - Area: {Area(sq)}"); 
        }
    }
}
