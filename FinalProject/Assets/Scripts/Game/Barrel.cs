using UnityEngine;


public enum BarrelContents : byte
{
    Empty,
    BlueDetonator,
    GreenDetonator,
    PinkDetonator,
    WhiteDetonator
}

public class Barrel : MonoBehaviour
{
    public Indicator indicator;

    public delegate void BarrelDestroyed(Barrel barrel);
    public BarrelDestroyed destroyedCallback = null;

    private BarrelContents contents = BarrelContents.Empty;
    private MeshRenderer meshRenderer;


    public BarrelContents Contents
    {
        get { return contents; }
        set { contents = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        DisableIndicator();
    }

    public Vector3 GetCenter()
    {
        Vector3 center = transform.position;

        if (meshRenderer != null)
        {
            center = meshRenderer.bounds.center;
        }

        return center;
    }

    public void EnableIndicator(Color color)
    {
        indicator.SetColor(color);
        indicator.Enable();
    }

    public void DisableIndicator()
    {
        indicator.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Spawn an explosion
            float ratio = GetCenter().z / 50.0f;
            Game.Instance.ExplosionObjectPool.SpawnGameObject(GetCenter() + new Vector3(0.4f, 0.0f, 1.1f + ratio * 0.4f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));

            // Spawn a detonator if the barrel contained one
            if (contents == BarrelContents.BlueDetonator)
            {
                Game.Instance.SpawnDetonator(transform.position, TankColour.Blue);
            }
            else if (contents == BarrelContents.GreenDetonator)
            {
                Game.Instance.SpawnDetonator(transform.position, TankColour.Green);
            }
            else if (contents == BarrelContents.PinkDetonator)
            {
                Game.Instance.SpawnDetonator(transform.position, TankColour.Pink);
            }
            else if (contents == BarrelContents.WhiteDetonator)
            {
                Game.Instance.SpawnDetonator(transform.position, TankColour.White);
            }

            // Invoke the barrel destroyed callback
            if (destroyedCallback != null)
            {
                destroyedCallback.Invoke(this);
            }

            // Remove the barrel from the game's Barrel list
            Game.Instance.BarrelDestroyed(this);

            // Release the barrel back to the object pool
            Game.Instance.BarrelObjectPool.ReleaseGameObject(gameObject);

            // Release the projectile back to the object pool
            Game.Instance.ProjectileObjectPool.ReleaseGameObject(collision.gameObject);
        }
    }
}
