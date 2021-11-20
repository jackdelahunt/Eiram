using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eiram;

namespace Tags
{
    [Serializable]
    public class Tag : ICloneable
    {
        protected Dictionary<string, object> dictionary = new Dictionary<string, object>();
        
        public Tag() {}
        

        public Tag(params ValueTuple<string, object>[] keyValues)
        {
            foreach (var keyValue in keyValues)
            {
                dictionary.Add(keyValue.Item1, keyValue.Item2);
            }
        }

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

        public int GetInt(string key)
        {
            var contains = dictionary.TryGetValue(key, out object value);
            return contains ? Convert.ToInt32(value) : throw new NotFoundException(key); 
        }
        
        public void SetInt(string key, int newValue)
        {
            dictionary.Remove(key);
            dictionary.Add(key, newValue);
        }
        
        public object Clone()
        {
            return new Tag
            {
                dictionary = new Dictionary<string, object>(dictionary)
            };
        }

        public int Count => dictionary.Count;
        public bool HasTags => Count > 0;
        public bool HasNoTags => !HasTags;
    }
}