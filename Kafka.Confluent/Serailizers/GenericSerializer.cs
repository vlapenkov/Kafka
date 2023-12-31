using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Serailizers
{
    public class GenericSerializer<T> : ISerializer<T>, IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull) return default(T);

            string str = null;
#if NETCOREAPP2_1
                    str= Encoding.UTF8.GetString(data);
#else
            str = Encoding.UTF8.GetString(data.ToArray());
#endif  
            //Encoding.UTF8.GetString()
            return JsonConvert.DeserializeObject<T>(str);

        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            string res = JsonConvert.SerializeObject(data, Formatting.None);

            return Encoding.UTF8.GetBytes(res);
            // throw new NotImplementedException();
        }
    }
}
