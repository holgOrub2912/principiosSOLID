using Isas_Pizza.Models;

namespace Isas_Pizza {
    public interface ICounterVisitor<TReturn, TVisit>
    {
        public TReturn Visit(TVisit obj) => Visit(obj, 1);
        public TReturn Visit(TVisit obj, double cantidad);
    }
}