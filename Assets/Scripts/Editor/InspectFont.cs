using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

public class InspectFont
{
    [MenuItem("EmergencyVR/Inspect Font")]
    public static void Run()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Evaluacion.unity");
        foreach (var tmp in GameObject.FindObjectsOfType<TextMeshProUGUI>(true))
        {
            if (tmp.font != null)
            {
                Debug.Log($"TMP Object: {tmp.gameObject.name}, Font: {tmp.font.name}, Size: {tmp.fontSize}");
            }
        }
    }
}
