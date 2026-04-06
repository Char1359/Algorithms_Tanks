using UnityEngine;


public enum TankColour : byte
{
    Unknown,
    Blue,
    Green,
    Pink,
    White
}

public enum TankType : byte
{
    Unknown,
    Human,
    AI
}

public class Tank : MonoBehaviour
{
    public delegate void TankIsImmobilied(Tank tank);
    public delegate void TankIsNoLongerImmobilied(Tank tank);

    public TankSettings settings;

    public GameObject turret;
    public Transform projectileSpawnTransform;

    public TankIsImmobilied TankIsImmobiliedCallback = null;
    public TankIsImmobilied TankIsNoLongerImmobiliedCallback = null;

    [SerializeField] private TankColour colour;
    private TankType type;

    private Barrel targetedBarrel;
    private Tank targetedTank;

    private Quaternion turretRotationOffset;

    private Rigidbody tankRigidbody;
    private Rigidbody turretRigidbody;

    private float projectileCooldown = 0.0f;
    private float immobilizedCooldown = 0.0f;
    private float immobilizedInterval = 0.0f;
    private float forwardMovement = 0.0f;
    private float tankRotation = 0.0f;
    private float turretRotation = 0.0f;
    private int hitCount = 0;

    private bool isExploded = false;
    private bool isFlashingRed = false;

    private const float kSphereCastRadius = 0.41f;

    public static string Key(Tank tank)
    {
        return tank.ToString();
    }

    public static string IsExplodedKey(Tank tank)
    {
        return tank.ToString() + "-IsExploded";
    }

    public static string IsImmobilizedKey(Tank tank)
    {
        return tank.ToString() + "-IsImmobilized";
    }

    public TankColour Colour
    {
        get { return colour; }
    }

    public TankType Type
    {
        get { if (type == TankType.Unknown) DetermineType();  return type; }
    }

    public float ForwardMovement
    {
        get { return forwardMovement; }
        set { forwardMovement = value; }
    }

    public float TankRotation
    {
        get { return tankRotation; }
        set { tankRotation = value; }
    }

    public float TurretRotation
    {
        get { return turretRotation; }
        set { turretRotation = value; }
    }

    public Barrel TargetedBarrel
    {
        get { return targetedBarrel; }
    }

    public Tank TargetedTank
    {
        get { return targetedTank; }
    }

    public bool CanFireProjectile
    {
        get { return projectileCooldown <= 0.0f; }
    }

    public bool IsImmobilized
    {
        get { return immobilizedCooldown > 0.0f; }
    }

    public bool IsExploded
    {
        get { return isExploded; }
    }

    public override string ToString()
    {
        return Colour.ToString() + "Tank";
    }

