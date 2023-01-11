using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fume : MonoBehaviour
{
    private float spawnFrame;
    MeshRenderer meshrend;
    // Start is called before the first frame update
    void Awake()
    {
        meshrend = gameObject.GetComponentInChildren<MeshRenderer>();
        changeModeToFade();
        spawnFrame = Time.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 randomFloat = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        transform.position += (transform.TransformDirection(Vector3.forward) + GameManager.Instance.getWindDirection()) * Time.deltaTime;
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10f, Color.yellow);
        Color meshColor = meshrend.material.color;
        meshrend.material.color = new Color(meshColor.r, meshColor.g, meshColor.b, meshColor.a - 0.01f); //fume slowly becomes transparent

        if (Time.frameCount % 15 == 0) transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f); //
        if (Time.frameCount - spawnFrame >= 60) Destroy(this.gameObject); // gameObject will get destroyed after 60frames.
    }

    void changeModeToFade()
    {
        meshrend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshrend.material.SetFloat("_Mode", 2);
        meshrend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        meshrend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        meshrend.material.SetInt("_ZWrite", 0);
        meshrend.material.DisableKeyword("_ALPHATEST_ON");
        meshrend.material.EnableKeyword("_ALPHABLEND_ON");
        meshrend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        meshrend.material.renderQueue = 3000;
    }
}
