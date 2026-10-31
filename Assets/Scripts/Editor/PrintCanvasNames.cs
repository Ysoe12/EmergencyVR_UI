using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

public class PrintCanvasNames
{
    [MenuItem("EmergencyVR/Print Canvas Names")]
    public static void Print()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Evaluacion.unity");
        GameObject canvas = GameObject.Find("CanvasMenu");
        if (canvas == null)
        {
            Debug.LogError("CanvasMenu not found");
            return;
        }

        foreach (Transform t in canvas.GetComponentsInChildren<Transform>(true))
        {
            TextMeshProUGUI tmp = t.GetComponent<TextMeshProUGUI>();
            string textVal = tmp != null ? $" [Text: '{tmp.text}']" : "";
            Debug.Log($"Obj: {t.name}{textVal}");
        }
    }
}
