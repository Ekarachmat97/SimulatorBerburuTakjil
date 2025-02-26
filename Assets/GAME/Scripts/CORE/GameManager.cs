using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Dictionary<System.Type, IManager> managers = new Dictionary<System.Type, IManager>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        RegisterManagers();
    }

    private void RegisterManagers()
    {
        foreach (IManager manager in FindObjectsOfType<MonoBehaviour>().OfType<IManager>())
        {
            managers[manager.GetType()] = manager;
            manager.Initialize();
        }
    }

    public T GetManager<T>() where T : class, IManager
    {
        return managers.TryGetValue(typeof(T), out IManager manager) ? manager as T : null;
    }
}
