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
    [SerializeField] private int collectibleTypeACount;
    [SerializeField] private int collectibleTypeBCount;

    public static PlayerInventory Instance { get; private set; } 

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        pickupAAmount = GameObject.Find("PickupAAmount").GetComponent<TextMeshProUGUI>();
        pickupBAmount = GameObject.Find("PickupBAmount").GetComponent<TextMeshProUGUI>();
        UpdateHUD();
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
