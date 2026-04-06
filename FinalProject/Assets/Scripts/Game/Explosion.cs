using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IPoolableObject
{
    private static readonly string[] kStates = { "Explosion00", "Explosion01", "Explosion02" };

    public void OnRelease()
    {
        
    }

    public void OnSpawn()
    {
        int index = Random.Range(0, kStates.Length);
        GetComponent<Animator>().Play(kStates[index]);
    }

    void OnAnimationFinished()
    {
        Game.Instance.ExplosionObjectPool.ReleaseGameObject(gameObject);
    }
}
