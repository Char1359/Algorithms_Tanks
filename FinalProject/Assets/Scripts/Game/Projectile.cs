using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float kSpeed = 125.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        Vector3 velocity = transform.forward * kSpeed;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boundary") || collision.gameObject.CompareTag("Tank") || collision.gameObject.CompareTag("Projectile"))
        {
            if(collision.gameObject.CompareTag("Tank"))
            {
                collision.gameObject.GetComponentInParent<Tank>().OnProjectileHit();
            }

            Vector3 contactPoint = collision.contacts[0].point;
            Game.Instance.ExplosionObjectPool.SpawnGameObject(contactPoint + new Vector3(0.0f, 1.25f, 0.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
            Game.Instance.ProjectileObjectPool.ReleaseGameObject(gameObject);
        }
    }
}
