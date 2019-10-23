using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Options.JsonOptions
{
    public class NullToEmptyResolver:DefaultContractResolver
    {
        /// <summary>
        /// 将属性值为null的修改为"",也就是Empty
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                .Select(t =>
                {
                    var property = CreateProperty(t, memberSerialization);
                    property.ValueProvider = new NullToEmptyProvider(t);
                    return property;
                }).ToList();
        }

        /// <summary>
        /// 使属性名小写
        /// </summary>
        /// <param name="propertyName">属性名字</param>
        /// <returns></returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }

    }
}
