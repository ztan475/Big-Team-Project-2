using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;
    public void StartGame()
    {
        // Loads the gameplay scene
        SceneManager.LoadScene("Scene1"); // Make sure the name matches exactly
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        Debug.Log("Settings closed");
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit(); // Will only work in built application, not in editor
    }
}
