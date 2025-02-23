using UnityEngine;
using TMPro; // Import TextMesh Pro namespace

public class title : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Reference to the TextMesh Pro UI element
    private float countdownTime = 5f; // Starting countdown time
    public bool isCountingDown = true;

    // This method is called when the object becomes active
    private void OnEnable()
    {
        ResetCountdown();
    }

    void Update()
    {
        if (isCountingDown)
        {
            countdownTime -= Time.deltaTime; // Decrease countdown time each frame
            countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Update the TextMesh Pro text with the current countdown time

            if (countdownTime <= 0)
            {
                isCountingDown = false;
                countdownTime = 5f; // Reset countdown time
            }
        }
    }

    // Method to reset the countdown
    private void ResetCountdown()
    {
        isCountingDown = true;
        countdownTime = 5f; // Reset countdown time
    }
}
