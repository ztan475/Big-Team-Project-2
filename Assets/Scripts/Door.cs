using UnityEngine;

public class Door : MonoBehaviour
{
    public int requiredA = 3;
    public int requiredB = 0;
    public GameObject gameManager;

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered door range");
        if (isOpen) return;

        PlayerInventory inventory = gameManager.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            if (inventory.TrySpendCollectibles(requiredA, requiredB))
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("Not enough collectibles to open this door.");
            }
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        gameObject.SetActive(false); // Or play an animation / move the door
    }
}
