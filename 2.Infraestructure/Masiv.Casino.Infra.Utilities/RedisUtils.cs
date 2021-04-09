using StackExchange.Redis;
using System.Collections.Generic;

namespace Masiv.Casino.Infra.Utilities
{
    public static class RedisUtils
    {
        /// <summary>
        /// Example method for converting instance into hashentry list with reflection
        /// Use library (like FastMember) for this kind of mapping
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static List<HashEntry> ConvertToHashEntryList(this object instance)
        {
            var propertiesInHashEntryList = new List<HashEntry>();
            foreach (var property in instance.GetType().GetProperties())
            {
                if (!property.Name.Equals("ObjectAdress"))
                {
                    // This is just for an example
                    propertiesInHashEntryList.Add(new HashEntry(property.Name, instance.GetType().GetProperty(property.Name).GetValue(instance).ToString()));
                }
                else
                {
                    var subPropertyList = ConvertToHashEntryList(instance.GetType().GetProperty(property.Name).GetValue(instance));
                    propertiesInHashEntryList.AddRange(subPropertyList);
                }
            }
            return propertiesInHashEntryList;
        }
    }
}