using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CodePenawaran : MonoBehaviour
{
    public static CodePenawaran Instance;

    [Header("UI Components")]
    public TMP_InputField inputField;
    public Button submitButton;

    private Dictionary<string, System.Action> promoCodes = new Dictionary<string, System.Action>();
    private HashSet<string> usedCodes = new HashSet<string>();

    [Header("Referal Code")]
    [SerializeField] private string codeSuperCoins = "SUPERCOINS";
    [SerializeField] private string codeExtraPahala = "EXTRAPAHALA";
    [SerializeField] private string codeSpeedBoost = "SPEEDBOOST";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        promoCodes.Add(codeSuperCoins, () => TambahCoins(1000000));
        promoCodes.Add(codeExtraPahala, () => TambahPahala(500));
        promoCodes.Add(codeSpeedBoost, () => TambahSpeed(2f));

        if (submitButton != null)
            submitButton.onClick.AddListener(SubmitCode);
    }

    public void SubmitCode()
    {
        if (inputField != null)
        {
            string enteredCode = inputField.text.ToUpper();
            RedeemCode(enteredCode);
            inputField.text = "";
        }
    }

    public void RedeemCode(string code)
    {
        if (usedCodes.Contains(code))
        {
            Debug.Log("Kode sudah pernah digunakan!");
            NotificationManager.Instance.ShowNotification("Kode sudah pernah digunakan!");
            return;
        }

        if (promoCodes.ContainsKey(code))
        {
            promoCodes[code].Invoke();
            usedCodes.Add(code); // Tandai kode sudah digunakan
            Debug.Log("Kode berhasil digunakan: " + code);
        }
        else
        {
            Debug.Log("Kode tidak valid!");
            NotificationManager.Instance.ShowNotification("Kode tidak valid!");
        }
    }

    void TambahCoins(int jumlah)
    {
        PlayerManager.Instance.AddCoins(jumlah);
        NotificationManager.Instance.ShowNotification("Kamu mendapatkan " + jumlah + " koin!");
    }

    void TambahPahala(int jumlah)
    {
        PlayerManager.Instance.AddPahala(jumlah);
        NotificationManager.Instance.ShowNotification("Kamu mendapatkan " + jumlah + " pahala!");
    }

    void TambahSpeed(float nilai)
    {
        PlayerManager.Instance.movementSpeed += nilai;
        NotificationManager.Instance.ShowNotification("Kecepatan bertambah " + nilai + "!");
    }
}
