using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_TransitionTiles : Tiled2Unity.ICustomTiledImporter
{
    string markerTag = "MapImportObject";
    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> customProperties)
    {
        if (customProperties.ContainsKey("Transition"))
        {
            Debug.Log("MakingTransition");

            TransitionMarker marker = gameObject.AddComponent<TransitionMarker>();
            marker.markerKey = customProperties["Transition"];

            marker.tag = markerTag;
        }
        else if (customProperties.ContainsKey("Character name"))
        {
            Debug.Log("adding character " + customProperties["Character name"]);

            var marker = gameObject.AddComponent<SpawnMarkers.CharacterSpawnMarker>();
            marker.characterName = customProperties["Character name"];
            gameObject.transform.position += new Vector3(.5f, -.5f); // IMPORTANT! This assumes that tiles are 1 unit by 1 unit! 

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