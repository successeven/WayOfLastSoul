using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class AttackItem : MonoBehaviour
{
		new public string name = "New Item";  // Name of the item
		public Sprite icon = null;        // Item icon
		public bool showInInventory = true;

		// Called when the item is pressed in the inventory
		public virtual void Use()
		{
				// Use the item
				// Something may happen
		}

		// Call this method to remove the item from inventory
		public void RemoveFromInventory()
		{
				Inventory.instance.Remove(this);
		}

}
