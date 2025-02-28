using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject inventoryPanelUI;
    public GameObject inventoryItemPrefab;
    public Transform inventoryGrid;
    public TextMeshProUGUI itemDescriptionText; // UI untuk menampilkan deskripsi item

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

        Dictionary<TakjilData, int> takjilInventory = InventoryManager.Instance.GetInventory();
        List<CardDiskonData> cardDiskonInventory = InventoryManager.Instance.GetCardDiskonInventory();

        // Tampilkan takjil dalam inventory
        foreach (var item in takjilInventory)
        {
            GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryGrid);

            Image iconImage = inventoryItem.GetComponentInChildren<Image>();
            TextMeshProUGUI quantityText = inventoryItem.GetComponentInChildren<TextMeshProUGUI>();
            Button selectButton = inventoryItem.GetComponentInChildren<Button>(); // Ambil button dari prefab

            if (iconImage != null && quantityText != null && selectButton != null)
            {
                iconImage.sprite = item.Key.takjilIcon; // Set ikon dari TakjilData
                quantityText.text = item.Value.ToString(); // Set jumlah

                // Tambahkan event ke button
                selectButton.onClick.AddListener(() => ShowItemDescription(item.Key));
            }
        }

        // Tampilkan kartu diskon dalam inventory
        foreach (var card in cardDiskonInventory)
        {
            GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryGrid);

            Image iconImage = inventoryItem.GetComponentInChildren<Image>();
            TextMeshProUGUI discountText = inventoryItem.GetComponentInChildren<TextMeshProUGUI>();
            Button selectButton = inventoryItem.GetComponentInChildren<Button>(); // Ambil button dari prefab

            if (iconImage != null && discountText != null && selectButton != null)
            {
                iconImage.sprite = card.imageIcon; // Set ikon kartu diskon
                discountText.text = $"{card.persentaseDiskon}%"; // Tampilkan persentase diskon

                // Tambahkan event ke button
                selectButton.onClick.AddListener(() => ShowCardDescription(card));
            }
        }
    }

    private void ShowItemDescription(TakjilData takjil)
    {
        itemDescriptionText.text = $"{takjil.takjilName}";
    }

    private void ShowCardDescription(CardDiskonData card)
    {
        itemDescriptionText.text = $"{card.namaKartu}\nDiskon: {card.persentaseDiskon}%\n{card.deskripsiKartu}";
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

        itemDescriptionText.text = "";
        AudioManager.instance.Play("close");  
        inventoryPanelUI.SetActive(false);
    }
}
