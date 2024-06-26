using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AutoTrader.Application.Extensions
{
    public static class ObjectExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");
            }

            var serializedObject = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}
