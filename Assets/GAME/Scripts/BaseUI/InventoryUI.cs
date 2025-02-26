using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanelUI;
    public GameObject inventoryItemPrefab;
    public Transform inventoryGrid; 

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChangedEvent += UpdateInventoryUI;
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        Dictionary<TakjilData, int> inventory = InventoryManager.Instance.GetInventory();

        foreach (var item in inventory)
        {
            GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryGrid);

            Image iconImage = inventoryItem.GetComponentInChildren<Image>();
            TextMeshProUGUI quantityText = inventoryItem.GetComponentInChildren<TextMeshProUGUI>();

            if (iconImage != null && quantityText != null)
            {
                iconImage.sprite = item.Key.takjilIcon; // Set ikon dari TakjilData
                quantityText.text = item.Value.ToString(); // Set jumlah
            }
        }
    }

    public void OpenInventory()
    {   
        Time.timeScale = 0;
        AudioManager.instance.Play("clicksfx");    
        inventoryPanelUI.SetActive(true);
    }
    public void CloseInventory()
    {
        Time.timeScale = 1;
        AudioManager.instance.Play("close");  
        inventoryPanelUI.SetActive(false);
    }
}