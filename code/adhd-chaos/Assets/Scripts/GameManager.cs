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

    private int currentTaskPrompt = 0; // every time 3 the amountOfItems is found, this number is increased by 1, and added to the seed for randomization
    private List<Button> spots = new List<Button>();
    Dictionary<HidingSpotBehaviour, Image> hidingSpots = new Dictionary<HidingSpotBehaviour, Image>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spots = findAllButtonsLocations();
        List<Image> hiddenItems = findAllHiddenItemsLocations();
        
        Debug.Log("Found " + spots.Count + " hiding spots and " + hiddenItems.Count + " hidden items in the scene.");
        
        // Randomize the hiding spots that players need to find
       RandomizeTaskFinding(); 
        
        // move position of hidden items to the hiding spots randomly using the random seed
        int foundSpotsCount = 0;
        int totalspotsCount = spots.Count;
        bool[] usedSpots = new bool[totalspotsCount];
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
        SetTasksAndImages();       
    }

    // Update is called once per frame
    void Update()
    {
       // Check if all items have been found
       Button[] validHidingSpots = spots.Where(spot => spot.GetComponent<HidingSpotBehaviour>() != null && spot.GetComponent<HidingSpotBehaviour>().NeedsToBeFound).ToArray();
       int cntFoundSpots = validHidingSpots.Count(spot => spot.GetComponent<HidingSpotBehaviour>().HasBeenFound);
       if (cntFoundSpots >= amountOfItemsToFind)
       {
           Debug.Log("All items found! Current task prompt: " + currentTaskPrompt);
           currentTaskPrompt++;
              // Reset the found items
              foreach (HidingSpotBehaviour btn in hidingSpots.Keys)
              {
                  btn.ResetHidingSpot();
              }
              
           RandomizeTaskFinding();
           SetTasksAndImages();
       }
    }
    
    private void RandomizeTaskFinding()
    { 
        int currentHidingSpotIndex = 0;
        currentAmntOfHiddenItemsToFind = 0;
        Random.InitState(randomSeed + currentTaskPrompt);
        foreach (Button spot in spots)
        {
            HidingSpotBehaviour btn = spot.GetComponent<HidingSpotBehaviour>();
            if (btn == null)
                continue;
            btn.isHiding = false; // reset all hiding spots
        }
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
    }

    private void SetTasksAndImages()
    {
        taskImages.Clear();
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
            images[imageIndex].color = Color.white; // reset the color of the image to white
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
