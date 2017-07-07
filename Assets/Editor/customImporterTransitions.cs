using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        // Do nothing
    }
}