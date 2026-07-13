using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] PlatformPrefabs;
    [SerializeField] private int initialPlatformCount;
    private Vector3 nextSpawnPoint;
    [SerializeField] int poolsize;
    private Queue<GameObject> PlatformPool_1=new Queue<GameObject>();
    private Queue<GameObject> PlatformPool_2=new Queue<GameObject>();
    private Queue<GameObject>[] platformPools=new Queue<GameObject>[2];


    private void Awake()
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject temp = Instantiate(PlatformPrefabs[0]);
            GameObject temp2 = Instantiate(PlatformPrefabs[1]);
            temp.SetActive(false);
            temp2.SetActive(false);
            PlatformPool_1.Enqueue(temp);
            PlatformPool_2.Enqueue(temp2);
        }
        platformPools[0] = PlatformPool_1;
        platformPools[1] = PlatformPool_2;
    }
    void Start()
    {
        for (int i = 0; i < initialPlatformCount; i++)
        {
            Spawn();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Spawn()
    {
        int pool = Random.Range(0,platformPools.Length);
        GameObject obj = platformPools[pool].Dequeue();
        obj.transform.position = nextSpawnPoint;
        obj.SetActive(true);
        platformPools[pool].Enqueue(obj);
        nextSpawnPoint = nextSpawnPoint + Vector3.forward * 50;
    }

}
