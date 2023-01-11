using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustFumes : MonoBehaviour
{

    [SerializeField]
    private GameObject[] particles;
    [SerializeField]
    private Transform particleSpawn;
    [SerializeField]
    private int fumeCount = 10;
    [SerializeField]
    private Rigidbody carBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fumeCount = (int)carBody.velocity.magnitude + 1;
        if (fumeCount != 0 && Time.frameCount % fumeCount != 0)
        {
            for (int i = 0; i < fumeCount; i++)
            {
                spawnFumes();
            }
        }
    }

    private void spawnFumes()
    {
        int randomParticleIndex = Random.Range(0, particles.Length - 1);
        GameObject particle = particles[randomParticleIndex];
        particle.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
       
        var fuming = Instantiate(particle, particleSpawn); //spawn fume.
        fuming.AddComponent<Fume>(); //add fumeMovescript.
        transform.DetachChildren(); //detach from parents transform.
    }
}
