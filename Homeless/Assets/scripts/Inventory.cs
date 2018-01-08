using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : PausableObject {

  private bool isOpen = false;
  //TODO: change to a reasonable value
  private int nrOfSlots = 10;
  private Dictionary<string, int> items = new Dictionary<string, int>();

  private static Inventory instance;

  void Start() { 
    instance = this;
  }

  public static Inventory getInstance() {
    return instance;
  }

  protected override void updatePausable() {
    if (Input.GetKeyDown(KeyCode.I) && !isOpen) {
      isOpen = true;
      //TODO: show inventory
    }
    else if(Input.GetKeyDown(KeyCode.I) && isOpen) {
      isOpen = false;
      //TODO: hide inventory
    }
  }

  public void addItem(Collectible item) {
    if(nrOfSlots == items.Count) {
      //TODO: Output for user
      Debug.Log("Inventory is full");
      return;
    }
    
    if(items.ContainsKey(item.name)) {
      items[item.name] += 1;
    }
    else {
      items[item.name] = 1;
    }    
    Debug.Log("Added item " + item.name + " to inventory");
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
