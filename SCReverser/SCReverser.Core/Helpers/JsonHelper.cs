using Newtonsoft.Json;
using System;

namespace SCReverser.Core.Helpers
{
    public class JsonHelper
    {
        static JsonSerializerSettings SettingsTypesNull = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        static JsonSerializerSettings SettingsTypesNotNull = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        static JsonSerializerSettings SettingsNoTypesNotNull = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="obj">Objet</param>
        public static string Serialize(object obj) { return Serialize(obj, false, false); }
        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="obj">Objet</param>
        /// <param name="indent">True for indent</param>
        /// <param name="serializeNull">True for serialize null properties</param>
        public static string Serialize(object obj, bool indent, bool serializeNull)
        {
            return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None, serializeNull ? SettingsTypesNull : SettingsTypesNotNull);
        }
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="json">Json</param>
        /// <param name="tp">Type</param>
        /// <param name="withTypes">With types</param>
        public static object Deserialize(string json, Type tp, bool withTypes = false)
        {
            if (!string.IsNullOrEmpty(json))
                try
                {
                    if (!withTypes)
                        return JsonConvert.DeserializeObject(json, tp, SettingsNoTypesNotNull);
                    else
                        return JsonConvert.DeserializeObject(json, tp, SettingsTypesNull);
                }
                catch { }

            return null;
        }
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="json">Json</param>
        /// <param name="withTypes">With types</param>
        public static T Deserialize<T>(string json, bool withTypes = false)
        {
            return (T)Deserialize(json, typeof(T), withTypes);
        }
        /// <summary>
        /// Clone object
        /// </summary>
        /// <param name="obj">Object for clon</param>
        public static object Clone(object obj)
        {
            if (obj == null) return null;

            string json = Serialize(obj);
            return Deserialize(json, obj.GetType());
        }
    }
}