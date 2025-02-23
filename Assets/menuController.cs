using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;  // Import TextMeshPro namespace

public class menuController : MonoBehaviour
{
    public Button _start;

    void Start()
    {
        // Add the method reference to the button listener
        _start.onClick.AddListener(LoadMain);
    }

    // Load the main scene
    private void LoadMain()
    {
        SceneManager.LoadScene("Jacob");
    }
}
