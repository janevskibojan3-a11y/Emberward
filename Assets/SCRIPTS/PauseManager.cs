using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject homeScreenPanel;
    public GameObject gameOverPanel;

    private bool isPaused = false;

    void Update()
    {
        // Prevent pausing while on the Home Screen or Game Over screen
        bool inMenus = (homeScreenPanel != null && homeScreenPanel.activeSelf) ||
                       (gameOverPanel != null && gameOverPanel.activeSelf);

        if (Input.GetKeyDown(KeyCode.Escape) && !inMenus)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freezes gameplay
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Resumes gameplay
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
}