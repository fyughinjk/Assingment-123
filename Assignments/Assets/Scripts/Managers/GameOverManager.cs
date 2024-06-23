using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        if (restartButton)
            restartButton.onClick.AddListener(RestartGame);

        if (mainMenuButton)
            mainMenuButton.onClick.AddListener(MainMenu);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    void MainMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
