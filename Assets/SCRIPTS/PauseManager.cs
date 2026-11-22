using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required if you reference Slider components directly

public class PauseManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject homeScreenPanel;
    public GameObject gameOverPanel;
    public GameObject settingsPanel;
    public GameObject statsPanel;

    [Header("Gameplay HUD to Hide in Settings")]
    public GameObject[] gameplayHudObjects;

    private bool isPaused = false;
    private bool openedFromHomeScreen = false;

    void Update()
    {
        // Prevent pausing while on the Home Screen or Game Over screen
        bool inMenus = (homeScreenPanel != null && homeScreenPanel.activeSelf) ||
                       (gameOverPanel != null && gameOverPanel.activeSelf);

        if (Input.GetKeyDown(KeyCode.Escape) && !inMenus)
        {
            if (isPaused)
            {
                // Close sub-panels first before resuming the whole game
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    CloseSettings();
                }
                else if (statsPanel != null && statsPanel.activeSelf)
                {
                    CloseStats();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        openedFromHomeScreen = false;
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freezes gameplay
    }

    public void ResumeGame()
    {
        isPaused = false;
        openedFromHomeScreen = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false); // Ensure settings closes too
        if (statsPanel != null) statsPanel.SetActive(false); // Ensure stats closes too
        SetGameplayHudActive(true);
        Time.timeScale = 1f; // Resumes gameplay
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            openedFromHomeScreen = (homeScreenPanel != null && homeScreenPanel.activeSelf);

            if (openedFromHomeScreen)
            {
                homeScreenPanel.SetActive(false);
            }
            else if (pausePanel != null)
            {
                pausePanel.SetActive(false); // Hide main pause buttons while in settings
            }

            if (statsPanel != null) statsPanel.SetActive(false);
            settingsPanel.SetActive(true);
            SetGameplayHudActive(false);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);

            if (openedFromHomeScreen)
            {
                if (homeScreenPanel != null) homeScreenPanel.SetActive(true);
                SetGameplayHudActive(false);
                openedFromHomeScreen = false;
            }
            else
            {
                if (pausePanel != null) pausePanel.SetActive(true); // Bring back main pause buttons
                SetGameplayHudActive(true);
            }
        }
    }

    public void OpenStats()
    {
        if (statsPanel != null)
        {
            openedFromHomeScreen = (homeScreenPanel != null && homeScreenPanel.activeSelf);

            if (openedFromHomeScreen)
            {
                homeScreenPanel.SetActive(false);
            }
            else if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }

            if (settingsPanel != null) settingsPanel.SetActive(false);
            statsPanel.SetActive(true);
            SetGameplayHudActive(false);
        }
    }

    public void CloseStats()
    {
        if (statsPanel != null)
        {
            statsPanel.SetActive(false);

            if (openedFromHomeScreen)
            {
                if (homeScreenPanel != null) homeScreenPanel.SetActive(true);
                SetGameplayHudActive(false);
                openedFromHomeScreen = false;
            }
            else
            {
                if (pausePanel != null) pausePanel.SetActive(true);
                SetGameplayHudActive(true);
            }
        }
    }

    public void SetVolume(float volume)
    {
        // This method can be hooked directly to your UI Slider's On Value Changed event
        AudioListener.volume = volume;
        Debug.Log("Volume set to: " + volume);
    }

    public void GoToHomeScreen()
    {
        Time.timeScale = 1f; // Always reset timeScale before reloading!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); // Shows in Console when testing in the Unity Editor
    }

    private void SetGameplayHudActive(bool active)
    {
        if (gameplayHudObjects == null) return;

        foreach (GameObject hudObj in gameplayHudObjects)
        {
            if (hudObj != null)
                hudObj.SetActive(active);
        }
    }
}