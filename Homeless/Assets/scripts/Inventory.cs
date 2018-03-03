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
  private AudioSource audioSource;
  private AudioClip dropClip;

  void Start() {
    nrOfSlots = GameController.instance.panelInventory.GetComponentsInChildren<InventoryButton>().Length;
    buttons = GameController.instance.panelInventory.GetComponentsInChildren<InventoryButton>();
    items = new List<Collectible>();
    audioSource = GetComponent<AudioSource>();
    dropClip = (AudioClip)Resources.Load("sfx/scrunchpaper");
  }

  public bool addItem(Collectible item) {
    if (nrOfSlots == items.Count) {
      //TODO: Output for user
      Debug.Log("Inventory is full");
      return false;
    }

    item.inventoryIndex = findEmptySlotIndex();
    items.Add(item);
    if (!this.name.Equals(mainCharacterName)) {
      showInventoryInfoDebug();
      return true;
    }
    Debug.Log("Created new item entry " + item.name + " in inventory");

    InventoryButton button = findButtonMatchingCollectible(item);
    if (button == null) {
      Debug.Log("Button matching failed");
      return true;
    }

    if (button.GetComponent<Image>() == null) {
      Debug.Log("Can't add sprite to button");
      return true;
    }
    button.GetComponent<Image>().sprite = item.sprite;
    Debug.Log("Added item instances to " + button.name);

    showInventoryInfoDebug();

    return true;
  }

  private int findEmptySlotIndex() {
    for (int i = 1; i <= nrOfSlots; i++) {
      var match = items.Find(item => item.inventoryIndex == i);
      if (match == null) {
        return i;
      }
    }
    return 0;
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
    foreach (Collectible item in items) {
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

    //Drop item on circle around character
    GameObject go = itemToUse.gameObject;
    float r = 1.0f;
    float angle = UnityEngine.Random.Range(0.0f, 360.0f);
    float x = r * Mathf.Cos(angle);
    float y = r * Mathf.Sin(angle);
    go.transform.position = new Vector3(this.transform.position.x + x, this.transform.position.y + y, go.transform.position.z);
    audioSource.clip = dropClip;
    audioSource.Play();
    go.SetActive(true);
  }

  private void removeItemFromInventory(Collectible item) {
    InventoryButton button = findButtonMatchingCollectible(item);
    button.GetComponent<Image>().sprite = defaultSprite;
    items.Remove(item);
    Debug.Log(item.name + " deleted from inventory");
  }

  public Collectible findMatch(Collectible item) {
    Collectible match = items.FirstOrDefault(type => (type.name == item.name));
    return match;
  }

  public Collectible findMatch(String itemName) {
    Collectible match = items.FirstOrDefault(type => (type.name == itemName));
    return match;
  }

  public Collectible findMatch(Collectible.Type itemType) {
    Collectible match = items.FirstOrDefault(type => (type.type == itemType));
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

  public bool giveItem(Collectible collectible, Inventory other) {
    if (!findMatch(collectible)) {
      Debug.Log("Cannot give item: Count = 0");
      return false;
    }
    GameController.instance.player.GetComponent<CharacterAnimation>().playOnce("give_front");
    removeItemFromInventory(collectible);
    other.addItem(collectible);
    return true;
  }

}
