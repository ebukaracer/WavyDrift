#if UNITY_EDITOR
namespace Racer.SaveManager
{
    internal static class SaveUtility
    {
        private static string[] _inputValues;

        public static void CheckValue(string keyField)
        {
            if (!GetValidKey(keyField, out _inputValues))
                return;

            foreach (var v in _inputValues)
            {
                if (string.IsNullOrWhiteSpace(v))
                    continue;

                var newValue = v.Trim(' ');

                Logging.Log($"{newValue} | {SaveManager.Contains(newValue)}");
            }
        }

        public static void EraseValue(string keyField)
        {
            if (!GetValidKey(keyField, out _inputValues))
                return;

            _inputValues = keyField.Split(',');

            foreach (var v in _inputValues)
            {
                if (string.IsNullOrWhiteSpace(v))
                    continue;

                var newValue = v.Trim(' ');

                if (!SaveManager.Contains(newValue))
                    continue;

                SaveManager.ClearPref(newValue);

                Logging.Log(!SaveManager.Contains(newValue)
                    ? $"Successfully Cleared | {newValue}"
                    : $"Couldn't Clear | {newValue}");
            }
        }

        public static void EraseAllValues()
        {
            SaveManager.ClearAllPrefs();

            Logging.Log("Success");
        }

        private static bool GetValidKey(string keyField, out string[] inputs)
        {
            if (string.IsNullOrWhiteSpace(keyField))
            {
                Logging.LogWarning($"{nameof(keyField)} cannot be Empty!");

                inputs = null;

                return false;
            }

            inputs = keyField.Split(',');

            return true;
        }
    }
}
#endif