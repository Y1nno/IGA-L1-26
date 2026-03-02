using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager _instance;
    public static GlobalGameManager Instance => _instance;
    public string currentLevelName;
    public GameObject currentPauseMenuInstance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            enabled = false;
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadLevel(string levelName)
    {
        // Implement level loading logic here
        Debug.Log($"Loading level: {levelName}");
        currentLevelName = levelName;
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        // Implement level restarting logic here
        Debug.Log("Restarting level...");
        LoadLevel(currentLevelName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void TogglePauseMenu()
    {
        if (currentPauseMenuInstance == null)
        {
            Debug.LogWarning("Pause menu prefab is not assigned!");
        }

        if(Time.timeScale != 0f)
        {
            Debug.Log("Bringing up pause menu...");
            currentPauseMenuInstance.SetActive(true);
            TogglePauseGame(true);
        }
        else
        {
            currentPauseMenuInstance.SetActive(false);
            TogglePauseGame(false);
        }
    }



    public void TogglePauseGame(bool isPaused = true)
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}