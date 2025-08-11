using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isas_Pizza.IO
{
    public class GenericBlockingPrompter : IBlockingPrompter
    {
        private readonly Dictionary<Type, ICollection<object>> _itemsByType;

        public GenericBlockingPrompter(params (Type type, ICollection<object> items)[] itemCollections)
        {
            _itemsByType = new Dictionary<Type, ICollection<object>>();
            foreach (var (type, items) in itemCollections)
            {
                _itemsByType[type] = items;
            }
        }

        public T Ask<T>()
        {
            if (!_itemsByType.TryGetValue(typeof(T), out var items))
            {
                throw new InvalidOperationException($"No hay elementos registrados para el tipo {typeof(T).Name}");
            }

            Console.WriteLine($"\n=== Seleccione un {typeof(T).Name} ===");

            int index = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"{index}. {item}");
                index++;
            }

            int selectedIndex;
            while (true)
            {
                Console.Write("Ingrese el número de opción: ");
                if (int.TryParse(Console.ReadLine(), out selectedIndex) &&
                    selectedIndex >= 1 &&
                    selectedIndex <= items.Count)
                {
                    break;
                }
                Console.WriteLine("¡Opción inválida! Intente nuevamente.");
            }

            return (T)items.ElementAt(selectedIndex - 1);
        }
    }
}
