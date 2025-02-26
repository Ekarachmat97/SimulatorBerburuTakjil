using UnityEngine;
using System.Collections.Generic;

public class QuestItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform[] spawnPoints;

    public void SpawnItems(int itemCount)
    {
        ClearExistingItems();

        List<int> availableIndices = new List<int>();

        // indeks spawn point ke dalam list
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < itemCount; i++)
        {
            // reset daftar
            if (availableIndices.Count == 0)
            {
                for (int j = 0; j < spawnPoints.Length; j++)
                {
                    availableIndices.Add(j);
                }
            }

            int randomIndex = Random.Range(0, availableIndices.Count);
            int spawnIndex = availableIndices[randomIndex];

            availableIndices.RemoveAt(randomIndex);

            Instantiate(itemPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        }
    }

    private void ClearExistingItems()
    {
        foreach (var item in FindObjectsOfType<ItemQuest>())
        {
            Destroy(item.gameObject);
        }
    }
}
