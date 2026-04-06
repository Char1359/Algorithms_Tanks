using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    [SerializeField] private TankColour colour;

    public static string IsSpawnedKey(TankColour colour)
    {
        return colour.ToString() + "Detonator-IsSpawned";
    }

    public static string Key(TankColour colour)
    {
        return colour.ToString() + "Detonator";
    }

    public TankColour Colour
    {
        get { return colour; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
            Game.Instance.ExplodeTank(colour);
        }
    }
}
