using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    public GameObject notificationPrefab;
    public Transform canvasTransform;
    public float displayDuration = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowNotification(string message)
    {
        GameObject notification = Instantiate(notificationPrefab, canvasTransform);
        TextMeshProUGUI textComponent = notification.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = message;

        StartCoroutine(FadeOut(notification));
    }

    private IEnumerator FadeOut(GameObject notification)
    {
        CanvasGroup canvasGroup = notification.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(displayDuration);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(notification);
    }
}
