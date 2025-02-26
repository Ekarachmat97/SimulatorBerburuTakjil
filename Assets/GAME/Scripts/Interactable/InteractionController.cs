using UnityEngine;
using UnityEngine.UI; 

public class InteractionController : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactableLayer; 
    public Button interactButton; 

    private IInteractable currentInteractable; 

    void Start()
    {
        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButtonPressed);
            interactButton.interactable = false; 
        }
    }

    void Update()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);
        
        if (hitColliders.Length > 0)
        {
            currentInteractable = hitColliders[0].GetComponent<IInteractable>();
            UpdateInteractButton(true); 
        }
        else
        {
            currentInteractable = null;
            UpdateInteractButton(false); 
        }
    }

    private void OnInteractButtonPressed()
    {
        if (currentInteractable != null)
        {
            AudioManager.instance.Play("clicksfx");
            
            currentInteractable.Interact();
        }
    }

    private void UpdateInteractButton(bool canInteract)
    {
        if (interactButton != null)
        {
            interactButton.interactable = canInteract; 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
