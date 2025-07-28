using UnityEngine;
using UnityEngine.UI;

public class TaskPanelBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static Image[] FindAllTaskImages()
    {
        // Get Panel Object
        GameObject panel = GameObject.Find("Tasks");
        if (panel == null)
        {
            Debug.LogError("Panel object not found in the scene.");
            return new []{ default(Image) };
        }
        
        // Get Image component of the panel
        Image[] panelImage = panel.GetComponents<Image>();
        Image[] images = panelImage[0].GetComponentsInChildren<Image>();
        return images;
    }
}
