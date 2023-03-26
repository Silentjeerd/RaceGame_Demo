using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fume : MonoBehaviour
{
    private float spawnFrame;
    MeshRenderer meshRenderer;

    /// <summary>
    /// Sets the meshRenderer, changes to Renderingmode.
    /// Sets the spawnFrame to its instantiation frame.
    /// </summary>
    void Awake()
    {
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        changeModeToFade();
        spawnFrame = Time.frameCount;
    }

    /// <summary>
    /// Adjusts the position of the GameObject based on its direction and the wind direction.
    /// Slowly decreases the alpha channel of the color every update to make it fade away.
    /// Slowly decreases the scale of the gameobject in addition to the fade away.
    /// After 60 frames the object will be destroyed.
    /// </summary>
    void Update()
    {
        transform.position += (transform.TransformDirection(Vector3.forward) + GameManager.Instance.getWindDirection()) * Time.deltaTime;
        Color meshColor = meshRenderer.material.color;
        meshRenderer.material.color = new Color(meshColor.r, meshColor.g, meshColor.b, meshColor.a - 0.01f); //fume slowly becomes transparent

        if (Time.frameCount % 15 == 0) transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        if (Time.frameCount - spawnFrame >= 60) Destroy(this.gameObject);
    }

    /// <summary>
    /// Sets material values so it is possible to make the object fadeaway.
    /// </summary>
    void changeModeToFade()
    {
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.material.SetFloat("_Mode", 2);
        meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        meshRenderer.material.SetInt("_ZWrite", 0);
        meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
        meshRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        meshRenderer.material.renderQueue = 3000;
    }
}
