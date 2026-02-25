using UnityEngine;

public class Utils : MonoBehaviour
{
    public static GameObject TryGetObject(string name)
    {
        GameObject chimeObj = GameObject.Find(name);

        if (chimeObj == null)
        {
            Debug.Log($"No object with the name '{name}' found.");
            return null;
        }

        return chimeObj;
    }

    public static T TryGetComponent<T>(string objectName)
    {
        GameObject obj = TryGetObject(objectName);

        if (obj == null) return default;

        T component = obj.GetComponent<T>();

        if (component == null)
        {
            Debug.Log("Chime object doesn't have component attached");
            return default;
        }

        return component;
    }
}