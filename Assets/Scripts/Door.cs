using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int requiredA;
    [SerializeField] private int requiredB;
    public Sprite openDoor;
    public Gate exit;

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    private PlayerInventory inventory;
    private GameObject player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = PlayerInventory.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (player == null)
            {
                player = other.gameObject;
            }

            Debug.Log("Player entered door range");
            // Teleport player only when open

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
            exit.Teleport(player.transform);
        }
    }
}
