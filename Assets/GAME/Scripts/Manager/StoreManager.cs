using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;
    
    [SerializeField] private GameObject[] electricStores;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void DisableElectricShops(bool disable)
    {
        foreach (GameObject store in electricStores)
        {
            store.SetActive(!disable);
        }
    }
}
