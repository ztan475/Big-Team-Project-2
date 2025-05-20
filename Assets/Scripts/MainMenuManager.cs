using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Loads the gameplay scene
        SceneManager.LoadScene("MoveTest"); // Make sure the name matches exactly
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button clicked");
        // You can show a settings panel here
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit(); // Will only work in built application, not in editor
    }
}
