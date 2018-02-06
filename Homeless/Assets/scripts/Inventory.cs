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
  Button[] buttons;

  void Start() {
    nrOfSlots = GameController.instance.panelInventory.GetComponentsInChildren<Button>().Length;
    buttons = GameController.instance.panelInventory.GetComponentsInChildren<Button>();
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
      foreach(Button button in buttons) {
        if(button.GetComponentInChildren<Text>().text == item.name) {
          button.GetComponentsInChildren<Text>()[1].text = items[match].ToString();
          Debug.Log("Added item instances to text of " + button.name);
          break;
        }
      }
    }
    else {
      items[item] = 1;
      Debug.Log("Created new item entry " + item.name + " in inventory");
      Button button = buttons[items.Count - 1];
      buttons = GameController.instance.panelInventory.GetComponentsInChildren<Button>();
      Text itemName = button.GetComponentInChildren<Text>();
      itemName.text = item.name;
      Debug.Log("Added item name to text of " + button.name);
      itemName.GetComponentsInChildren<Text>()[1].text = items[item].ToString();
      Debug.Log("Added item instances to text of " + button.name);
    }    
    showInventoryInfoDebug();
  }

  private void showInventoryInfoDebug() {
    foreach(KeyValuePair<Collectible, int> item in items) {
      Debug.Log("Item: " + item.Key.name + " Instances: " + item.Value);
    }
  }

  public void removeSingleInstanceOfItem(Collectible item) {
    var match = findMatch(item);
    if (match == null)
    {
      Debug.Log("Failed to remove Item: Invalid Key");
      return;
    }
    items[match] -= 1;
    removeIfNoInstances(match);
    Debug.Log("1 instance of " + item.name + " removed from inventory");
  }

  public void removeMultipleInstancesOfItem(Collectible item, int count) {
    var match = findMatch(item);
    if (match == null)
    {
      Debug.Log("Failed to remove Item: Invalid Key");
      return;
    }
    items[match] -= count;
    removeIfNoInstances(match);
    Debug.Log(count + " instances of " + item.name + " removed from inventory");
  }

  private void removeIfNoInstances(Collectible type) {
    if (items[type] <= 0) {
      items.Remove(type);
      Debug.Log(name + " deleted from inventory");
    }
  }
  private Collectible findMatch(Collectible item)
  {
    Collectible match = items.Keys.FirstOrDefault(type => (type.name == item.name));
    return match;
  }

}
