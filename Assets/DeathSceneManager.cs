using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;  // Import TextMeshPro namespace

public class DeathSceneManager : MonoBehaviour
{
    // UI elements
    public TMP_Text deathMessage;  // Change from Text to TMP_Text
    public Button restartButton;
    public Button menuButton;

    void Start()
    {
        // Set up button listeners
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(LoadMainMenu);
    }

    // Restart the "Jacob" scene
    private void RestartGame()
    {
        SceneManager.LoadScene("Jacob");  // Load the "Jacob" scene
    }

    // Load the main menu scene (make sure you have a scene called "MainMenu")
    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // Load the "MainMenu" scene
    }
}
