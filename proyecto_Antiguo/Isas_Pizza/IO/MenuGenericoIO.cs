using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class MenuGenericoIO: IBlockingSelector
    {
        public T SelectOne<T>(string title, ICollection<(string label, T option)> options)
    {
        if (options == null || options.Count == 0)
            throw new ArgumentException("No hay opciones para seleccionar");

        Console.WriteLine($"\n=== {title} ===");

        int index = 1;
        foreach (var (label, _) in options)
        {
            Console.WriteLine($"{index}. {label}");
            index++;
        }

        int selectedIndex;
        while (true)
        {
            Console.Write("Ingrese el número de opción: ");
            if (int.TryParse(Console.ReadLine(), out selectedIndex) &&
                selectedIndex >= 1 &&
                selectedIndex <= options.Count)
            {
                break;
            }
            Console.WriteLine("¡Opción inválida! Intente nuevamente.");
        }

        return options.ElementAt(selectedIndex - 1).option;
    }

        public T SelectOne<T>(ICollection<(string label, T option)> options)
            => SelectOne("Seleccione una opción", options);
}
}
