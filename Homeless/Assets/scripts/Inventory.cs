using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
  private const string inventoryButtonPrefix = "ButtonItemSlot";
  private const string mainCharacterName = "MainCharacter";
  private int nrOfSlots;
  private List<Collectible> items;
  InventoryButton[] buttons;
  public Sprite defaultSprite;

  void Start() {
    nrOfSlots = GameController.instance.panelInventory.GetComponentsInChildren<InventoryButton>().Length;
    buttons = GameController.instance.panelInventory.GetComponentsInChildren<InventoryButton>();
    items = new List<Collectible>();
  }

  public void addItem(Collectible item) {
    if (nrOfSlots == items.Count) {
      //TODO: Output for user
      Debug.Log("Inventory is full");
      return;
    }

    int currentItemIndex = items.Count;
    item.inventoryIndex = currentItemIndex + 1;
    items.Add(item);
    if (!this.name.Equals(mainCharacterName)) {
      showInventoryInfoDebug();
      return;
    }
    Debug.Log("Created new item entry " + item.name + " in inventory");

    InventoryButton button = findButtonMatchingCollectible(item);
    if (button == null) {
      Debug.Log("Button matching failed");
      return;
    }

    if (button.GetComponent<Image>() == null) {
      Debug.Log("Can't add sprite to button");
      return;
    }
    button.GetComponent<Image>().sprite = item.sprite;
    Debug.Log("Added item instances to " + button.name);

    showInventoryInfoDebug();
  }

  public void useItem(InventoryButton button) {
    int buttonNr = Int32.Parse(button.name.Replace(inventoryButtonPrefix, ""));
    var itemToUse = items.FirstOrDefault(item => item.inventoryIndex == buttonNr);
    if (itemToUse == null) {
      Debug.Log("ItemSlot empty, no item to use");
      return;
    }
    Debug.Log("Use " + itemToUse.name);
    Character character = this.GetComponent<Character>();
    if (character == null) {
      Debug.Log("No Character script attached to: " + this.name);
      return;
    }
    if (itemToUse.use(character)) { 
      removeItemFromInventory(itemToUse);
    }
  }

  private void showInventoryInfoDebug() {
    foreach(Collectible item in items) {
      Debug.Log("Item: " + item.name);
    }
  }

  public void dropItem(InventoryButton button) {
    int buttonNr = Int32.Parse(button.name.Replace(inventoryButtonPrefix, ""));
    var itemToUse = items.FirstOrDefault(item => item.inventoryIndex == buttonNr);
    if (itemToUse == null) {
      Debug.Log("ItemSlot empty, no item to remove");
      return;
    }
    Debug.Log("Remove " + itemToUse.name);
    removeItemFromInventory(itemToUse);
  }

  private void removeItemFromInventory(Collectible item) {
    InventoryButton button = findButtonMatchingCollectible(item);
    button.GetComponent<Image>().sprite = defaultSprite;
    items.Remove(item);
    Debug.Log(item.name + " deleted from inventory");
  }

  private Collectible findMatch(Collectible item)
  {
    Collectible match = items.FirstOrDefault(type => (type.name == item.name));
    return match;
  }

  private Collectible findMatch(String itemName) {
    Collectible match = items.FirstOrDefault(type => (type.name == itemName));
    return match;
  }

  private InventoryButton findButtonMatchingCollectible(Collectible item) {
    foreach (InventoryButton itButton in buttons) {
      int buttonNr = Int32.Parse(itButton.name.Replace(inventoryButtonPrefix, ""));
      if (buttonNr == item.inventoryIndex) {
        return itButton;
      }
    }
    return null;
  }

  public bool giveItem(String itemName, Inventory other)
  {
    Collectible itemToGive = findMatch(itemName);
    if (itemToGive == null)
    {
      Debug.Log("Cannot give item: Count = 0");
      return false;
    }
    removeItemFromInventory(itemToGive);
    other.addItem(itemToGive);
    return true;
  }

}
