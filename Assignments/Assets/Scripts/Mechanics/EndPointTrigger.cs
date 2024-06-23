using UnityEngine;
using UnityEngine.SceneManagement;

public class EndpointTrigger : MonoBehaviour
{
    // Name of the credits scene
    public string creditsSceneName = "Credits";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the collider
        if (other.CompareTag("Player"))
        {
            // Load the credits scene
            SceneManager.LoadScene(creditsSceneName);
        }
    }
}
