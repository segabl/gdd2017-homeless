using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{

  private int nrOfSlots;
  //TODO: possibly replace value of items with Collectible
  private Dictionary<string, int> items;
  Button[] buttons;

  void Start() {
    nrOfSlots = GameController.instance.panelInventory.GetComponentsInChildren<Button>().Length;
    buttons = GameController.instance.panelInventory.GetComponentsInChildren<Button>();
    items = new Dictionary<string, int>();
  }

  public void addItem(Collectible item) {
    if(nrOfSlots == items.Count) {
      //TODO: Output for user
      Debug.Log("Inventory is full");
      return;
    }
    
    if(items.ContainsKey(item.name)) {
      items[item.name] += 1;
      Debug.Log("Added instance of item " + item.name);
      foreach(Button button in buttons) {
        if(button.GetComponentInChildren<Text>().text == item.name) {
          button.GetComponentsInChildren<Text>()[1].text = items[item.name].ToString();
          Debug.Log("Added item instances to text of " + button.name);
          break;
        }
      }
    }
    else {
      items[item.name] = 1;
      Debug.Log("Created new item entry " + item.name + " in inventory");
      Button button = buttons[items.Count - 1];
      buttons = GameController.instance.panelInventory.GetComponentsInChildren<Button>();
      Text itemName = button.GetComponentInChildren<Text>();
      itemName.text = item.name;
      Debug.Log("Added item name to text of " + button.name);
      itemName.GetComponentsInChildren<Text>()[1].text = items[item.name].ToString();
      Debug.Log("Added item instances to text of " + button.name);
    }    
    showInventoryInfoDebug();
  }

  private void showInventoryInfoDebug() {
    foreach(KeyValuePair<string, int> item in items) {
      Debug.Log("Item: " + item.Key + " Instances: " + item.Value);
    }
  }

  public void removeSingleInstanceOfItem(Collectible item) {
    items[item.name] -= 1;
    removeIfNoInstances(item.name);
    Debug.Log("1 instance of " + item.name + " removed from inventory");
  }

  public void removeMultipleInstancesOfItem(Collectible item, int count) {
    items[item.name] -= count;
    removeIfNoInstances(item.name);
    Debug.Log(count + " instances of " + item.name + " removed from inventory");
  }

  private void removeIfNoInstances(string name) {
    if (items[name] <= 0) {
      items.Remove(name);
      Debug.Log(name + " deleted from inventory");
    }
  }

}
