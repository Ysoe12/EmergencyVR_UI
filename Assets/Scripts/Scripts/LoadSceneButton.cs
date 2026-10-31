using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName = "Menu";

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
