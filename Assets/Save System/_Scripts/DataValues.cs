using System.Collections.Generic;

namespace Racer.SaveSystem
{
    /// <summary>
    /// Container that stores and manages the state of a save-file.
    /// </summary>
    internal class DataValues
    {
        public readonly Dictionary<string, object> KeyValues;

        public DataValues()
        {
            KeyValues = new Dictionary<string, object>();
        }

        public void AddValue<T>(string key, T value)
        {
            // If key already exists, overwrite the value
            if (ContainsKey(key))
                KeyValues[key] = value;

            // Add a value based off a key
            else
                KeyValues.Add(key, value);
        }

        public bool ContainsKey(string key) => KeyValues.ContainsKey(key);

        public T GetValue<T>(string key) => (T)KeyValues[key];
    }
}