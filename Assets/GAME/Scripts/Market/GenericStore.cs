using UnityEngine;
using System.Linq;

public class GenericStore : MonoBehaviour, IInteractable
{
    private PlayerManager playerManager;
    public TakjilData takjilData;
    public MinigameData minigameData;
    public GameObject minigameInstance;
    private int currentDiscount = 0;
    private float playerDiscountBonus = 0f; // Diskon tambahan dari upgrade


    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }
    void Start()
    {
        UpdatePlayerDiscountBonus();
    }

    public void Interact()
    {
        StoreUIManager.Instance.ShowStorePanel(this);
    }

    public void SetDiscount(int discount)
    {
        currentDiscount = discount;
        Debug.Log($"Diskon tambahan {discount}% diterapkan!");
    }

    private void ResetDiscount()
    {
        currentDiscount = 0;
    }

    public int GetDiscountedPrice()
    {
        if (takjilData == null) return 0; // Jika tidak ada data, harga 0

        int itemQuantity = StoreUIManager.Instance.GetItemQuantity(); // Ambil jumlah item dari StoreUIManager
        int basePrice = takjilData.price * itemQuantity; // Harga dasar berdasarkan jumlah item

        int highestDiscount = InventoryManager.Instance.kartuDiskonInventory
            .Select(card => card.persentaseDiskon)
            .DefaultIfEmpty(0)
            .Max();

        float totalDiscount = 1 - ((currentDiscount + highestDiscount + playerDiscountBonus) / 100f);
        return Mathf.RoundToInt(basePrice * totalDiscount);
    }

    public void UpdatePlayerDiscountBonus()
    {
        if (playerManager != null)
        {
            playerDiscountBonus = playerManager.discountBonus; // Ambil nilai diskon dari PlayerManager
        }
    }



    public CardDiskonData GetActiveDiscount()
    {
        return InventoryManager.Instance.kartuDiskonInventory.FirstOrDefault();
    }

  public void Haggle()
{    
    AudioManager.instance.Play("clicksfx");

    if (InventoryManager.Instance.kartuDiskonInventory.Count > 0)
    {
        StoreUIManager.Instance.ShowMessage("Kamu sudah memiliki diskon dek");
        return;
    }

    if (minigameData != null && minigameData.minigamePanel != null)
    {
        Time.timeScale = 0;
        GameObject uiContainer = GameObject.Find("UI");

        StoreUIManager.activeMiniGame = Instantiate(minigameData.minigamePanel, uiContainer.transform);   
    }
}


public void LeaveMinigame()
{
    Time.timeScale = 1;
    if (StoreUIManager.activeMiniGame != null)
    {
        Destroy(StoreUIManager.activeMiniGame);
        StoreUIManager.activeMiniGame = null;
    }
}




    public void BuyItem(int quantity)
{
    AudioManager.instance.Play("collectmoney");
    int totalPrice = GetDiscountedPrice() * quantity; // Kalikan dengan jumlah item yang akan dibeli

    if (playerManager.totalCoins >= totalPrice)
    {
        playerManager.totalCoins -= totalPrice;
        playerManager.UpdateUI();
        InventoryManager.Instance.AddItem(takjilData, quantity);

        CardDiskonData activeDiscount = GetActiveDiscount();
        if (activeDiscount != null)
        {
            InventoryManager.Instance.RemoveCardDiskon(activeDiscount);
            Debug.Log($"Kartu diskon '{activeDiscount.namaKartu}' ({activeDiscount.persentaseDiskon}%) telah digunakan dan dihapus.");
        }

        if (takjilData != null && !takjilData.isCollected)
        {
            takjilData.isCollected = true;
        }

        Debug.Log($"Berhasil membeli {quantity} {takjilData.takjilName} dengan harga {totalPrice}!");
    }
    else
    {
        StoreUIManager.Instance.ShowMessage("Tidak cukup uang, kembali bekerja!");
    }
}

}
