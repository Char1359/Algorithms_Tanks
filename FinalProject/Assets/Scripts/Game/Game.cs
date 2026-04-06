using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Rendering;

public class Game : MonoBehaviour
{
    private static Game sInstance;

    [Header("Behavior Graph Agents")]
    public BehaviorGraphAgent greenTankBehaviourAgent;
    public BehaviorGraphAgent blueTankBehaviourAgent;
    public BehaviorGraphAgent pinkTankBehaviourAgent;
    public BehaviorGraphAgent whiteTankBehaviourAgent;

    [Header("Tanks")]
    public Tank BlueTank;
    public Tank GreenTank;
    public Tank PinkTank;
    public Tank WhiteTank;

    [Header("Object pools")]
    public ObjectPool ProjectileObjectPool;
    public ObjectPool BarrelObjectPool;
    public ObjectPool ExplosionObjectPool;

    [Header("Detonator prefabs")]
    public GameObject GreenDetonatorPrefab;
    public GameObject BlueDetonatorPrefab;
    public GameObject PinkDetonatorPrefab;
    public GameObject WhiteDetonatorPrefab;

    public enum GameMode
    {
        OneAI,
        TwoAI,
        ThreeAI,
        FourAI
    }

    [Header("Number of AI")]
    public GameMode gameMode;

    private List<Barrel> barrels = new List<Barrel>();
    private List<TankColour> tanksRemaining = new List<TankColour>();
    private bool isGameOver = false;

    struct DetonatorIndex
    {
        public DetonatorIndex(BarrelContents contents, Barrel barrel)
        {
            this.contents = contents;
            this.barrel = barrel;
        }

        public BarrelContents contents;
        public Barrel barrel;
    }

    private List<DetonatorIndex> detonatorIndices = new List<DetonatorIndex>();

    public static Game Instance
    {
        get { return sInstance; }
    }

    public List<Barrel> Barrels
    {
        get { return barrels; }
    }

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup the static instance of the Game class
        if (sInstance != null && sInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sInstance = this;
        }

        // Spawn the barrels from the placeholder tag
        BarrelObjectPool.SpawnFromPlaceholder("Barrel-Placeholder");

        // Loop through the active barrel game objects and add them to the barrels list
        foreach (GameObject barrelObject in BarrelObjectPool.ActiveGameObjects)
        {
            barrels.Add(barrelObject.GetComponent<Barrel>());
        }

        // Register for the tank callbacks
        BlueTank.TankIsImmobiliedCallback += OnTankIsImmobilied;
        BlueTank.TankIsNoLongerImmobiliedCallback += OnTankIsNoLongerImmobilied;
        GreenTank.TankIsImmobiliedCallback += OnTankIsImmobilied;
        GreenTank.TankIsNoLongerImmobiliedCallback += OnTankIsNoLongerImmobilied;

        if (PinkTank != null)
        {
            PinkTank.TankIsImmobiliedCallback += OnTankIsImmobilied;
            PinkTank.TankIsNoLongerImmobiliedCallback += OnTankIsNoLongerImmobilied;
        }

        if (WhiteTank != null)
        {
            WhiteTank.TankIsImmobiliedCallback += OnTankIsImmobilied;
            WhiteTank.TankIsNoLongerImmobiliedCallback += OnTankIsNoLongerImmobilied;
        }

        List<int> indices = new List<int>();
        for(int i = 0; i < Barrels.Count; i++)
        {
            indices.Add(i);
        }

        // Get a random index for the blue detonator
        int index = Random.Range(0, indices.Count);
        int blueIndex = indices[index];
        indices.Remove(index);

        Barrels[blueIndex].Contents = BarrelContents.BlueDetonator;
        detonatorIndices.Add(new DetonatorIndex(BarrelContents.BlueDetonator, Barrels[blueIndex]));
        tanksRemaining.Add(TankColour.Blue);
        SetBlackboardTankKey(BlueTank);

        // Get a random index for the green detonator
        index = Random.Range(0, indices.Count);
        int greenIndex = indices[index];
        indices.Remove(index);

        Barrels[greenIndex].Contents = BarrelContents.GreenDetonator;
        detonatorIndices.Add(new DetonatorIndex(BarrelContents.GreenDetonator, Barrels[greenIndex]));
        tanksRemaining.Add(TankColour.Green);
        SetBlackboardTankKey(GreenTank);

