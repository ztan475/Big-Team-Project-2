using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player HUD")]
    [SerializeField] private TextMeshProUGUI pickupAAmount;
    [SerializeField] private TextMeshProUGUI pickupBAmount;

    [Header("Collectible Amounts")]
    public int collectibleTypeACount = 0;
    public int collectibleTypeBCount = 0;

    [Header("Door Logic")]
    public GameObject[] doorsToOpen;
    public int requiredA = 1;
    public int requiredB = 0;

    private bool doorsOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        pickupAAmount = GameObject.Find("PickupAAmount").GetComponent<TextMeshProUGUI>();
        pickupBAmount = GameObject.Find("PickupBAmount").GetComponent<TextMeshProUGUI>();
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCollectible(CollectibleType type)
    {
        switch (type)
        {
            case CollectibleType.TypeA:
                collectibleTypeACount++;
                break;

            case CollectibleType.TypeB:
                collectibleTypeBCount++;
                break;

            default:
                Debug.LogWarning("Collectible doesn't exist");
                break;
        }

        // CheckToOpenDoors();
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        pickupAAmount.text = "PickupA: " + collectibleTypeACount.ToString();
        pickupBAmount.text = "PickupB: " + collectibleTypeBCount.ToString();
    }

    // private void CheckToOpenDoors()
    // {
    //     if (!doorsOpened && collectibleTypeACount >= requiredA && collectibleTypeBCount >= requiredB)
    //     {
    //         foreach (GameObject door in doorsToOpen)
    //         {
    //             if (door != null)
    //             {
    //                 door.SetActive(false);
    //             }
    //         }
    //         doorsOpened = true;
    //     }
    // }
    
    public bool TrySpendCollectibles(int costA, int costB)
    {
        if (collectibleTypeACount >= costA && collectibleTypeBCount >= costB)
        {
            collectibleTypeACount -= costA;
            collectibleTypeBCount -= costB;
            UpdateHUD();
            return true;
        }
        return false;
    }

}
