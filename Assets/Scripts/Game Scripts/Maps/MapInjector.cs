using System.Collections.Generic;
using UnityEngine;

public class MapInjector
{
    public Map FindAndInjectObject(Vector3 position, Object obj) {
        Map map = null;

        Collider[] hits = Physics.OverlapSphere(position, 0.1f, LayerMask.GetMask("Maps"));

        if (hits.Length > 0) {
            map = hits[0].GetComponent<Map>();

            if (map != null) {
                map.RegisterObject(obj);
            }
        }

        return map;
    }
}
