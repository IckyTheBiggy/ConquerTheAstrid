using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    [SerializeField] private float _timeToDestoryParticle;
    
    void Start()
    {
        StartCoroutine(DestoryParticleRoutine());
    }

    private IEnumerator DestoryParticleRoutine()
    {
        yield return new WaitForSeconds(_timeToDestoryParticle);
        Destroy(gameObject);
    }
}
