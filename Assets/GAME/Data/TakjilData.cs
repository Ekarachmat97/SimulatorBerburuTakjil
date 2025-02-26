using UnityEngine;

[CreateAssetMenu(fileName = "NewTakjil", menuName = "Takjil System/Takjil")]
public class TakjilData : ScriptableObject
{
    public string takjilName; 
    public Sprite takjilIcon;
    [Min(0)] public int price;
    public bool isCollected; 

    
    public enum CollectionStatus
    {
        NotCollected,
        Collected,
        Locked
    }
    public CollectionStatus collectionStatus = CollectionStatus.NotCollected;
    public BazarType bazarType;
}

public enum BazarType
{
    Default,
    Modern,
    PasarMalam
}
