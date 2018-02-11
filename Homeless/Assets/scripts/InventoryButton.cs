using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerClickHandler {

  public void OnPointerClick(PointerEventData eventData) {
    Inventory inventory = GameController.instance.player.GetComponent<Inventory>();
    if (eventData.button == PointerEventData.InputButton.Left) {
      Debug.Log("InventoryItem Left click: Use");
      inventory.useItem(this);
    }
    else if (eventData.button == PointerEventData.InputButton.Middle) {
      Debug.Log("Middle click");

    }
    else if (eventData.button == PointerEventData.InputButton.Right) {
      Debug.Log("InventoryItem Right click: Drop");
      inventory.dropItem(this);
    }
  }
}