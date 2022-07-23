using System.Collections.Generic;

namespace Racer.SaveSystem
{
    /// <summary>
    /// Container that stores and manages the state of a save-file.
    /// </summary>
    class DataValues
    {
        public readonly Dictionary<string, object> _keyValues;

        public DataValues()
        {
            _keyValues = new Dictionary<string, object>();
        }

        public void AddValue<T>(string key, T value)
        {
            // If key already exists, overwrite the value
            if (ContainsKey(key))
                _keyValues[key] = value;

            // Add a value based off a key
            else
                _keyValues.Add(key, value);
        }

        public bool ContainsKey(string key) => _keyValues.ContainsKey(key);

        public bool ContainsValue<T>(T value) => _keyValues.ContainsValue(value);

        public T GetValue<T>(string key) => (T)_keyValues[key];
    }
}