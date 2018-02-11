using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{

  private int nrOfSlots;
  //TODO: possibly replace value of items with Collectible
  private Dictionary<Collectible, int> items;
  InventoryButton[] buttons;
  public Sprite defaultSprite;

  void Start() {
    nrOfSlots = GameController.instance.panelInventory.GetComponentsInChildren<InventoryButton>().Length;
    buttons = GameController.instance.panelInventory.GetComponentsInChildren<InventoryButton>();
    items = new Dictionary<Collectible, int>();
  }

  public void addItem(Collectible item) {
    if(nrOfSlots == items.Count) {
      //TODO: Output for user
      Debug.Log("Inventory is full");
      return;
    }

    var match = findMatch(item);
    if (match != null) {
      items[match] += 1;
      Debug.Log("Added instance of item " + item.name);
      foreach (InventoryButton button in buttons) {
        int buttonNr = Int32.Parse(button.name.Replace("ButtonItemSlot", ""));
        if (match.inventoryIndex == buttonNr) {
          Text itemInstances = button.GetComponentInChildren<Text>();
          itemInstances.text = items[match].ToString();
          Debug.Log("Added item instances to text of " + button.name);
          if (button.GetComponent<Image>() == null) {
            Debug.Log("Can't add sprite to button");
            break;
          }
          button.GetComponent<Image>().sprite = item.spriteRenderer.sprite;
          break;
        }
      }
    }
    else {
      int currentItemIndex = items.Count + 1;
      item.inventoryIndex = currentItemIndex;
      items[item] = 1;
      Debug.Log("Created new item entry " + item.name + " in inventory");

      InventoryButton button = findButtonMatchingCollectible(item);
      if(button == null) {
        Debug.Log("Button matching failed");
        return;
      }

      Text itemInstances = button.GetComponentInChildren<Text>();
      itemInstances.text = items[item].ToString();
      Debug.Log("Added item name to text of " + button.name);
      if (button.GetComponent<Image>() == null) {
        Debug.Log("Can't add sprite to button");
        return;
      }
      button.GetComponent<Image>().sprite = item.spriteRenderer.sprite;
      Debug.Log("Added item instances to text of " + button.name);
    }    
    showInventoryInfoDebug();
  }

  public void useItem(InventoryButton button) {
    int buttonNr = Int32.Parse(button.name.Replace("ButtonItemSlot", ""));
    var itemToUse = items.Keys.FirstOrDefault(item => item.inventoryIndex == buttonNr);
    if(itemToUse == null) {
      Debug.Log("ItemSlot empty, no item to use");
      return;
    }
    Debug.Log("Use " + itemToUse.name);
    //TODO: Use item
    items[itemToUse] -= 1;
    Text itemInstances = button.GetComponentInChildren<Text>();
    itemInstances.text = items[itemToUse].ToString();
    removeIfNoInstances(itemToUse);
  }

  private void showInventoryInfoDebug() {
    foreach(KeyValuePair<Collectible, int> item in items) {
      Debug.Log("Item: " + item.Key.name + " Instances: " + item.Value);
    }
  }

  public void dropItem(InventoryButton button) {
    int buttonNr = Int32.Parse(button.name.Replace("ButtonItemSlot", ""));
    var itemToUse = items.Keys.FirstOrDefault(item => item.inventoryIndex == buttonNr);
    if (itemToUse == null) {
      Debug.Log("ItemSlot empty, no item to remove");
      return;
    }
    Debug.Log("Remove " + itemToUse.name);
    items[itemToUse] -= 1;
    Text itemInstances = button.GetComponentInChildren<Text>();
    itemInstances.text = items[itemToUse].ToString();
    removeIfNoInstances(itemToUse);
  }

  private void removeIfNoInstances(Collectible item) {
    if (items[item] <= 0) {
      InventoryButton button = findButtonMatchingCollectible(item);
      button.GetComponent<Image>().sprite = defaultSprite;
      items.Remove(item);
      Debug.Log(item.name + " deleted from inventory");
    }
  }
  private Collectible findMatch(Collectible item)
  {
    Collectible match = items.Keys.FirstOrDefault(type => (type.name == item.name));
    return match;
  }

  private InventoryButton findButtonMatchingCollectible(Collectible item) {
    foreach (InventoryButton itButton in buttons) {
      int buttonNr = Int32.Parse(itButton.name.Replace("ButtonItemSlot", ""));
      if (buttonNr == item.inventoryIndex) {
        return itButton;
      }
    }
    return null;
  }

}
