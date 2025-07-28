using UnityEngine;
using UnityEngine.UI;

public class HidingSpotBehaviour : MonoBehaviour
{
    private Button _button = null;
    // Editor fields to set the reset time in seconds and the distance to move
    [SerializeField] private float moveDistance = 100f; 
    [SerializeField] private float resetTime = 2f;
    [SerializeField] private bool isHiding = true;
    
    
    private void Awake()
    {
        gameObject.TryGetComponent<Button>(out Button fetchedButton);
        if (fetchedButton != null)
        {
            _button = fetchedButton;
            _button.onClick.AddListener(OnButtonClick);
        }
    }
    
    private void OnButtonClick()
    {
        Debug.Log("Hiding spot selected: " + gameObject.name);
        transform.Translate(Vector3.left * moveDistance); // Move the hiding spot to the left by 100 units
        // Make button non-interactable
        if(_button == null)
        {
            Debug.LogError("Button component is null on: " + gameObject.name);
            return;
        }
        _button.interactable = false;
        
        // ToDo: Give Points to Player, Complete Task
        if(isHiding)
            Debug.Log("Hiding spot selected: " + gameObject.name);
        
        Invoke("ResetPosition", resetTime);
        
    }
    private void ResetPosition()
    {        
        Debug.Log("Resetting position of: " + gameObject.name);
        transform.Translate(Vector3.right * moveDistance); // Move it back to the original position
        // Make button interactable again
        if(_button == null)
        {
            Debug.LogError("Button component is null on: " + gameObject.name);
            return;
        }
        
        Debug.Log("Hiding spot re-enabled: " + gameObject.name);
        _button.interactable = true;
        Debug.Log("Hiding spot enabled: " + _button.IsInteractable());
    }
}
