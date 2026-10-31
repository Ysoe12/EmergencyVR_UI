using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConfigureCopiedButton
{
    [MenuItem("EmergencyVR/Configure Copied Libre Button")]
    public static void Configure()
    {
        string scenePath = "Assets/Scenes/Evaluacion.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        GameObject canvasMenu = GameObject.Find("CanvasMenu");
        if (canvasMenu == null)
        {
            Debug.LogError("CanvasMenu not found!");
            return;
        }

        Transform panelTrans = canvasMenu.transform.Find("Panel") ?? canvasMenu.transform.GetChild(0);
        if (panelTrans == null)
        {
            Debug.LogError("Panel not found!");
            return;
        }

        // Find the copied button named "Libre"
        Transform libreBtnTrans = panelTrans.Find("Libre");
        if (libreBtnTrans == null)
        {
            Debug.LogError("Could not find 'Libre' button in Evaluacion panel!");
            return;
        }

        // 1. Change text content to "← ATRAS"
        TextMeshProUGUI tmpText = libreBtnTrans.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpText != null)
        {
            tmpText.text = "← ATRAS";
            Debug.Log("Updated button text to '← ATRAS'.");
        }
        else
        {
            // Fallback to legacy UI.Text if TMP not found
            Text legacyText = libreBtnTrans.GetComponentInChildren<Text>(true);
            if (legacyText != null)
            {
                legacyText.text = "← ATRAS";
                Debug.Log("Updated legacy button text to '← ATRAS'.");
            }
        }

        // 2. Remove or deactivate "Icono" object
        Transform iconTrans = libreBtnTrans.Find("Icono") ?? FindDeep(libreBtnTrans, "Icono") ?? FindDeep(libreBtnTrans, "Icon");
        if (iconTrans != null)
        {
            Object.DestroyImmediate(iconTrans.gameObject);
            Debug.Log("Removed 'Icono' object from button.");
        }

        // 3. Configure LoadSceneButton component to load "Menu" scene
        LoadSceneButton loadScript = libreBtnTrans.GetComponent<LoadSceneButton>() ?? libreBtnTrans.gameObject.AddComponent<LoadSceneButton>();
        loadScript.sceneName = "Menu";

        Button btnComponent = libreBtnTrans.GetComponent<Button>();
        if (btnComponent != null)
        {
            btnComponent.onClick.RemoveAllListeners();
            #if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btnComponent.onClick, loadScript.LoadTargetScene);
            #endif
            Debug.Log("Configured Button onClick event to load 'Menu' scene.");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Configured copied Libre button successfully!");
    }

    private static Transform FindDeep(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name.ToLower().Contains(name.ToLower())) return child;
            Transform found = FindDeep(child, name);
            if (found != null) return found;
        }
        return null;
    }
}
