using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CernalSphere : MonoBehaviour
{
    public UnityEvent getEnemyEvent;
    public UnityEvent getParticleEvent;

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.GetComponent<Enemy>())
            getEnemyEvent?.Invoke();

        if (collision.gameObject.GetComponent<Particle>())
            getParticleEvent?.Invoke();

        if (collision.gameObject.GetComponent<Destroyable>())
            collision.gameObject.GetComponent<Destroyable>().Destroy();


    }
}
