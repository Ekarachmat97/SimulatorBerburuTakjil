using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                // Teleport player ke spawnPoint
                playerMovement.TeleportToSpawnPoint();

                // Mulai efek blink pada playerModel
                StartCoroutine(BlinkEffect(playerMovement));
            }
        }
    }

    private IEnumerator BlinkEffect(PlayerMovement playerMovement)
    {
        GameObject playerModel = playerMovement.playerModel;

        if (playerModel != null)
        {
            for (int i = 0; i < 6; i++) // 6 kali (3 detik, karena 0.5 detik per loop)
            {
                playerModel.SetActive(false);
                yield return new WaitForSeconds(0.25f);
                playerModel.SetActive(true);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
