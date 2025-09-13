using Isas_Pizza.Models;

namespace Isas_Pizza.IO {
    public class ConsoleFacade: IIOFacade
    {
        private readonly CredentialPrompter credentialPrompter;
        private readonly ProductoIO productoIO;
        private readonly PrimitiveIO primitiveIO;
        private readonly IngredienteIO ingredienteIO;
        private readonly MenuGenericoIO menuGenericoIO;
        private readonly OrdenIO ordenIO;

        public ConsoleFacade(Pizzeria pizzeria)
        {
            credentialPrompter = new();
            primitiveIO = new();
            ingredienteIO = new(pizzeria.ingredientes.View(null));
            menuGenericoIO = new();
            productoIO = new(
                pizzeria.ingredientes.View(null),
                primitiveIO,
                menuGenericoIO
            );
            ordenIO = new(
                menuGenericoIO,
                () => pizzeria.menu.View(null)
            );
        }
        public LoginCredentials? Ask(LoginCredentials? _)
            => credentialPrompter.Ask(null);
        public Orden Ask(Orden? _)
            => ordenIO.Ask(null);

        public IngredienteEnStock Ask(Ingrediente ingrediente)
            => ingredienteIO.Ask(ingrediente);

        public IngredienteEnStock Ask(IngredienteEnStock ingrediente)
            => ingredienteIO.Ask(ingrediente);

        void IBlockingDisplayer<string>.Display(ICollection<string> elements)
            => primitiveIO.Display(elements);

        void IBlockingDisplayer<Orden>.Display(ICollection<Orden> elements)
            => ordenIO.Display(elements);

        void IBlockingDisplayer<IngredienteEnStock>.Display(ICollection<IngredienteEnStock> elements)
            => ingredienteIO.Display(elements);

        void IBlockingDisplayer<Producto>.Display(ICollection<Producto> elements)
            => productoIO.Display(elements);
        public Producto Ask(Producto? _)
            => productoIO.Ask((Producto?) null);

        public T SelectOne<T>(string title, ICollection<(string label, T option)> options)
            => menuGenericoIO.SelectOne<T>(title, options);
        public T SelectOne<T>(ICollection<(string label, T option)> options)
            => menuGenericoIO.SelectOne<T>(options);

    }
}