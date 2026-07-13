using System.Collections.Generic;
using UnityEngine;

public class MouseControlls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Camera mouseControllCamera;
    [SerializeField] private Rigidbody Player;
    [Header("Obstracles")]
    [SerializeField] private GameObject[] obstraclePrefabs;
    [SerializeField] int poolsize;
    [SerializeField] float dropHeight;
    [SerializeField] float cooldown;
    [SerializeField] float ForceMultiplier;
    [SerializeField] LayerMask roadLayer;
    float initialForce;
    float time;
    private Queue<GameObject> ObstraclePool_1 = new Queue<GameObject>();
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject temp = Instantiate(obstraclePrefabs[0]);
            temp.SetActive(false);
            ObstraclePool_1.Enqueue(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        ray=mouseControllCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit,1000,roadLayer))
        {
            transform.position= new Vector3(hit.point.x,hit.point.y,Mathf.Clamp(hit.point.z,Player.transform.position.z+10,mouseControllCamera.transform.position.z-10));
        }
        if (Input.GetMouseButtonDown(0)&&time>=cooldown)
        {
            time = 0;
            Spawn();
        }
        initialForce=Player.linearVelocity.magnitude*Time.deltaTime*ForceMultiplier;
    }

    public void Spawn()
    {
        GameObject obj = ObstraclePool_1.Dequeue();
        obj.SetActive(false);
        Rigidbody rb=obj.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        obj.transform.position = new Vector3(transform.position.x,dropHeight,transform.position.z+initialForce);
        obj.transform.rotation = Quaternion.Euler(90, 0, 0);
        obj.SetActive(true);
        //rb.AddForce(initialForce*Vector3.forward,ForceMode.Impulse);
        ObstraclePool_1.Enqueue(obj);
    }



}
