using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustFumes : MonoBehaviour
{

    [SerializeField] private GameObject[] particles; //A list of usable sprites for the fumes, set in the UnityEditor.
    [SerializeField] private Transform particleSpawn; //The spawnposition of the fumes.
    [SerializeField] private Rigidbody carBody; //Body of the car, used to get its speed.

    /// <summary>
    /// Spawns fumes based on the speed of the car.
    /// </summary>
    void FixedUpdate()
    {
        int fumeCount = (int)carBody.velocity.magnitude + 1;
        for (int i = 0; i < fumeCount; i++)
        {
            spawnFumes();
        }
    }

    /// <summary>
    /// Picks a random particle from the list and adjusts the scale.
    /// Instantiates the gameobject, adds the Fume script and detaches it from the spawnpoint.
    /// </summary>
    private void spawnFumes()
    {
        int randomParticleIndex = Random.Range(0, particles.Length - 1);
        GameObject particle = particles[randomParticleIndex];
        particle.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);

        GameObject fuming = Instantiate(particle, particleSpawn);
        fuming.AddComponent<Fume>();
        transform.DetachChildren();
    }
}
