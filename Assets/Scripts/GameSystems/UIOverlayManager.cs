using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOverlayManager : MonoBehaviour
{
    public static UIOverlayManager Instance { get; private set; }

    // Track which UI scenes are loaded
    private HashSet<string> loadedUIScenes = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowOverlay(string sceneName)
    {
        if (!loadedUIScenes.Contains(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            loadedUIScenes.Add(sceneName);
        }
    }

    public void HideOverlay(string sceneName)
    {
        if (loadedUIScenes.Contains(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
            loadedUIScenes.Remove(sceneName);
        }
    }

    public void ToggleOverlay(string sceneName)
    {
        if (loadedUIScenes.Contains(sceneName))
        {
            HideOverlay(sceneName);
        }
        else
        {
            ShowOverlay(sceneName);
        }
    }

    public void HideAllOverlays()
    {
        foreach (var scene in new List<string>(loadedUIScenes))
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        loadedUIScenes.Clear();
    }
}
