using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace firstWeb.Domain.Options.JsonOptions
{
    internal class NullToEmptyProvider : IValueProvider
    {
        private PropertyInfo t;

        public NullToEmptyProvider(PropertyInfo t)
        {
            this.t = t;
        }

        public object GetValue(object target)
        {
            var result = t.GetValue(target);
            if (t.PropertyType == typeof(string) && result == null)
            {
                result = string.Empty;
            }

            return result;
        }

        public void SetValue(object target, object value)
        {
            t.SetValue(target, value);
        }
    }
}