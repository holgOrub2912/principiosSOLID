
namespace Isas_Pizza.IO
{
    class PrimitiveIO : IBlockingDisplayer<string>, IBlockingPrompter<int>, IBlockingPrompter<double>
    {
        public void Display(ICollection<string> strings)
        {
            Console.WriteLine(string.Join("\n", strings));
        }

        public int Ask(int _)
        {
            int cantidad;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out cantidad) && cantidad > 0 )
                    break;
                Console.WriteLine("Favor escribir una cantidad entera positiva.");
            }
            return cantidad;
        }

        public double Ask(double _)
        {
            double cantidad;
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out cantidad) && cantidad > 0 )
                    break;
                Console.WriteLine("Favor escribir una cantidad positiva.");
            }
            return cantidad;
        }
    }
}