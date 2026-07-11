using System.Collections.Generic;
using UnityEngine;

public class MouseControlls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Camera mouseControllCamera;
    [Header("Obstracles")]
    [SerializeField] private GameObject[] obstraclePrefabs;
    [SerializeField] int poolsize;
    [SerializeField] float dropHeight;
    [SerializeField] float cooldown;
    float time;
    private Queue<GameObject> ObstraclePool_1 = new Queue<GameObject>();
    private Queue<GameObject> ObstraclePool_2 = new Queue<GameObject>();
    private Queue<GameObject>[] obstraclePools = new Queue<GameObject>[2];
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject temp = Instantiate(obstraclePrefabs[0]);
            GameObject temp2 = Instantiate(obstraclePrefabs[1]);
            temp.SetActive(false);
            temp2.SetActive(false);
            ObstraclePool_1.Enqueue(temp);
            ObstraclePool_2.Enqueue(temp2);
        }
        obstraclePools[0] = ObstraclePool_1;
        obstraclePools[1] = ObstraclePool_2;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        ray=mouseControllCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.position= hit.point;
        }
        if (Input.GetMouseButtonDown(0)&&time>=cooldown)
        {
            time = 0;
            Spawn();
        }

    }

    public void Spawn()
    {
        int pool = Random.Range(0, obstraclePools.Length);
        GameObject obj = obstraclePools[pool].Dequeue();
        obj.transform.position = transform.position+Vector3.up*dropHeight;
        obj.SetActive(true);
        obstraclePools[pool].Enqueue(obj);
    }



}
