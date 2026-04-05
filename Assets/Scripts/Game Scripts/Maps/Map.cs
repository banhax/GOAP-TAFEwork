using GOAP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Data
    public List<Inventory> inventories = new List<Inventory>();

    public List<NPCGOAPHandler> npcs = new List<NPCGOAPHandler>();

    public List<LocationInstance> locations = new List<LocationInstance>();
    #endregion

    #region  Registry

    public void RegisterObject(Object obj) {
        if (obj is Inventory) {
            Register(obj as Inventory);
        }
        else if (obj is NPCGOAPHandler) {
            Register(obj as NPCGOAPHandler);
        }
        else if (obj is LocationInstance) {
            Register(obj as LocationInstance);
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

    void Register(LocationInstance location) {
        if (!locations.Contains(location)) {
            locations.Add(location);
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

        return inventory;
    }

    public LocationInstance FindNearestObjectOfType(Vector3 searchPosition, LocationType type) {
        LocationInstance locationInstance = null;
        List<LocationInstance> foundLocations = FindAllObjectsOfType(type);

        if (foundLocations.Count > 0) {
            float closestDistance = Mathf.Infinity;
            float currentDistance;

            for (int i = 0; i < foundLocations.Count; i++) {
                currentDistance = Vector3.Distance(searchPosition, foundLocations[i].GetAccessPoint());
                if (locationInstance == null || currentDistance < closestDistance) {
                    locationInstance = foundLocations[i];
                    closestDistance = currentDistance;
                }
            }
        }

        return locationInstance;
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

    public List<LocationInstance> FindAllObjectsOfType(LocationType type) {
        List<LocationInstance> foundLocations = new List<LocationInstance>();

        for (int i = 0; i < locations.Count; i++) {
            if (locations[i].GetLocationType().name == type.name) {
                foundLocations.Add(locations[i]);
            }
        }

        return foundLocations;
    }

    #endregion

    #endregion
}
