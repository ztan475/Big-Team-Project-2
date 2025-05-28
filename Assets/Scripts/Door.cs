using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int requiredA;
    [SerializeField] private int requiredB;
    public Sprite openDoor;

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    private PlayerInventory inventory;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = PlayerInventory.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player entered door range");
            if (isOpen) return;

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
    }

    private void OpenDoor()
    {
        isOpen = true;
        if (openDoor != null)
        {
            spriteRenderer.sprite = openDoor;
        }
    }
}
