using UnityEngine;

[CreateAssetMenu(fileName = "NewCardDiskon", menuName = "Card Discount/Card Diskon")]
public class CardDiskonData : ScriptableObject
{
    [Header("Informasi Diskon")]
    public string namaKartu;
    [TextArea] public string deskripsiKartu; // Tambahkan deskripsi kartu  
    [Range(1, 100)]
    public int persentaseDiskon;
    
    [Header("Ikon Kartu Diskon")]
    public Sprite imageIcon;

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
