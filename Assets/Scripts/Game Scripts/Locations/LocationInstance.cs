using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInstance : MonoBehaviour
{
    [SerializeField] LocationType locationType;
    [SerializeField] Transform accessPoint;

    MapInjector mapInjector = new MapInjector();

    private void Awake() {
        mapInjector.FindAndInjectObject(transform.position, this);
    }

    public Vector3 GetAccessPoint() {
        if (accessPoint != null) {
            return accessPoint.position;
        }
        else {
            return transform.position;
        }
    }

    public LocationType GetLocationType() {
        return locationType;
    }
}
