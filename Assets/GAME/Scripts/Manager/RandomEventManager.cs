using System.Collections;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance;

    public bool isHujan = false;
    public bool isListrikMati = false;

    [SerializeField] private float minTimeBetweenEvents = 30f;
    [SerializeField] private float maxTimeBetweenEvents = 60f;
    [SerializeField] private float eventDuration = 15f;

    [Header("Efek Visual")]
    [SerializeField] private ParticleSystem hujanParticle; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
         if (hujanParticle != null)
            hujanParticle.Stop();


        StartCoroutine(EventLoop());
    }

    private IEnumerator EventLoop()
    {
        while (true)
        {
            float waktuTunggu = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
            yield return new WaitForSeconds(waktuTunggu);

            int randomEvent = Random.Range(0, 2);
            if (randomEvent == 0)
                StartCoroutine(HujanEvent());
            else
                StartCoroutine(ListrikMatiEvent());
        }
    }

     

    private IEnumerator HujanEvent()
{
    isHujan = true;
    Debug.Log("Event: Hujan turun!");
    NotificationManager.Instance.ShowNotification("Hujan turun, jalanan jadi licin!");

    // Aktifkan efek hujan
   if (hujanParticle != null)
            hujanParticle.Play();
    LightingManager.Instance.SetHujan(true);

    yield return new WaitForSeconds(eventDuration);

    isHujan = false;
    Debug.Log("Hujan berhenti.");
    NotificationManager.Instance.ShowNotification("Hujan berhenti!");

    // Matikan efek hujan
    if (hujanParticle != null)
            hujanParticle.Stop();
    LightingManager.Instance.SetHujan(false);
}


    private IEnumerator ListrikMatiEvent()
    {
        isListrikMati = true;
      
        NotificationManager.Instance.ShowNotification("Listrik padam!");

        StoreManager.Instance.DisableElectricShops(true);
        
        // Matikan lampu taman dan direction light melalui LightingManager
        LightingManager.Instance.SetListrikMati(true);

        yield return new WaitForSeconds(eventDuration);

        isListrikMati = false;
      
        NotificationManager.Instance.ShowNotification("Listrik kembali menyala.");
        StoreManager.Instance.DisableElectricShops(false);

        // Hidupkan kembali pencahayaan sesuai waktu saat ini
        LightingManager.Instance.SetListrikMati(false);
    }

}
