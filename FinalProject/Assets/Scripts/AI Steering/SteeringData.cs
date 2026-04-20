using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteeringData
{
    private List<Collider> targets = new List<Collider>();
    private List<Collider> obstacles = new List<Collider>();
    private List<Collider> barrels = new List<Collider>();
    private List<Collider> tanks = new List<Collider>();
    private List<Collider> detonators = new List<Collider>();

    private Transform currentTarget = null;
    private Transform currentBarrel = null;
    private Transform currentTank = null;

    private Vector3? wanderLocation = null;
    private SteeringSettings settings = null;

    public List<Collider> Targets
    {
        get { return targets; }
        set { targets = value; }
    }

    public List<Collider> Obstacles
    {
        get { return obstacles; }
        set { obstacles = value; }
    }

    public List<Collider> Barrels
    {
        get { return barrels; }
        set { barrels = value; }
    }

    public List<Collider> Tanks
    {
        get { return tanks; }
        set { tanks = value; }
    }

    public List<Collider> Detonators
    {
        get { return detonators; }
        set { detonators = value; }
    }

    public Transform CurrentTarget
    {
        get { return currentTarget; }
        set { currentTarget = value; }
    }

    public Transform CurrentBarrel
    {
        get { return currentBarrel; }
        set { currentBarrel = value; }
    }

    public Transform CurrentTank
    {
        get { return currentTank; }
        set { currentTank = value; }
    }

    public Vector3? WanderLocation
    {
        get { return wanderLocation; }
        set { wanderLocation = value; }
    }

    public SteeringSettings Settings
    {
        get { return settings; }
    }

    public SteeringData(SteeringSettings settings)
    {
        this.settings = settings;
    }

    public void Reset()
    {
        targets.Clear();
        obstacles.Clear();
        barrels.Clear();
        tanks.Clear();
        detonators.Clear();
    }

    public Collider GetClosestTarget(Vector3 position)
    {
        return Targets.OrderBy(collider => Vector3.Distance(collider.transform.position, position)).FirstOrDefault();
    }

    public Collider GetClosestBarrel(Vector3 position)
    {
        return Barrels.OrderBy(collider => Vector3.Distance(collider.transform.position, position)).FirstOrDefault();
    }

    public Collider GetClosestTank(Vector3 position)
    {
        return Tanks.OrderBy(collider => Vector3.Distance(collider.transform.position, position)).FirstOrDefault();
    }

    public Collider GetClosestDetonator(Vector3 position)
    {
        return Detonators.OrderBy(collider => Vector3.Distance(collider.transform.position, position)).FirstOrDefault();
    }

    public bool IsTargetBitEnabled(int layer)
    {
        return ((1 << layer) & settings.targetMask) != 0;
    }

    public bool IsObstacleBitEnabled(int layer)
    {
        return ((1 << layer) & settings.obstacleMask) != 0;
    }
}
