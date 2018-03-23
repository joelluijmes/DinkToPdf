using System;

namespace DinkToPdf
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WkHtmlAttribute : Attribute
    {
        public WkHtmlAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}