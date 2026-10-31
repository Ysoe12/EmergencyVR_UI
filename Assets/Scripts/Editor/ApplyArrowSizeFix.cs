using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

public class ApplyArrowSizeFix
{
    [MenuItem("EmergencyVR/Fix Arrow Size (Option A)")]
    public static void Fix()
    {
        string scenePath = "Assets/Scenes/Evaluacion.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        GameObject canvasMenu = GameObject.Find("CanvasMenu");
        if (canvasMenu == null) return;

        Transform panelTrans = canvasMenu.transform.Find("Panel") ?? canvasMenu.transform.GetChild(0);
        if (panelTrans == null) return;

        Transform libreBtnTrans = panelTrans.Find("Libre");
        if (libreBtnTrans == null) return;

        TextMeshProUGUI tmpText = libreBtnTrans.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpText != null)
        {
            tmpText.text = "<size=150%>←</size> ATRAS";
            Debug.Log("Updated button text with Rich Text arrow size tag.");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }
}
