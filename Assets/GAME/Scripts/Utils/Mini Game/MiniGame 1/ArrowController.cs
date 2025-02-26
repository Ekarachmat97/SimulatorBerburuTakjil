using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public RectTransform arrow; 
    public RectTransform bar; 
    public float speed = 500f; 
    private bool movingRight = true;
    private bool isArrowStopped = false; 

    void Update()
    {
        // Jika panah sedang dihentikan, jangan lakukan apapun
        if (isArrowStopped) return;

        // Gerakkan panah ke kanan/kiri menggunakan Time.unscaledDeltaTime
        if (movingRight)
            arrow.anchoredPosition += Vector2.right * speed * Time.unscaledDeltaTime;
        else
            arrow.anchoredPosition += Vector2.left * speed * Time.unscaledDeltaTime;

        // Pantulan panah
        if (arrow.anchoredPosition.x >= bar.rect.width / 2)
            movingRight = false;
        else if (arrow.anchoredPosition.x <= -bar.rect.width / 2)
            movingRight = true;
    }

    // Fungsi untuk menghentikan pergerakan panah
    public void StopArrow()
    {
        isArrowStopped = true;
    }

    // Fungsi untuk memulai kembali pergerakan panah (opsional)
    public void StartArrow()
    {
        isArrowStopped = false;
    }
}
