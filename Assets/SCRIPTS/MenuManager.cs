using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject homeScreenPanel;

    void Start()
    {
        // Show the home screen and pause the game when it first loads
        if (homeScreenPanel != null)
        {
            homeScreenPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void StartGame()
    {
        // Hide the home screen and unpause the game
        if (homeScreenPanel != null)
        {
            homeScreenPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Unpause and reload the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}