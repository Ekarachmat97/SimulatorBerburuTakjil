using TMPro;
using UnityEngine;
using UnityEngine.UI;

//ini termasuk script MiniGame yang kedua
public class TawarButton : MonoBehaviour
{
    public ArrowController arrowController;
    public RectTransform arrow;
    public RectTransform[] discountAreas;
    public TextMeshProUGUI messageText;
    public Button tawarButtonPressed;
    private GenericStore store;

    private int currentDiscount = 0;
    private bool hasCheckedDiscount = false;
    [SerializeField] private CardDiskonData[] availableCards;

    private void Start()
    {
        store = FindObjectOfType<GenericStore>();
        tawarButtonPressed.onClick.AddListener(CheckDiscount);
    }

    // Fungsi untuk mengecek hasil diskon
    void CheckDiscount()
    {
        if (hasCheckedDiscount) return; // Cegah spam klik

        hasCheckedDiscount = true;
        tawarButtonPressed.interactable = false; // Disable tombol

        arrowController.StopArrow();

        RectTransform bestMatchArea = null;
        int bestMatchDiscount = 0;

        foreach (RectTransform discountArea in discountAreas)
        {
            if (IsArrowInDiscountArea(arrow, discountArea))
            {
                int discountValue = GetDiscountValue(discountArea);

                if (discountValue > 0)
                {
                    bestMatchArea = discountArea;
                    bestMatchDiscount = discountValue;
                    break; // Ambil diskon pertama yang ditemukan
                }
            }
        }

        if (bestMatchArea != null)
        {
            currentDiscount = bestMatchDiscount;
            AudioManager.instance.Play("minigamewin");

            messageText.text = $"Selamat! Anda mendapatkan diskon {currentDiscount}%!";

            // Berikan kartu diskon sesuai diskon yang didapat
            CardDiskonData kartuDiskon = GetCardDiskonByDiscount(currentDiscount);
            if (kartuDiskon != null && InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddCard(kartuDiskon);
                messageText.text += $"\nKamu juga dapat kartu '{kartuDiskon.namaKartu}'!";
            }
        }
        else
        {
            messageText.text = "Anda tidak mendapatkan diskon.";
        }
    }

    private CardDiskonData GetCardDiskonByDiscount(int discount)
    {
        foreach (CardDiskonData card in availableCards)
        {
            if (card.namaKartu.Contains($"{discount}%")) // Pastikan kartu sesuai diskon
            {
                return card;
            }
        }
        return null;
    }

    private int GetDiscountValue(RectTransform discountArea)
    {
        string discountText = discountArea.gameObject.name;
        int discount = 0;

        if (!string.IsNullOrEmpty(discountText) && TryParseDiscount(discountText, out discount))
        {
            return discount;
        }

        return 0;
    }

    private bool TryParseDiscount(string name, out int discount)
    {
        discount = 0;
        string discountValue = System.Text.RegularExpressions.Regex.Match(name, @"\d+").Value;
        return !string.IsNullOrEmpty(discountValue) && int.TryParse(discountValue, out discount);
    }

    private bool IsArrowInDiscountArea(RectTransform arrow, RectTransform discountArea)
    {
        Vector2 arrowScreenPos = RectTransformUtility.WorldToScreenPoint(null, arrow.position);
        return RectTransformUtility.RectangleContainsScreenPoint(discountArea, arrowScreenPos, null);
    }

    public void CloseMiniGame()
    {
        store.LeaveMinigame();
    }
}
