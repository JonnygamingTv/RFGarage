using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Discord
{
    public static class Utils
    {
        /// <summary>
        /// Convert Color object into hex integer
        /// </summary>
        /// <param name="color">Color to be converted</param>
        /// <returns>Converted hex integer</returns>
        public static int ColorToHex(Color color)
        {
            var hs =
                color.R.ToString("X2") +
                color.G.ToString("X2") +
                color.B.ToString("X2");

            return int.Parse(hs, System.Globalization.NumberStyles.HexNumber);
        }

        internal static JObject StructToJson(object @struct)
        {
            var type = @struct.GetType();
            var json = new JObject();

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var name = FieldNameToJsonName(field.Name);
                var value = field.GetValue(@struct);
                if (value == null)
                    continue;

                if (value is bool b)
                    json.Add(name, b);
                else if (value is int i)
                    json.Add(name, i);
                else if (value is Color color)
                    json.Add(name, ColorToHex(color));
                else if (value is string s)
                    json.Add(name, s);
                else if (value is DateTime time)
                    json.Add(name, time.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"));
                else if (value is IList list && list.GetType().IsGenericType)
                {
                    var array = new JArray();
                    foreach (var obj in list)
                        array.Add(StructToJson(obj));
                    json.Add(name, array);
                }
                else json.Add(name, StructToJson(value));
            }
            return json;
        }

        private static readonly string[] Ignore = { "InLine" };

        private static string FieldNameToJsonName(string name)
        {
            if (Ignore.ToList().Contains(name))
                return name.ToLower();

            var result = new List<char>();

            if (IsFullUpper(name))
                result.AddRange(name.ToLower().ToCharArray());
            else
                for (var i = 0; i < name.Length; i++)
                {
                    if (i > 0 && char.IsUpper(name[i]))
                        result.AddRange(new[] { '_', char.ToLower(name[i]) });
                    else result.Add(char.ToLower(name[i]));
                }
            return string.Join("", result);
        }

        private static bool IsFullUpper(string str)
        {
            return str.All(char.IsUpper);
        }

        public static string Decode(Stream source)
        {
            using var reader = new StreamReader(source);
            return reader.ReadToEnd();
        }

        public static byte[] Encode(string source, string encoding = "utf-8")
            => Encoding.GetEncoding(encoding).GetBytes(source);
    }
}
