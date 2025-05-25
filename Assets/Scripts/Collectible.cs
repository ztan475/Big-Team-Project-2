using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    TypeA,
    TypeB,
}

public class Collectible : MonoBehaviour
{
    [Header("Popup Menu")]
    [SerializeField] private Canvas popupMenu;
    [SerializeField] private float popupTime;

    [SerializeField] private CollectibleType collectibleType;

    private PlayerInventory playerInventory;

    private bool playerInRange = false;
    private bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        popupMenu.enabled = false;
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // Collect if player in range and not collected
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Item collected");
            collected = true;
            playerInventory.AddCollectible(collectibleType);
            StartCoroutine(showPopup());
        }
    }

    private IEnumerator showPopup()
    {
        popupMenu.enabled = true;

        yield return new WaitForSeconds(popupTime);

        popupMenu.enabled = false;

        Destroy(gameObject);
    }

    public void PlayerEntered()
    {
        playerInRange = true;
    }

    public void PlayerExited()
    {
        playerInRange = false;
    }
}