using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int randomSeed = 420;
    [SerializeField] private int amountOfItemsToFind = 1;
    private List<Image> taskImages = new List<Image>();
   
    private int currentAmntOfHiddenItemsToFind = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Button> spots = findAllButtonsLocations();
        List<Image> hiddenItems = findAllHiddenItemsLocations();
        
        Debug.Log("Found " + spots.Count + " hiding spots and " + hiddenItems.Count + " hidden items in the scene.");
        
        // Randomize the hiding spots that players need to find
        int currentHidingSpotIndex = 0;
        Random.InitState(randomSeed);
        while (currentAmntOfHiddenItemsToFind < amountOfItemsToFind)
        {
            
            if (currentHidingSpotIndex >= spots.Count)
                currentHidingSpotIndex = 0;
            
            Button currentSpot = spots[currentHidingSpotIndex];
            HidingSpotBehaviour btn = currentSpot.GetComponent<HidingSpotBehaviour>();
            if (btn == null)
                continue;
            
            if (btn.isHiding)
                continue;
            
            btn.isHiding = Random.Range(0, 2) == 0;
            if (btn.isHiding)
            {
                currentAmntOfHiddenItemsToFind++;
            }
            currentHidingSpotIndex++;
            Debug.Log("Hiding spot " + currentHidingSpotIndex);
        }
        
        // move position of hidden items to the hiding spots randomly using the random seed
        int foundSpotsCount = 0;
        int totalspotsCount = spots.Count;
        bool[] usedSpots = new bool[totalspotsCount];
        Dictionary<HidingSpotBehaviour, Image> hidingSpots = new Dictionary<HidingSpotBehaviour, Image>();
        while (foundSpotsCount < totalspotsCount)
        {
            int randomIndex = Random.Range(0, totalspotsCount);
            if (usedSpots[randomIndex])
                continue;

            usedSpots[randomIndex] = true;
            Button currentSpot = spots[randomIndex];
            HidingSpotBehaviour btn = currentSpot.GetComponent<HidingSpotBehaviour>();
            if (btn == null)
                continue;

            Image hiddenItem = hiddenItems[foundSpotsCount];
            hiddenItem.transform.position = currentSpot.transform.position;
            hidingSpots.Add(btn, hiddenItem);
            foundSpotsCount++;
        }

        // add all items that have to be found to the taskImages list
        foreach (HidingSpotBehaviour btn in hidingSpots.Keys)
        {
            if (btn.isHiding)
            {
                btn.NeedsToBeFound = true;
                taskImages.Add(hidingSpots[btn]);     
            }
        }
        
        Image [] images = TaskPanelBehaviour.FindAllTaskImages();
        int imageIndex = 1;
        foreach (Image taskImage in taskImages)
        {
            images[imageIndex].sprite = taskImage.sprite;
            // set the index of the hiding spot in the task image
            HidingSpotBehaviour hidingSpot = hidingSpots.FirstOrDefault(x => x.Value == taskImage).Key;
            if (hidingSpot != null)
                hidingSpot.HidingSpotIndex = imageIndex;
            else
                Debug.LogError("Hiding spot not found for task image: " + taskImage.name);
            imageIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private List<Image> findAllHiddenItemsLocations()
    {
        GameObject group = GameObject.Find("HiddenThings");
        List<Image> hiddenItems = new List<Image>();
        group.GetComponentsInChildren<Image>(hiddenItems);
        return hiddenItems;
    }

    private List<Button> findAllButtonsLocations()
    {
        GameObject group = GameObject.Find("HidingSpots");
        List<Button> buttons = new List<Button>();
        group.GetComponentsInChildren<Button>(buttons);
        return buttons;
    }
}
