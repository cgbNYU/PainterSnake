using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatParticles : MonoBehaviour
{
    //Public
    public ParticleSystem SplatParticleSystem;
    public GameObject SplatPrefab;
    private Transform _splatHolder;
    
    //Private
    private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

    private void Start()
    {
        _splatHolder = GameObject.Find("SplatHolder").transform;
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(SplatParticleSystem, other, _collisionEvents);

        int count = _collisionEvents.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject splat = Instantiate(SplatPrefab, _collisionEvents[i].intersection, Quaternion.identity) as GameObject;
            splat.transform.SetParent(_splatHolder, true);
            Splat splatScript = splat.GetComponent<Splat>();
            splatScript.Initialize();
        }
    }
}
