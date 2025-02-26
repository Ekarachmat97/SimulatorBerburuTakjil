using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BargainMinigame : MonoBehaviour
{
    public RectTransform indicator;
    public RectTransform progressBar;
    public RectTransform targetZone;
    public Button controlButton;
    public TextMeshProUGUI messagesText;
    private GenericStore store;

    public float moveSpeed = 200f;
    public float targetZoneGravity = 100f;
    public float targetZoneMoveSpeed = 300f;
    public float requiredTime = 1f;
    public Slider progressSlider;
    
    private bool movingUp = true;
    private bool isHolding = false;
    private bool isGameOver = false;
    public float timeInsideTarget = 0f;

    [SerializeField] private CardDiskonData[] availableCards;

    void Start()
    {
        store = FindObjectOfType<GenericStore>();
        if (store != null)
        {
            store.minigameInstance = gameObject;
            Debug.Log("Minigame instance tersimpan di Store!");
        }

        StartCoroutine(MoveIndicatorRandomly());
    }

    void Update()
    {
        if (!isGameOver)
        {
            MoveTargetZone();
            CheckSuccessCondition();
        }
    }

    IEnumerator MoveIndicatorRandomly()
    {
        while (!isGameOver)
        {
            float randomSpeed = Random.Range(moveSpeed * 0.5f, moveSpeed * 1.5f);
            movingUp = Random.value > 0.5f;
            
            float duration = Random.Range(0.5f, 1.5f);
            float elapsed = 0f;
            
            while (elapsed < duration && !isGameOver)
            {
                float step = randomSpeed * Time.unscaledDeltaTime; // Menggunakan unscaledDeltaTime
                Vector2 newPosition = indicator.anchoredPosition;
                
                if (movingUp)
                    newPosition.y += step;
                else
                    newPosition.y -= step;
                
                if (newPosition.y >= progressBar.rect.height / 2)
                    movingUp = false;
                else if (newPosition.y <= -progressBar.rect.height / 2)
                    movingUp = true;
                
                indicator.anchoredPosition = newPosition;
                elapsed += Time.unscaledDeltaTime; // Menggunakan unscaledDeltaTime
                yield return null;
            }
        }
    }

    void MoveTargetZone()
    {
        if (isGameOver) return; // Stop movement if the game is over

        Vector2 newPosition = targetZone.anchoredPosition;
        
        if (isHolding)
            newPosition.y += targetZoneMoveSpeed * Time.unscaledDeltaTime; // Menggunakan unscaledDeltaTime
        else
            newPosition.y -= targetZoneGravity * Time.unscaledDeltaTime; // Menggunakan unscaledDeltaTime

        newPosition.y = Mathf.Clamp(newPosition.y, -progressBar.rect.height / 2, progressBar.rect.height / 2);
        targetZone.anchoredPosition = newPosition;
    }

    void CheckSuccessCondition()
    {
        if (indicator.anchoredPosition.y >= targetZone.anchoredPosition.y - targetZone.rect.height / 2 &&
            indicator.anchoredPosition.y <= targetZone.anchoredPosition.y + targetZone.rect.height / 2)
        {
            timeInsideTarget += Time.unscaledDeltaTime; // Menggunakan unscaledDeltaTime
            progressSlider.value = timeInsideTarget / requiredTime; // Update progress slider

            if (timeInsideTarget >= requiredTime)
            {
                WinGame();
            }
        }
        else
        {
            timeInsideTarget = 0f;
            progressSlider.value = 0f; // Reset progress slider saat keluar dari target zone
        }
    }

   void WinGame()
    {
         
        isGameOver = true; // Stop the game
        controlButton.interactable = false; // Disable button
        StopAllCoroutines(); // Stop all coroutines (including MoveIndicatorRandomly)

        // Dapatkan kartu diskon secara acak
        CardDiskonData kartuDiskon = GetRandomCardDiskon();
        if (kartuDiskon != null)
        {
            AudioManager.instance.Play("minigamewin");
            // Tambahkan kartu ke inventory
            InventoryManager.Instance.AddCard(kartuDiskon);

            messagesText.text = $"Berhasil! Kamu dapat kartu diskon '{kartuDiskon.namaKartu}' dengan {kartuDiskon.persentaseDiskon}% diskon!";
        }
        else
        {
            messagesText.text = "Berhasil! Tapi kamu tidak mendapatkan kartu diskon.";
        }
    }

    CardDiskonData GetRandomCardDiskon()
    {
        if (availableCards.Length == 0) return null;

        int randomIndex = Random.Range(0, availableCards.Length);
        return availableCards[randomIndex];
    }





    public void OnHoldButtonDown()
    {
        if (!isGameOver)
            isHolding = true;
    }

    public void OnHoldButtonUp()
    {
        if (!isGameOver)
            isHolding = false;
    }

    public void CloseMiniGame()
    {
        if (store != null)
        {
            store.LeaveMinigame();
        }
    }

}
