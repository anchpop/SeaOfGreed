using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraBlackout : MonoBehaviour
{

    public float intensity;
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
        Graphics.Blit(source, destination, material);
    }
}