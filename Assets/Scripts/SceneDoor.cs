using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour
{
    [SerializeField] private Scene nextScene;

    private void LoadScene()
    {
        // Convert enum to string (e.g., Scene.Scene2 -> "Scene2")
        string sceneName = nextScene.ToString();
        SceneManager.LoadScene(sceneName);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadScene();
        }
    }
}

public enum Scene
{
    Scene1, 
    Scene2, 
    Scene3, 
    Scene4
}
