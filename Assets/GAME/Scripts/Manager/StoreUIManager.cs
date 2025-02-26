using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreUIManager : MonoBehaviour
{
    public static StoreUIManager Instance;
    public GameObject storePanel;
    public TextMeshProUGUI itemNameText, itemQuantityText, priceText, coinsText, messageText;
    public Image itemIcon;
    private GenericStore activeStore;
    public static GameObject activeMiniGame;

    private PlayerManager playerManager;
    private int itemQuantity = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        UpdateUI();
    }   

    public void ShowStorePanel(GenericStore store)
    {
        Time.timeScale = 0;
        
        activeStore = store; 
        itemNameText.text = store.takjilData.takjilName;
        priceText.text = store.takjilData.price.ToString(); // Ambil harga dari SO
        coinsText.text = playerManager.totalCoins.ToString();

        // Set ikon dari SO TakjilData
        if (store.takjilData != null && store.takjilData.takjilIcon != null)
        {
            itemIcon.sprite = store.takjilData.takjilIcon;
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false); 
        }

        storePanel.SetActive(true);
    }


    public void CloseStorePanel()
    {
        Time.timeScale = 1;
        
        AudioManager.instance.Play("close");
        storePanel.SetActive(false);
        activeStore = null;
    }

     public void OnBuyButton()
    {
        if (activeStore != null)
        {
            activeStore.BuyItem(itemQuantity); // Kirim itemQuantity sebagai parameter
            UpdateUI();
        }
    }

    public void OnHaggleButton()
    {
        activeStore?.Haggle();
    }

    public void IncreaseItemQuantity()
    {
        itemQuantity++;
        UpdateItemQuantity(itemQuantity);
    }

    public void DecreaseItemQuantity()
    {
        if (itemQuantity > 1)
        {
            itemQuantity--;
            UpdateItemQuantity(itemQuantity);
        }
    }

    public void UpdateUI()
{
    if (activeStore != null)
    {
        itemNameText.text = activeStore.takjilData.takjilName;
        priceText.text = activeStore.GetDiscountedPrice().ToString(); // Harga benar
        coinsText.text = PlayerManager.Instance.totalCoins.ToString();
        UpdateItemQuantity(itemQuantity); // Pastikan jumlah item diperbarui dengan benar
    }
}


    public int GetItemQuantity()
    {
        return itemQuantity;
    }


    public void UpdateItemQuantity(int quantity)
    {
        itemQuantityText.text = quantity.ToString();
    }

    public void ShowMessage(string text)
    {
        messageText.text = text;
        StartCoroutine(HideMessage());
    }

    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(3);
        messageText.text = "";
    }
}
