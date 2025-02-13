using UnityEngine;
using UnityEngine.Events;


// This script is attached to a gate button object and handles the button click events
 public class GateButtonScript : MonoBehaviour
{
    // Event that is invoked when an escaper clicks the button
    [SerializeField] private UnityEvent OnEscaperClick;
     // Called when a collision is ongoing
    private void OnCollisionStay(Collision coll)
    {
        Debug.Log("collision ongoing");
        // Check if the colliding object is a player and the interact button is pressed
        if (!coll.gameObject.CompareTag("Player") || !Input.GetButtonDown("Interact")) return;
        // Invoke the OnEscaperClick event
        Debug.Log("Clicked");
        OnEscaperClick.Invoke();
    }

    

}


