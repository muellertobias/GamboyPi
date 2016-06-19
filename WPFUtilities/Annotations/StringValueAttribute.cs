using System;
using System.Reflection;

namespace WPFUtilities.Annotations
{
    public class StringValueAttribute : Attribute
    {
        private string value;

        public StringValueAttribute(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }

        public static string GetStringValue(Enum value)
        {
            string result = string.Empty;
            Type type = value.GetType();

            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            StringValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                throw new NotImplementedException(String.Format("StringValue for Enum {0} is not implemented!", value.ToString()));
            }
        }
    }
}
