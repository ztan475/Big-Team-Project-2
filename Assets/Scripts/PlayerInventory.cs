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

        UpdateHUD();
    }

    private void UpdateHUD()
    {
        pickupAAmount.text = "PickupA: " + collectibleTypeACount.ToString();
        pickupBAmount.text = "PickupB: " + collectibleTypeBCount.ToString();
    }
}
