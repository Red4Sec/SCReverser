using Newtonsoft.Json;
using System;

namespace SCReverser.Core.Helpers
{
    public class JsonHelper
    {
        //static JavaScriptSerializer JSON = new JavaScriptSerializer();
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
            return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = serializeNull ? NullValueHandling.Include : NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,

                //PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }
                );
        }
        /// <summary>
        /// Obtiene el objeto
        /// </summary>
        /// <param name="json">Json</param>
        public static object Deserialize(string json)
        {
            if (!string.IsNullOrEmpty(json))
                try { return JsonConvert.DeserializeObject(json); }
                catch { }

            return null;
        }
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="json">Json</param>
        /// <param name="tp">Type</param>
        public static object Deserialize(string json, Type tp)
        {
            if (!string.IsNullOrEmpty(json))
                try { return JsonConvert.DeserializeObject(json, tp); }
                catch { }

            return null;
        }
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="json">Json</param>
        public static T Deserialize<T>(string json)
        {
            if (!string.IsNullOrEmpty(json))
                try { return JsonConvert.DeserializeObject<T>(json); }
                catch { }

            return default(T);
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