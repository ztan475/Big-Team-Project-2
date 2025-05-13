using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleTrigger : MonoBehaviour
{
    private Collectible parentScript;

    private void Start()
    {
        parentScript = GetComponentInParent<Collectible>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered pickup range");
            parentScript.PlayerEntered();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited pickup range");
            parentScript.PlayerExited();
        }
    }
}
