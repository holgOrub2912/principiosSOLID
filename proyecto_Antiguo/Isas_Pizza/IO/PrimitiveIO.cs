
namespace Isas_Pizza.IO
{
    class PrimitiveIO : IBlockingDisplayer<string>
    {
        public void Display(ICollection<string> strings)
        {
            Console.WriteLine(string.Join("\n", strings));
        }
    }
}