        // Is this a 3 or 4 player game mode?
        if (gameMode == GameMode.ThreeAI || gameMode == GameMode.FourAI)
        {
            // Get a random index for the pink detonator
            index = Random.Range(0, indices.Count);
            int pinkIndex = indices[index];
            indices.Remove(index);

            detonatorIndices.Add(new DetonatorIndex(BarrelContents.PinkDetonator, Barrels[pinkIndex]));
            Barrels[pinkIndex].Contents = BarrelContents.PinkDetonator;
            tanksRemaining.Add(TankColour.Pink);
            SetBlackboardTankKey(PinkTank);
        }

        // Is this a 4 player game mode?
        if (gameMode == GameMode.FourAI)
        {
            // Get a random index for the pink detonator
            index = Random.Range(0, indices.Count);
            int whiteIndex = indices[index];
            indices.Remove(index);

            detonatorIndices.Add(new DetonatorIndex(BarrelContents.WhiteDetonator, Barrels[whiteIndex]));
            Barrels[whiteIndex].Contents = BarrelContents.WhiteDetonator;
            tanksRemaining.Add(TankColour.White);
            SetBlackboardTankKey(WhiteTank);
        }
    }

    public void BarrelDestroyed(Barrel barrel)
    {
        for (int i = 0; i < detonatorIndices.Count; i++)
        {
            if (detonatorIndices[i].barrel == barrel)
            {
                detonatorIndices.RemoveAt(i);
                break;
            }
        }

        barrels.Remove(barrel);
    }

    public void SpawnDetonator(Vector3 position, TankColour colour)
    {
        position.y = 0.0f;

        if (colour == TankColour.Blue && BlueDetonatorPrefab != null)
        {
            GameObject blueDetonator = Instantiate(BlueDetonatorPrefab, position, Quaternion.identity);
            SetBlackboardDetonatorHasSpawnedKey(blueDetonator, colour);
        }
        else if (colour == TankColour.Green && GreenDetonatorPrefab != null)
        {
            GameObject greenDetonator = Instantiate(GreenDetonatorPrefab, position, Quaternion.identity);
            SetBlackboardDetonatorHasSpawnedKey(greenDetonator, colour);
        }
        else if (colour == TankColour.Pink && PinkDetonatorPrefab != null)
        {
            GameObject pinkDetonator = Instantiate(PinkDetonatorPrefab, position, Quaternion.identity);
            SetBlackboardDetonatorHasSpawnedKey(pinkDetonator, colour);
        }
        else if (colour == TankColour.White && WhiteDetonatorPrefab != null)
        {
            GameObject whiteDetonator = Instantiate(WhiteDetonatorPrefab, position, Quaternion.identity);
            SetBlackboardDetonatorHasSpawnedKey(whiteDetonator, colour);
        }
    }

    public void ExplodeTank(TankColour colour)
    {
        if (colour == TankColour.Blue && BlueTank.IsExploded == false)
        {
            tanksRemaining.Remove(TankColour.Blue);
            BlueTank.Explode();
            SetBlackboardTankHasExplodedKey(BlueTank);
        }
        else if (colour == TankColour.Green && GreenTank.IsExploded == false)
        {
            tanksRemaining.Remove(TankColour.Green);
            GreenTank.Explode();
            SetBlackboardTankHasExplodedKey(GreenTank);
        }
        else if (colour == TankColour.Pink && PinkTank.IsExploded == false)
        {
            tanksRemaining.Remove(TankColour.Pink);
            PinkTank.Explode();
            SetBlackboardTankHasExplodedKey(PinkTank);
        }
        else if (colour == TankColour.White && WhiteTank.IsExploded == false)
        {
            tanksRemaining.Remove(TankColour.White);
            WhiteTank.Explode();
            SetBlackboardTankHasExplodedKey(WhiteTank);
        }

        // There's only one tank remaining, the game is over
        if(tanksRemaining.Count == 1)
        {
            Debug.Log(tanksRemaining[0] + " wins!");
            Invoke("DelayGameOver", 1.0f);
            greenTankBehaviourAgent.SetVariableValue<bool>("IsGameOver", true);

            if (blueTankBehaviourAgent != null)
            {
                blueTankBehaviourAgent.SetVariableValue<bool>("IsGameOver", true);
            }

            if (pinkTankBehaviourAgent != null)
            {
                pinkTankBehaviourAgent.SetVariableValue<bool>("IsGameOver", true);
            }

            if (whiteTankBehaviourAgent != null)
            {
                whiteTankBehaviourAgent.SetVariableValue<bool>("IsGameOver", true);
            }
        }
    }

    private void DelayGameOver()
    {
        isGameOver = true;
    }

    private void SetBlackboardTankKey(Tank tank)
    {
        TankColour colour = tank.Colour;

        if(colour == TankColour.Green)
        {
            if (blueTankBehaviourAgent != null)
            {
                blueTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            }

            if (pinkTankBehaviourAgent != null)
            {
                pinkTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            }

            if (whiteTankBehaviourAgent != null)
            {
                whiteTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            }
        }
        else if (colour == TankColour.Blue)
        {
            greenTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);

            if (pinkTankBehaviourAgent != null)
            {
                pinkTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            }

            if (whiteTankBehaviourAgent != null)
            {
                whiteTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            }
        }
        else if (colour == TankColour.Pink)
        {
            greenTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            blueTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);

            if (whiteTankBehaviourAgent != null)
            {
                whiteTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            }
        }
        else if (colour == TankColour.White)
        {
            greenTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            blueTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
            pinkTankBehaviourAgent.SetVariableValue<GameObject>(Tank.Key(tank), tank.gameObject);
        }
    }

    private void SetBlackboardDetonatorHasSpawnedKey(GameObject gameObject, TankColour colour)
    {
        greenTankBehaviourAgent.SetVariableValue<GameObject>(Detonator.Key(colour), gameObject);
        greenTankBehaviourAgent.SetVariableValue<bool>(Detonator.IsSpawnedKey(colour), true);

        if (blueTankBehaviourAgent != null)
        {
            blueTankBehaviourAgent.SetVariableValue<GameObject>(Detonator.Key(colour), gameObject);
            blueTankBehaviourAgent.SetVariableValue<bool>(Detonator.IsSpawnedKey(colour), true);
        }

        if (pinkTankBehaviourAgent != null)
        {
            pinkTankBehaviourAgent.SetVariableValue<GameObject>(Detonator.Key(colour), gameObject);
            pinkTankBehaviourAgent.SetVariableValue<bool>(Detonator.IsSpawnedKey(colour), true);
        }

        if (whiteTankBehaviourAgent != null)
        {
            whiteTankBehaviourAgent.SetVariableValue<GameObject>(Detonator.Key(colour), gameObject);
            whiteTankBehaviourAgent.SetVariableValue<bool>(Detonator.IsSpawnedKey(colour), true);
        }
    }

    private void SetBlackboardTankHasExplodedKey(Tank tank)
    {
        greenTankBehaviourAgent.SetVariableValue<bool>(Tank.IsExplodedKey(tank), true);

        if (blueTankBehaviourAgent != null)
        {
            blueTankBehaviourAgent.SetVariableValue<bool>(Tank.IsExplodedKey(tank), true);
        }

        if (pinkTankBehaviourAgent != null)
        {
            pinkTankBehaviourAgent.SetVariableValue<bool>(Tank.IsExplodedKey(tank), true);
        }

        if (whiteTankBehaviourAgent != null)
        {
            whiteTankBehaviourAgent.SetVariableValue<bool>(Tank.IsExplodedKey(tank), true);
        }
    }

    private void OnTankIsImmobilied(Tank tank)
    {
        greenTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), true);

        if (blueTankBehaviourAgent != null)
        {
            blueTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), true);
        }

        if (pinkTankBehaviourAgent != null)
        {
            pinkTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), true);
        }

        if (whiteTankBehaviourAgent != null)
        {
            whiteTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), true);
        }
    }

    private void OnTankIsNoLongerImmobilied(Tank tank)
    {
        greenTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), false);

        if (blueTankBehaviourAgent != null)
        {
            blueTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), false);
        }

        if (pinkTankBehaviourAgent != null)
        {
            pinkTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), false);
        }

        if (whiteTankBehaviourAgent != null)
        {
            whiteTankBehaviourAgent.SetVariableValue<bool>(Tank.IsImmobilizedKey(tank), false);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 position = Vector3.zero;

        foreach (DetonatorIndex detonatorIndex in detonatorIndices)
        {
            if(detonatorIndex.contents == BarrelContents.BlueDetonator)
            {
                Gizmos.color = Color.lightSlateBlue;
            }
            else if (detonatorIndex.contents == BarrelContents.GreenDetonator)
            {
                Gizmos.color = Color.limeGreen;
            }
            else if (detonatorIndex.contents == BarrelContents.PinkDetonator)
            {
                Gizmos.color = Color.hotPink;
            }
            else if (detonatorIndex.contents == BarrelContents.WhiteDetonator)
            {
                Gizmos.color = Color.white;
            }

            position = detonatorIndex.barrel.GetCenter() + new Vector3(0.0f, 3.5f, 0.0f);
            Gizmos.DrawSphere(position, 1.5f);
        }
    }
}
