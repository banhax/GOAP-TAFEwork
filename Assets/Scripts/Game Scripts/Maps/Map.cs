using GOAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Data
    public List<Inventory> inventories = new List<Inventory>();

    public List<NPCGOAPHandler> npcs = new List<NPCGOAPHandler>();

    // locations
    #endregion

    #region  Registry

    public void RegisterObject(Object obj) {
        if (obj is Inventory) {
            Register(obj as Inventory);
        }
        else if (obj is NPCGOAPHandler) {
            Register(obj as NPCGOAPHandler);
        }
    }

    void Register(Inventory inventory) {
        if (!inventories.Contains(inventory)) {
            inventories.Add(inventory);
        }
    }

    void Register(NPCGOAPHandler npc) {
        if (!npcs.Contains(npc)) {
            npcs.Add(npc);
        }
    }

    #endregion

    #region Find Functions

    #region Find Nearest

    public Inventory FindNearestObjectOfType(Vector3 searchPosition, G_Inventory type) {
        Inventory inventory = null;
        List<Inventory> foundInventories = FindAllObjectsOfType(type);

        if (foundInventories.Count > 0) {
            float closestDistance = Mathf.Infinity;
            float currentDistance;

            for (int i = 0; i < foundInventories.Count; i++) {
                currentDistance = Vector3.Distance(searchPosition, foundInventories[i].transform.position);
                if (inventory == null || currentDistance < closestDistance) {
                    inventory = foundInventories[i];
                    closestDistance = currentDistance;
                }
            }
        }

        return null;
    }

    #endregion

    #region Find All

    public List<Inventory> FindAllObjectsOfType(G_Inventory inventoryState) {
        List<Inventory> foundInventories = new List<Inventory>();

        for (int i = 0; i < inventories.Count; i++) {
            if (inventories[i].GetWorldState().name == inventoryState.name) {
                foundInventories.Add(inventories[i]);
            }
        }

        return foundInventories;
    }

    #endregion

    #endregion
}
