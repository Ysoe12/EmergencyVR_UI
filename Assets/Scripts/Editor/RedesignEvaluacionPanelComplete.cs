using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class RedesignEvaluacionPanelComplete
{
    [MenuItem("EmergencyVR/Complete Evaluacion Panel Redesign")]
    public static void Redesign()
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
            Debug.LogError("Panel not found under CanvasMenu!");
            return;
        }

        // Ensure images in Assets/UI are imported as Sprites
        string[] imagePaths = {
            "Assets/UI/ImagenCalle.jpeg",
            "Assets/UI/ImagenCasa.jpeg",
            "Assets/UI/ImagenPsicina.jpeg"
        };

        Sprite[] sprites = new Sprite[3];
        for (int i = 0; i < 3; i++)
        {
            string assetPath = imagePaths[i];
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
            sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        }

        // Identify and keep the Logo object, delete everything else inside Panel except panel background image
        Transform logoTransform = null;
        for (int i = panelTrans.childCount - 1; i >= 0; i--)
        {
            Transform child = panelTrans.GetChild(i);
            if (child.name == "Logo")
            {
                logoTransform = child;
            }
            else
            {
                // Remove old buttons, titles, etc.
                Object.DestroyImmediate(child.gameObject);
            }
        }

        // Ensure Logo is at top-left
        if (logoTransform != null)
        {
            logoTransform.SetParent(panelTrans, false);
            RectTransform logoRt = logoTransform.GetComponent<RectTransform>();
            logoRt.anchorMin = new Vector2(0f, 1f);
            logoRt.anchorMax = new Vector2(0f, 1f);
            logoRt.pivot = new Vector2(0f, 1f);
            logoRt.anchoredPosition = new Vector2(15f, -15f);
        }

        // 1. Large centered title at top showing selected scenario ("CASA")
        GameObject titleObj = new GameObject("SelectedScenarioTitle", typeof(RectTransform), typeof(TextMeshProUGUI));
        titleObj.transform.SetParent(panelTrans, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -40f);
        titleRect.sizeDelta = new Vector2(400f, 50f);
        TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
        titleText.text = "CASA";
        titleText.fontSize = 32;
        titleText.fontStyle = FontStyles.Bold;
        titleText.color = new Color(0.2f, 0.85f, 1f); // Neon cyan
        titleText.alignment = TextAlignmentOptions.Center;

        // 2. Horizontal Layout Group container for 3 circular scenario items
        GameObject hlgObj = new GameObject("ScenarioCirclesContainer", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        hlgObj.transform.SetParent(panelTrans, false);
        RectTransform hlgRect = hlgObj.GetComponent<RectTransform>();
        hlgRect.anchorMin = new Vector2(0.5f, 0.5f);
        hlgRect.anchorMax = new Vector2(0.5f, 0.5f);
        hlgRect.pivot = new Vector2(0.5f, 0.5f);
        hlgRect.anchoredPosition = new Vector2(0f, -10f);
        hlgRect.sizeDelta = new Vector2(750f, 320f);

        HorizontalLayoutGroup hlg = hlgObj.GetComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 35f;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        string[] names = { "CALLE", "CASA", "PISCINA" };
        string[] descs = { "Área urbana y vial", "Vivienda unifamiliar", "Zona acuática" };
        bool[] highlights = { false, true, false }; // Center one is main

        for (int i = 0; i < 3; i++)
        {
            float size = highlights[i] ? 140f : 115f; // Center is larger

            // Vertical Layout for circle + texts
            GameObject itemObj = new GameObject($"ScenarioItem_{names[i]}", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(Button));
            itemObj.transform.SetParent(hlgObj.transform, false);
            RectTransform itemRect = itemObj.GetComponent<RectTransform>();
            itemRect.sizeDelta = new Vector2(180f, 280f);

            VerticalLayoutGroup vlg = itemObj.GetComponent<VerticalLayoutGroup>();
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.spacing = 8f;
            vlg.childControlWidth = false;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = false;
            vlg.childForceExpandHeight = false;

            // Circle Container
            GameObject circleContainer = new GameObject($"Circle_{names[i]}", typeof(RectTransform));
            circleContainer.transform.SetParent(itemObj.transform, false);
            RectTransform circleRect = circleContainer.GetComponent<RectTransform>();
            circleRect.sizeDelta = new Vector2(size, size);

            // Glow border around circle
            GameObject glowObj = new GameObject("GlowBorder", typeof(RectTransform), typeof(Image));
            glowObj.transform.SetParent(circleContainer.transform, false);
            RectTransform glowRect = glowObj.GetComponent<RectTransform>();
            glowRect.anchorMin = Vector2.zero; glowRect.anchorMax = Vector2.one;
            glowRect.sizeDelta = new Vector2(14f, 14f);
            glowRect.anchoredPosition = Vector2.zero;
            Image glowImg = glowObj.GetComponent<Image>();
            glowImg.color = highlights[i] ? new Color(0.2f, 0.85f, 1f, 0.9f) : new Color(0.6f, 0.6f, 0.6f, 0.6f);

            // Circular Mask Container
            GameObject maskObj = new GameObject("CircularMask", typeof(RectTransform), typeof(Image), typeof(Mask));
            maskObj.transform.SetParent(circleContainer.transform, false);
            RectTransform maskRect = maskObj.GetComponent<RectTransform>();
            maskRect.anchorMin = Vector2.zero; maskRect.anchorMax = Vector2.one;
            maskRect.sizeDelta = Vector2.zero;
            maskRect.anchoredPosition = Vector2.zero;
            Mask mask = maskObj.GetComponent<Mask>();
            mask.showMaskGraphic = false; // Don't render mask white box, only clip children

            Image maskImg = maskObj.GetComponent<Image>();
            maskImg.color = Color.white;

            // Image inside mask
            GameObject imgObj = new GameObject("ScenarioImage", typeof(RectTransform), typeof(Image));
            imgObj.transform.SetParent(maskObj.transform, false);
            RectTransform imgRect = imgObj.GetComponent<RectTransform>();
            imgRect.anchorMin = Vector2.zero; imgRect.anchorMax = Vector2.one;
            imgRect.sizeDelta = Vector2.zero;
            imgRect.anchoredPosition = Vector2.zero;
            Image scenarioImg = imgObj.GetComponent<Image>();
            scenarioImg.sprite = sprites[i];
            scenarioImg.preserveAspect = false;

            // Text: Scenario Name (Uppercase)
            GameObject nameObj = new GameObject("Text_Name", typeof(RectTransform), typeof(TextMeshProUGUI));
            nameObj.transform.SetParent(itemObj.transform, false);
            RectTransform nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.sizeDelta = new Vector2(180f, 30f);
            TextMeshProUGUI nameT = nameObj.GetComponent<TextMeshProUGUI>();
            nameT.text = names[i];
            nameT.fontSize = 18;
            nameT.fontStyle = FontStyles.Bold;
            nameT.color = Color.white;
            nameT.alignment = TextAlignmentOptions.Center;

            // Text: Description
            GameObject descObj = new GameObject("Text_Desc", typeof(RectTransform), typeof(TextMeshProUGUI));
            descObj.transform.SetParent(itemObj.transform, false);
            RectTransform descRect = descObj.GetComponent<RectTransform>();
            descRect.sizeDelta = new Vector2(180f, 35f);
            TextMeshProUGUI descT = descObj.GetComponent<TextMeshProUGUI>();
            descT.text = descs[i];
            descT.fontSize = 12;
            descT.color = new Color(0.8f, 0.8f, 0.8f);
            descT.alignment = TextAlignmentOptions.Center;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Evaluacion panel completely redesigned with circular image items from Y:\\Capturas!");
    }
}
