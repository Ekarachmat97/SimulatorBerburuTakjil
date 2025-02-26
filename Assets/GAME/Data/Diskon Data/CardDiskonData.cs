using UnityEngine;

[CreateAssetMenu(fileName = "NewCardDiskon", menuName = "Card Discount/Card Diskon")]
public class CardDiskonData : ScriptableObject
{
    [Header("Informasi Diskon")]
    public string namaKartu;
    [Range(1, 100)]
    public int persentaseDiskon;

    [Header("Status Koleksi")]
    public bool isCollected;
    
    public enum CollectionStatus
    {
        NotCollected,
        Collected,
        Locked
    }
    public CollectionStatus collectionStatus = CollectionStatus.NotCollected;

    
}
