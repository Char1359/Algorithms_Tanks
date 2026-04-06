using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankSettings", menuName = "Scriptable Objects/Tank Settings")]
public class TankSettings : ScriptableObject
{
    [Header("Movement")]
    public float tankMovementSpeed = 500.0f;
    public float tankRotationSpeed = 25.0f;

    [Header("Turret")]
    public float turretRotationSpeed = 45.0f;

    [Header("Immobilized")]
    public int numberOfHitsBeforeBeingImmobilized = 3;
    public float immobilizedDuration = 2.5f;
    public float immobilizedFlashInterval = 0.2f;

    [Header("Projectile")]
    public float projectileCooldownDuration = 0.4f;

    [Header("Debug")]
    public bool drawProjectileRayCast = false;
}
