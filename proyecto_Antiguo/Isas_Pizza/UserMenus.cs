using System;
using System.Collections.Generic;

namespace Isas_Pizza
{
    /// <summary>
    /// Clase estática que contine todos los menús de la aplicación.
    /// Un menú es una colección de tuplas de la forma (string,
    /// Action<Pizzeria>), de tal forma que sea posible pasárselo a un
    /// IBlockingSelector.
    /// </summary>
    public static class UserMenus
    {
        /// <summary>
        /// Menú principales de la aplicación por roles.
        /// </summary>
        private static Dictionary<UserRole, (string, Action<Pizzeria>)[]>
        mainMenu = new([
            new KeyValuePair<UserRole, (string, Action<Pizzeria>)[]>(
                UserRole.ADMINISTRADOR, [
                    ("Ver inventario", Pizzeria.VerInventario),
                    ("Añadir a Stock", Pizzeria.VerInventario),
                ]
            ),
            new KeyValuePair<UserRole, (string, Action<Pizzeria>)[]>(
                UserRole.CHEF, [
                    ("Ver órdenes", Pizzeria.VerInventario),
                    ("Cocinar orden", Pizzeria.VerInventario),
                ]
            ),
            new KeyValuePair<UserRole, (string, Action<Pizzeria>)[]>(
                UserRole.CONSUMIDOR, [
                    ("Ordenar", Pizzeria.VerInventario),
                    ("Recibir orden", Pizzeria.VerInventario),
                ]
            ),
        ]);
    }
}