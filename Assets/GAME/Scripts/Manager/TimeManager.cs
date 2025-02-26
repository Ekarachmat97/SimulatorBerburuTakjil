using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; 
    
    private float timeSpeedMultiplier = 2f;
    [Header("Setting kecepatan Waktu")]
    [Tooltip("jika nilai nya lebih besar maka waktunya lebih cepat")]
    [SerializeField]  private float timeSpeed = 3f;

    [Header("Setting Waktu")]
    public TextMeshProUGUI timeText; 
    public int currentDay = 1;
    public int currentMonth = 1;
    public int currentYear = 2022;
    public int currentHour = 14;
    public int currentMinute = 0;

    public int questFailHour = 18;
    public int startHour = 14;

    private float timer = 0f;
    public bool questCompleted = false;
    public bool isWaitingForNextDay = false;
    private RewardUI rewardUI;
    private TakjilQuestManager questManager;
    private MomDialogAndInteraction momDialog;
    private LoadingScreen loadingScreen;
    private PlayerManager playerManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

   private void Start()
    {
        AudioManager.instance.Play("maintheme");

        questManager = FindObjectOfType<TakjilQuestManager>();
        rewardUI = FindObjectOfType<RewardUI>();
        momDialog = FindObjectOfType<MomDialogAndInteraction>();
        loadingScreen = FindObjectOfType<LoadingScreen>();
        playerManager = FindObjectOfType<PlayerManager>();

        // Sinkronisasi awal timeModifier
        UpdateTimeSpeed();

        UpdateTimeUI();
        UpdateLighting();
    }


    private void Update()
    {
        if (playerManager == null) 
        {
            playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager != null)
            {
                UpdateTimeSpeed();
            }
        }
        
        timer += Time.deltaTime * timeSpeedMultiplier;
        if (timer >= 1f) 
        {
            timer = 0f;
            currentMinute++;

            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;

                if (currentHour >= 24)
                {
                    currentHour = 0;
                    NextDay(); 
                    isWaitingForNextDay = false; 
                }
            }

            UpdateTimeUI();
            UpdateLighting();

            if (questCompleted && !isWaitingForNextDay && currentHour >= questFailHour)
            {
                StartCoroutine(ShowBerbukaDialogAndThenLoading());
                isWaitingForNextDay = true;
            }

            if (!questCompleted && !isWaitingForNextDay && currentHour >= questFailHour)
            {
                StartCoroutine(HandleQuestFailureSequence());
                isWaitingForNextDay = true;
            }
            
        }
    }


    public void UpdateTimeSpeed()
    {
        if (playerManager != null)
        {
            timeSpeedMultiplier = Mathf.Max(timeSpeed, 2f - playerManager.timeModifier); // Default 2x lebih cepat
        }
    }


    private IEnumerator ShowBerbukaDialogAndThenLoading()
    {
        momDialog.ShowBerbukaDialog(); 
        yield return new WaitForSeconds(2f);

        rewardUI.OpenRewardUI();
        
       
    }
        

    private IEnumerator HandleQuestFailureSequence()
    {
        // playerManager.SubtractCoins(rewardUI.initialRewardAmount); //kondisi jika uang ibu di kembalikan
        ClearInventoryForQuest(); //untuk menghapus semua makanan di inventory
        momDialog.ShowAngryDialog();
        yield return new WaitForSeconds(3f);

        rewardUI.OpenRewardUI();

       
    }

    private void UpdateTimeUI()
    {
        timeText.text = $"Day {currentDay}/30 {currentYear} Time {currentHour:00}:{currentMinute:00}";
    }

    private void UpdateLighting()
    {
        if (LightingManager.Instance != null)
        {
            LightingManager.Instance.UpdateLightingByTime(currentHour, currentMinute);
        }
    }



    public void NextDay()
    {

        DirectNextDay();
    }

    public void DirectNextDay()
    {
        rewardUI.CloseRewardUI();

        PlayerMovement  playerMovement = FindObjectOfType<PlayerMovement>();
        if(playerMovement != null)
        {
            playerMovement.TeleportToSpawnPoint();
        }
        


        // Reset waktu ke jam awal
        currentHour = startHour;
        currentMinute = 0;

        currentDay++;

        if (currentDay > 30)
        {
            currentDay = 1;
            currentYear++;
        }

        // Reset quest untuk hari baru
       
        questManager.ResetDailyQuest();
        questCompleted = false; // Reset status quest

        // Tampilkan dialog awal dari ibu
        momDialog.ShowInitialDialog();
        isWaitingForNextDay = false; 
    }

    public void CompleteQuest()
    {
        questCompleted = true;
    }

    private void ClearInventoryForQuest()
    {
        InventoryManager.Instance?.ClearInventoryForQuest(questManager.todayTakjilList);
    }

}
