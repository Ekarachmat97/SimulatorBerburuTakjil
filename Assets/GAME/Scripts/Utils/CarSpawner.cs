using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; 
    public Transform spawnPoint; 
    public Transform[] waypoints; 
    public Transform endPoint; 
    public float spawnInterval = 3f; 

    void Start()
    {
        StartCoroutine(SpawnCarRoutine());
    }

    IEnumerator SpawnCarRoutine()
    {
        while (true)
        {
            SpawnCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCar()
    {
        if (carPrefabs.Length > 0 && spawnPoint != null)
        {
            int randomIndex = Random.Range(0, carPrefabs.Length);
            GameObject selectedCar = carPrefabs[randomIndex];

            GameObject newCar = Instantiate(selectedCar, spawnPoint.position, spawnPoint.rotation);
            CarMovement carMovement = newCar.GetComponent<CarMovement>();

            if (carMovement != null)
            {
                // Mengatur waypoint dan endpoint
                carMovement.SetWaypoints(waypoints, endPoint);
            }
        }
    }
}
