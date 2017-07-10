using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraBlackout : MonoBehaviour
{

    public float intensity;
    public float x1;
    public float y1;
    public float x2;
    public float y2;
    public float x3;
    public float y3;
    public float x4;
    public float y4;

    public bool enabled;
    private Material material;
    private Material depthBlit;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Unlit/CameraBlackout"));
        depthBlit = new Material(Shader.Find("Hidden/DepthCopy"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_bwBlend", intensity);
        material.SetFloat("_x1", x1);
        material.SetFloat("_y1", y1);
        material.SetFloat("_x2", x2);
        material.SetFloat("_y2", y2);
        material.SetFloat("_x3", x3);
        material.SetFloat("_y3", y3);
        material.SetFloat("_x4", x4);
        material.SetFloat("_y4", y4);
        if (enabled) Graphics.Blit(source, destination, material);
        else Graphics.Blit(source, destination);
    }
}