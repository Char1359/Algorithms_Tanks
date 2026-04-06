using System;
using System.Collections.Generic;
using UnityEngine;


public interface IPoolableObject
{
    void OnSpawn();
    void OnRelease();
}

public class ObjectPool : MonoBehaviour
{
    public Action<GameObject> OnSpawned;
    public Action<GameObject> OnReleased;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int initialPoolSize = 10;

    private List<GameObject> activeGameObjects = new List<GameObject>();
    private List<GameObject> inactiveGameObjects = new List<GameObject>();

    public List<GameObject> ActiveGameObjects
    {
        get { return activeGameObjects; }
    }

    public List<GameObject> InactiveGameObjects
    {
        get { return inactiveGameObjects; }
    }

    public void SpawnFromPlaceholder(string placeholderTag)
    {
        // Find all the GameObjects with the placeholderTag
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(placeholderTag);

        // Local variables
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        // Loop through the found GameObjects
        for (int i = 0; i < gameObjects.Length; i++)
        {
            // Get the position and rotation of the placeholder GameObject
            position = gameObjects[i].transform.position;
            rotation = gameObjects[i].transform.rotation;

            // Destroy the placeholder GameObject
            Destroy(gameObjects[i]);

            // Spawn the GameObject at the location and rotation
            SpawnGameObject(position, rotation);
        }
    }

    public GameObject SpawnGameObject(Vector3 position)
    {
        return SpawnGameObject(position, Quaternion.identity);
    }

    public GameObject SpawnGameObject(Vector3 position, Quaternion rotation)
    {
        GameObject gameObject = GetObjectFromPool();

        if (gameObject != null)
        {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            gameObject.SetActive(true);

            inactiveGameObjects.Remove(gameObject);
            activeGameObjects.Add(gameObject);

            OnSpawned?.Invoke(gameObject);

            IPoolableObject poolable = gameObject.GetComponent<IPoolableObject>();
            if (poolable != null)
            {
                poolable.OnSpawn();
            }
        }

        return gameObject;
    }

    public void ReleaseGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);

        activeGameObjects.Remove(gameObject);
        inactiveGameObjects.Add(gameObject);

        OnReleased?.Invoke(gameObject);

        IPoolableObject poolable = gameObject.GetComponent<IPoolableObject>();
        if (poolable != null)
        {
            poolable.OnRelease();
        }
    }

    public int GetPoolSize()
    {
        return activeGameObjects.Count + inactiveGameObjects.Count;
    }

    private void Awake()
    {
        inactiveGameObjects.Capacity = initialPoolSize;
        activeGameObjects.Capacity = initialPoolSize;

        if (prefab != null && initialPoolSize > 0)
        {
            GameObject temp;
            for (int i = 0; i < initialPoolSize; i++)
            {
                temp = Instantiate(prefab);
                temp.SetActive(false);
                inactiveGameObjects.Add(temp);
            }
        }
        else
        {
            if (prefab == null)
            {
                Debug.Log("ObjectPool Error: prefab is null");
            }
        }
    }

    private GameObject GetObjectFromPool()
    {
        if (inactiveGameObjects.Count > 0)
        {
            return inactiveGameObjects[0];
        }

        return null;
    }
}
