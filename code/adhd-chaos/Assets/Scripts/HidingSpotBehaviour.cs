using UnityEngine;
using UnityEngine.UI;

public class HidingSpotBehaviour : MonoBehaviour
{
    private Button _button = null;
    
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
        transform.Translate(Vector3.left * 100f); // Move the hiding spot to the left by 100 units
        // Make button non-interactable
        if(_button == null)
        {
            Debug.LogError("Button component is null on: " + gameObject.name);
            return;
        }
        _button.interactable = false;
        Invoke("ResetPosition", 2f); // Reset position after 2 seconds
        
    }
    private void ResetPosition()
    {        
        Debug.Log("Resetting position of: " + gameObject.name);
        transform.Translate(Vector3.right * 100f); // Move it back to the original position
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
