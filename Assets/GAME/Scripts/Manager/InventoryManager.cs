using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Dictionary<TakjilData, int> inventory = new Dictionary<TakjilData, int>();
    public List<CardDiskonData> kartuDiskonInventory = new List<CardDiskonData>();

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged OnInventoryChangedEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =======================
    // SISTEM TAKJIL INVENTORY
    // =======================

    public void AddItem(TakjilData takjilData, int quantity)
    {
        if (inventory.ContainsKey(takjilData))
        {
            inventory[takjilData] += quantity;
        }
        else
        {
            inventory[takjilData] = quantity;
        }

        Debug.Log($"{quantity} {takjilData.takjilName} ditambahkan ke inventory. Total: {inventory[takjilData]}");

        OnInventoryChangedEvent?.Invoke();
    }

    public bool HasItem(TakjilData takjilData, int quantity)
    {
        return inventory.ContainsKey(takjilData) && inventory[takjilData] >= quantity;
    }

    public bool RemoveItem(TakjilData takjilData, int quantity)
    {
        if (HasItem(takjilData, quantity))
        {
            inventory[takjilData] -= quantity;
            if (inventory[takjilData] <= 0)
            {
                inventory.Remove(takjilData);
            }
            Debug.Log($"{quantity} {takjilData.takjilName} dihapus dari inventory. Sisa: {(inventory.ContainsKey(takjilData) ? inventory[takjilData] : 0)}");

            OnInventoryChangedEvent?.Invoke();
            return true;
        }
        else
        {
            Debug.Log($"Tidak cukup {takjilData.takjilName} dalam inventory!");
            return false;
        }
    }

    public void ClearInventoryForQuest(List<TakjilData> questItems)
    {
        foreach (var takjil in questItems)
        {
            if (inventory.ContainsKey(takjil))
            {
                inventory.Remove(takjil);
                Debug.Log($"{takjil.takjilName} dihapus dari inventory karena quest selesai.");
            }
        }

        OnInventoryChangedEvent?.Invoke();
    }

    public Dictionary<TakjilData, int> GetInventory()
    {
        return new Dictionary<TakjilData, int>(inventory);
    }

    // =======================
    // SISTEM KARTU DISKON
    // =======================

    public void AddCard(CardDiskonData card)
    {
        if (!kartuDiskonInventory.Contains(card))
        {
            kartuDiskonInventory.Add(card);
            card.isCollected = true;
            card.collectionStatus = CardDiskonData.CollectionStatus.Collected;
            Debug.Log($"Kartu diskon '{card.namaKartu}' ({card.persentaseDiskon}%) ditambahkan ke inventory.");

            OnInventoryChangedEvent?.Invoke(); // Update UI setelah kartu ditambahkan
        }
        else
        {
            Debug.Log($"Kamu sudah memiliki kartu diskon '{card.namaKartu}'.");
        }
    }

    public bool HasCardDiskon(CardDiskonData card)
    {
        return kartuDiskonInventory.Contains(card);
    }

    public bool RemoveCardDiskon(CardDiskonData card)
    {
        if (kartuDiskonInventory.Contains(card))
        {
            kartuDiskonInventory.Remove(card);
            card.isCollected = false;
            card.collectionStatus = CardDiskonData.CollectionStatus.NotCollected;
            Debug.Log($"Kartu {card.namaKartu} dihapus dari inventory!");
            OnInventoryChangedEvent?.Invoke();
            return true;
        }
        else
        {
            Debug.Log($"Kartu {card.namaKartu} tidak ditemukan di inventory!");
            return false;
        }
    }

    public List<CardDiskonData> GetCardDiskonInventory()
    {
        return kartuDiskonInventory.FindAll(card => card.isCollected);
    }

    public void PrintInventory()
    {
        Debug.Log("Inventory saat ini:");
        foreach (var item in inventory)
        {
            Debug.Log($"{item.Key.takjilName}: {item.Value}");
        }

        foreach (var card in kartuDiskonInventory)
        {
            Debug.Log($"- {card.namaKartu} ({card.persentaseDiskon}% diskon)");
        }
    }
}
