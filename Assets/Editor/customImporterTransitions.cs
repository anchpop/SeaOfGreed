using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;

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
        else if (customProperties.ContainsKey("Place Enviromental"))
        {
            var rectToPlaceGrassIn = gameObject.AddComponent<Tiled2Unity.RectangleObject>();
            Debug.Log("placing grass at " + rectToPlaceGrassIn.transform.position);

            int numToPlace = 3;
            if (customProperties.ContainsKey("Number to place")) int.TryParse(customProperties["Number to place"], out numToPlace);

            int chance = 1;
            if (customProperties.ContainsKey("Chance")) int.TryParse(customProperties["Chance"], out chance);

            for (int i = 0; i < numToPlace; i++)
            {
                if (UnityEngine.Random.Range(0, 1) <= chance)
                {
                    GameObject enviromental = new GameObject(customProperties["Place Enviromental"] + " " + i);
                    enviromental.transform.SetParent(rectToPlaceGrassIn.transform, false);
                    enviromental.transform.localPosition = Vector3.zero + Vector3.right * UnityEngine.Random.Range(0f, 1f) + -Vector3.up * UnityEngine.Random.Range(0f, 1f);
                    var renderer = enviromental.AddComponent<SpriteRenderer>();
                    renderer.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(customProperties["Place Enviromental"], typeof(Sprite));
                    renderer.sortingLayerName = "Characters";
                    renderer.sortingOrder = myMath.floatToSortingOrder(enviromental.transform.position.y);
                    enviromental.layer = LayerMask.NameToLayer("mainCameraOnly");
                }
            }
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