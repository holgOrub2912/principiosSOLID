using System;
using System.Diagnostics.Contracts;

namespace Isas_Pizza
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MenuOptionAttribute : Attribute
    {
        public string label;

        public MenuOptionAttribute(string label)
        {
            this.label = label;
        }
    }
}