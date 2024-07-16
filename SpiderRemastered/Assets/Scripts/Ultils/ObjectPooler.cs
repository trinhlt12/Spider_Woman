using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered.Ultils
{
    public class ObjectPooler : MonoBehaviour
    {
        public GameObject pooledObject; // prefab of the object to pool
        public int initialPoolSize = 10;

        private Queue<GameObject> pool; 

        void Start()
        {
            pool = new Queue<GameObject>();

            // Populate the pool
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(pooledObject);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public GameObject GetObject()
        {
            // If the pool is empty, create a new object
            if (pool.Count == 0)
            {
                GameObject obj = Instantiate(pooledObject);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }

            // Get an object from the pool
            GameObject pooledObj = pool.Dequeue();
            pooledObj.SetActive(true);
            return pooledObj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}