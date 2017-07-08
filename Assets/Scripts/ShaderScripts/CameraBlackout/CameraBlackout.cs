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
        Graphics.Blit(source, destination, material);
    }
}