using System;
using System.Collections.Generic;
using Eiram;

namespace Tags
{
    public class Tag
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public bool HasKey(string tag)
        {
            return dictionary.ContainsKey(tag);
        }

        public bool RemoveTag(string tag)
        {
            return dictionary.Remove(tag);
        }

        public void AddBool(string key, bool value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }

            dictionary.Add(key, value);
        }

        public bool GetBool(string tag)
        {
            var contains = dictionary.TryGetValue(tag, out object value);
            return contains ? Convert.ToBoolean(value) : throw new NotFoundException(tag); 
        }

        public void AddString(string key, string value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }

            dictionary.Add(key, value);
        }

        public String GetString(string tag)
        {
            var contains = dictionary.TryGetValue(tag, out object value);
            return contains ? Convert.ToString(value) : throw new NotFoundException(tag); 
        }

        public void AddInt(string key, int value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }

            dictionary.Add(key, value);
        }

        public int GetInt(string tag)
        {
            var contains = dictionary.TryGetValue(tag, out object value);
            return contains ? Convert.ToInt32(value) : throw new NotFoundException(tag); 
        }

        public int Count => dictionary.Count;
        public bool HasTags => Count > 0;
        public bool HasNoTags => !HasTags;
    }
}