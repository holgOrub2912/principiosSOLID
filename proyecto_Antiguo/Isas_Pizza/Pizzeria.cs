using System;
﻿using Isas_Pizza;
using Isas_Pizza.IO;
using Isas_Pizza.Persistence;

class Pizzeria
{
    IROPersistenceLayer<Producto> menu;
    IPersistenceLayer<IngredienteEnStock> inventario;
    IPersistenceLayer<Orden> ordenes;

    IUserAgent? usuarioActivo = null;
    IAuthenticator auth = new GenericAuthenticator(new AuthPersistenceLayer("userdb.txt"), new PrimitiveIO());

    /// <summary>
    /// Inicializar capa de persistencia e interfaz.
    /// </summary>
    public Pizzeria()
    {
    
    }

    /// <summary>
    /// Encapsula toda la lógica de alto nivel del negocio.
    /// </summary>
    /// <details>
    /// Flujo de programa:
    /// 1. Iniciar sesión, guardar en usuarioActivo.
    /// 2. Presentar el Menú principal para este usuario.
    /// 3. Si la opción seleccionada es Cerrar Sesión, eliminar
    ///    usuarioActivo y volver a (1).
    /// 4. Si no, ejecutar la opción seleccionada normalmente.
    /// 5. Volver a (2).
    /// </details>
    public void Enter()
    {
        if (this.usuarioActivo is null)
        {
            this.usuarioActivo = auth.Authenticate(new CredentialPrompter());
        }
        Console.WriteLine(this.usuarioActivo.GetRole());
    }

    public static void Main(string[] args)
    {
        Pizzeria pizzeria = new Pizzeria();
        pizzeria.Enter();
    }

    internal static void VerInventario(Pizzeria pizzeria)
    {
        throw new NotImplementedException();
    }
}
