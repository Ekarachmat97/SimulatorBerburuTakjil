using UnityEngine;

public class ItemQuest : MonoBehaviour, IInteractable
{
    public string questItemName = "Barang Hilang";
    private bool isCollected = false;

    public void Interact()
    {
        if (isCollected) return; 

        NPCSideQuest npcSideQuest = FindObjectOfType<NPCSideQuest>();
        if (npcSideQuest != null && npcSideQuest.IsQuestActive())
        {
            npcSideQuest.ItemFound(); 
            isCollected = true;
            gameObject.SetActive(false);
           
        }
    }
}
