using System.Reflection.Metadata;

namespace Isas_Pizza
{
    public interface IRequestHandler<T>
    {
        public void Handle(T request);
        public void SetNext(IRequestHandler<T> handler);
    }
}