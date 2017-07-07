using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_TransitionTiles : Tiled2Unity.ICustomTiledImporter
{
    string markerTag = "TransitionMarker";
    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> customProperties)
    {
        if (customProperties.ContainsKey("Transition"))
        {
            Debug.Log("MakingTransition");
            // Add the terrain tile game object
            TransitionMarker marker = gameObject.AddComponent<TransitionMarker>();
            marker.markerKey = customProperties["Transition"];

            marker.tag = markerTag;
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        prefab.layer = LayerMask.NameToLayer("Border");
        EdgeCollider2D collider = prefab.AddComponent<EdgeCollider2D>();
        var size = myMath.getTiledMapSize(prefab.GetComponent<Tiled2Unity.TiledMap>());

        List<Vector2> newVerticies = new List<Vector2>();
        newVerticies.Add(new Vector2(0f, 0f));
        newVerticies.Add(new Vector2(size.x, 0f));
        newVerticies.Add(new Vector2(size.x, -size.y));
        newVerticies.Add(new Vector2(0f, -size.y));
        newVerticies.Add(new Vector2(0f, 0f));
        collider.points = newVerticies.ToArray();

        // Do nothing
    }
}