using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PengemisInteraction : MonoBehaviour, IInteractable
{
    [Header("UI References")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public Button giveMoneyButton;
    public Button giveFoodButton;
    public Button closeButton;

    [Header("Pengemis Settings")]
    public int moneyAmount = 500;
    public int pahalaReward = 50;
    public string[] dialogMessage = new string[] 
    {
        "Aaaa kasian aaa...",
        "Tolong, nak, saya belum makan dari pagi.",
        "Kasihanilah saya, demi anak istri di rumah."
    };

    private PlayerManager playerManager;
    private InventoryManager inventoryManager;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        inventoryManager = InventoryManager.Instance;

        if (giveMoneyButton != null) giveMoneyButton.onClick.AddListener(GiveMoney);
        if (giveFoodButton != null) giveFoodButton.onClick.AddListener(GiveFood);
        if (closeButton != null) closeButton.onClick.AddListener(CloseDialog);

        dialogPanel.SetActive(false);
    }

    public void Interact()
    {
        ShowDialog();
    }

    private void ShowDialog()
    {
        Time.timeScale = 0;

        dialogPanel.SetActive(true);
        dialogText.text = GetRandomDialog();
    }

    private string GetRandomDialog()
    {
        if (dialogMessage.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, dialogMessage.Length);
            return dialogMessage[randomIndex];
        }
        return "Kasihanilah saya...";
    }

    private void GiveMoney()
    {
        AudioManager.instance.Play("collectmoney");
        if (playerManager.totalCoins >= moneyAmount)
        {
            playerManager.SubtractCoins(moneyAmount);
            playerManager.AddPahala(pahalaReward);
            dialogText.text = "Terima kasih banyak! Semoga berkah untukmu!";
        }
        else
        {
            AudioManager.instance.Play("close");
            dialogText.text = "Kamu gak punya uang juga!";
        }
    }

    private void GiveFood()
    {
        TakjilData foodToGive = FindAvailableFood(); // Cari makanan di inventory

        if (foodToGive != null)
        {
            inventoryManager.RemoveItem(foodToGive, 1); // Hapus 1 item dari inventory
            playerManager.AddPahala(pahalaReward);
            dialogText.text = $"Terima kasih! {foodToGive.takjilName} ini sangat membantu!";
        }
        else
        {
            AudioManager.instance.Play("close");
            dialogText.text = "Kamu gak punya makanan juga!";
        }
    }

    private TakjilData FindAvailableFood()
    {
        if (inventoryManager == null) return null;

        Dictionary<TakjilData, int> inventory = inventoryManager.GetInventory();
        foreach (var item in inventory)
        {
            if (item.Value > 0)
            {
                return item.Key; // Ambil makanan pertama yang ditemukan
            }
        }
        return null; // Tidak ada makanan
    }

    private void CloseDialog()
    {
        Time.timeScale = 1;
        
        AudioManager.instance.Play("close");
         
        dialogPanel.SetActive(false);
    }
}
