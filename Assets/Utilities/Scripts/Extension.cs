using System.Collections;
using UnityEngine;

public static class Extension
{
    public static void DestroyChildren(this Transform parent)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);
    }


    public static IEnumerator Shake(this Transform myTransform, float duration, float magnitude)
    {
        Vector3 originalPosition = myTransform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            myTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        myTransform.localPosition = originalPosition;
    }

    public static void ToggleActive(this GameObject gameObject, bool state)
    {
        if (state)
        {
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

        }
        else
        {
            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
        }
    }

    public static void IsEnabled(this Behaviour behaviour, bool state)
    {
        if (state)
        {
            if (!behaviour.enabled)
                behaviour.enabled = true;
        }
        else
        {
            if (behaviour.enabled)
                behaviour.enabled = false;
        }
    }
}