    // Start is called before the first frame update
    void Start()
    {
        tankRigidbody = GetComponent<Rigidbody>();
        turretRigidbody = turret.GetComponent<Rigidbody>();

        // Calculate the turret's starting offset, used when calculating the rotation when auto targetting
        float angle = turret.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        turretRotationOffset = Quaternion.Euler(0.0f, angle, 0.0f);

        DetermineType();
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown the projectile cooldown
        if (projectileCooldown > 0.0f)
        {
            projectileCooldown -= Time.deltaTime;
            if(projectileCooldown <= 0.0f)
            {
                projectileCooldown = 0.0f;
            }
        }

        // Countdown the immobilized cooldown
        if (immobilizedCooldown > 0.0f)
        {
            immobilizedCooldown -= Time.deltaTime;
            immobilizedInterval -= Time.deltaTime;

            if (immobilizedInterval <= 0.0f)
            {
                immobilizedInterval = settings.immobilizedFlashInterval;
                FlashRed(!isFlashingRed);
            }

            if (immobilizedCooldown <= 0.0f)
            {
                immobilizedCooldown = 0.0f;

                // Invoke the delegate
                if (TankIsNoLongerImmobiliedCallback != null)
                {
                    TankIsNoLongerImmobiliedCallback.Invoke(this);
                }
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.SphereCast(projectileSpawnTransform.position, kSphereCastRadius, projectileSpawnTransform.TransformDirection(Vector3.forward), out hit, 500.0f))
        {
            // Is the hit component a Barrel?
            if (hit.collider.gameObject.CompareTag("Barrel"))
            {
                // If a targetedBarrel is already targeted, disable the indicator
                if (targetedBarrel != null)
                {
                    targetedBarrel.destroyedCallback -= OnBarrelDestroyed;
                    targetedBarrel.DisableIndicator();
                    targetedBarrel = null;
                }

                // Get the targetedBarrel script and enable the indicator
                targetedBarrel = hit.collider.gameObject.GetComponentInParent<Barrel>();
                if (targetedBarrel != null)
                {
                    targetedBarrel.destroyedCallback += OnBarrelDestroyed;
                    targetedBarrel.EnableIndicator(GetIndicatorColor());
                }
            }
            else
            {
                // If a targetedBarrel is already targeted, disable the indicator
                if (targetedBarrel != null)
                {
                    targetedBarrel.destroyedCallback -= OnBarrelDestroyed;
                    targetedBarrel.DisableIndicator();
                    targetedBarrel = null;
                }
            }

            // Debug draw the ray cast to visualize where the turret is facing
            if (settings.drawProjectileRayCast)
            {
                Debug.DrawRay(projectileSpawnTransform.position, projectileSpawnTransform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            }
        }

        // Move the tank
        float forward = IsImmobilized ? 0.0f : forwardMovement;
        forward = IsExploded ? 0.0f : forward;
        forward = Game.Instance.IsGameOver ? 0.0f : forward;

        Vector3 position = GetComponent<Rigidbody>().position;
        Vector3 velocity = forward * transform.forward * settings.tankMovementSpeed;
        tankRigidbody.linearVelocity = velocity * Time.fixedDeltaTime;

        // Rotate the tank
        float rotation = IsImmobilized ? 0.0f : tankRotation;
        rotation = IsExploded ? 0.0f : rotation;
        rotation = Game.Instance.IsGameOver ? 0.0f : rotation;

        Vector3 tankAngularVelocity = Vector3.zero;
        tankAngularVelocity.y += rotation * settings.tankRotationSpeed;
        tankRigidbody.angularVelocity = tankAngularVelocity * Time.fixedDeltaTime;

        // Rotate the turret
        float turret = IsImmobilized ? 0.0f : turretRotation;
        turret = IsExploded ? 0.0f : turret;
        turret = Game.Instance.IsGameOver ? 0.0f : turret;

        Vector3 turretAngularVelocity = Vector3.zero;
        turretAngularVelocity.y += rotation * settings.tankRotationSpeed;
        turretAngularVelocity.y += turret * settings.turretRotationSpeed;
        turretRigidbody.angularVelocity = turretAngularVelocity * Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        // Lock on the target
        if (tankRotation == 0.0f && turretRotation == 0.0f)
        {
            if (targetedBarrel != null)
            {
                Target(targetedBarrel.GetCenter());
            }
            else if (targetedTank != null)
            {
                Target(targetedTank.transform.position);
            }
        }
    }

    public bool FireProjectile()
    {
        if (Game.Instance.IsGameOver == false)
        {
            if (CanFireProjectile && isExploded == false)
            {
                Game.Instance.ProjectileObjectPool.SpawnGameObject(projectileSpawnTransform.position, projectileSpawnTransform.rotation);
                projectileCooldown = settings.projectileCooldownDuration;
                return true;
            }
        }

        return false;
    }

    public void OnProjectileHit()
    {
        if (IsImmobilized == false)
        {
            hitCount++;

            if (hitCount >= settings.numberOfHitsBeforeBeingImmobilized)
            {
                hitCount = 0;
                immobilizedCooldown = settings.immobilizedDuration;
                immobilizedInterval = settings.immobilizedFlashInterval;

                // Invoke the delegate
                if (TankIsImmobiliedCallback != null)
                {
                    TankIsImmobiliedCallback.Invoke(this);
                }
            }
        }
    }

    private void Target(Vector3 location)
    {
        Vector3 turretLocation = turret.transform.position;
        Vector3 target = location;

        target.y = turretLocation.y;

        Vector3 delta1 = (target - turretLocation).normalized;
        turret.transform.rotation = Quaternion.LookRotation(delta1) * turretRotationOffset;
    }

    public void Explode()
    {
        if (isExploded == false)
        {
            isExploded = true;

            // Spawn a big explosion
            Game.Instance.ExplosionObjectPool.SpawnGameObject(transform.position + new Vector3(0.0f, 3.75f, 0.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));

            Invoke("DelayedExplosionOne", 0.75f);
            Invoke("DelayedExplosionTwo", 1.1f);
            Invoke("DelayedExplosionThree", 1.45f);

            // Delay calling the change materials method
            Invoke("ChangeMaterials", 1.15f);
        }
    }

    private void DelayedExplosionOne()
    {
        Game.Instance.ExplosionObjectPool.SpawnGameObject(transform.position + new Vector3(5.0f, 3.75f, -3.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
    }

    private void DelayedExplosionTwo()
    {
        Game.Instance.ExplosionObjectPool.SpawnGameObject(transform.position + new Vector3(-2.0f, 3.75f, 4.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
    }

    private void DelayedExplosionThree()
    {
        Game.Instance.ExplosionObjectPool.SpawnGameObject(transform.position + new Vector3(1.0f, 3.75f, 1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
    }

    private void OnBarrelDestroyed(Barrel barrel)
    { 
        if(targetedBarrel == barrel)
        {
            targetedBarrel.destroyedCallback -= OnBarrelDestroyed;
            targetedBarrel = null;
        }
    }

    private void FlashRed(bool red)
    {
        isFlashingRed = red;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            Material[] materials = meshRenderer.materials;

            foreach (Material material in materials)
            {
                if (isFlashingRed)
                {
                    if (material.color == Color.limeGreen || material.color == Color.lightSlateBlue || material.color == Color.hotPink || material.color == Color.white)
                        material.color = Color.red;
                }
                else
                {
                    if (material.color == Color.red)
                    {
                        if (colour == TankColour.Blue)
                            material.color = Color.lightSlateBlue;
                        else if (colour == TankColour.Green)
                            material.color = Color.limeGreen;
                        else if (colour == TankColour.Pink)
                            material.color = Color.hotPink;
                        else if (colour == TankColour.White)
                            material.color = Color.white;
                    }
                }
            }
        }
    }

    private void ChangeMaterials()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            Material[] materials = meshRenderer.materials;

            foreach (Material material in materials)
            {
                material.color = Color.black;
            }
        }


        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            Material[] materials = skinnedMeshRenderer.materials;

            foreach (Material material in materials)
            {
                material.color = Color.black;
            }
        }
    }

    private Color GetIndicatorColor()
    {
        if(colour == TankColour.Blue)
        {
            return Color.lightSlateBlue;
        }
        else if (colour == TankColour.Green)
        {
            return Color.limeGreen;
        }
        else if (colour == TankColour.Pink)
        {
            return Color.deepPink;
        }
        else if (colour == TankColour.White)
        {
            return Color.white;
        }

        return Color.darkGray;
    }

    private void DetermineType()
    {
        // Determine the type based on the controller attached
        if (GetComponent<TankController>() != null)
        {
            type = TankType.Human;
        }
        else if (GetComponent<AIController>() != null)
        {
            type = TankType.AI;
        }
        else
        {
            type = TankType.Unknown;
        }
    }
}
