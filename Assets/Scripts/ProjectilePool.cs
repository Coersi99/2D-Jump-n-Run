using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;           // object to be spawned
    [SerializeField] List<GameObject> poolObjects;      // list of objects to be (de-)activated
    [SerializeField] int nrOfActiveObjects = 0;         // number of active objects in the list
    [SerializeField] int initialPoolSize = 10;          // initial size of the pool, to be doubled each time the limit is reached

    void Start()
    {
        // initialize object pool by setting a capacity and initializing deactivated gameObjects
        // Note, that this results in a higher loading time at the beginning, since all objects need to be created
        poolObjects = new List<GameObject>(initialPoolSize);
        for (int i = 0; i < initialPoolSize; i++)
            poolObjects.Add(createNewObject());
    }

    GameObject createNewObject()
    {
        // create a new object, deactivate it and assign its parent transform to the spawner
        var go = Instantiate<GameObject>(objectPrefab);
        go.transform.parent = transform;
        go.GetComponent<Projectile>().setProjectilePool(this);
        go.SetActive(false);
        return go;
    }

    void IncreasePool()
    {
        // Double the size of the pool, fill empty slots with newly created objects
        // Note, other implementations involve an incremental increase of x elements, to not overshoot to much
        poolObjects.Capacity *= 2;
        for (int i = nrOfActiveObjects; i < poolObjects.Capacity; i++)
        {
            poolObjects.Add(createNewObject());
        }
    }

    public void SpawnObject(bool facingRight, Vector3 position, float size)
    {
        // in case all pool objects are already in use, resize the pool
        if (nrOfActiveObjects == poolObjects.Capacity)
            IncreasePool();

        // get the first deactivated object and activate it
        var currentGo = poolObjects[nrOfActiveObjects];
        currentGo.SetActive(true);
        Vector3 dir;
        if (facingRight)
            dir = Vector3.right;
        else
            dir = Vector3.left;
        
        currentGo.transform.position = position + dir * size;
        currentGo.GetComponent<Projectile>().setAttributes(nrOfActiveObjects, facingRight);

        // increase active objects counter
        nrOfActiveObjects += 1;
    }

    public void DestroyObject(int destroyIndex)
    {
        // deactivate the selected object immediately
        poolObjects[destroyIndex].SetActive(false);

        // swap with last activated object to keep the list sorted
        var tmp = poolObjects[destroyIndex];
        poolObjects[nrOfActiveObjects - 1].GetComponent<Projectile>().setId(destroyIndex);
        poolObjects[destroyIndex] = poolObjects[nrOfActiveObjects - 1];
        poolObjects[nrOfActiveObjects - 1] = tmp;

        // reduce active objects counter
        nrOfActiveObjects -= 1;
    }
}
