using UnityEngine;
using System.Collections;

class myMath
{
    public static float parabolicScaleCalc(float time, float scale)
    {
        return scale * (-Mathf.Pow(time, 2) + 1);
    }

    public static Vector2 getTiledMapSize(Tiled2Unity.TiledMap map)
    {
        var width = map.NumTilesWide * map.TileWidth * map.ExportScale;
        var height = map.NumTilesHigh * map.TileHeight * map.ExportScale;
        return new Vector2(width, height);
    }
}