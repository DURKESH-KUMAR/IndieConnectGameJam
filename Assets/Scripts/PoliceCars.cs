using UnityEngine;

public class PoliceCars : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject player;
    [SerializeField] float distance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(0,0,player.transform.position.z+distance);
    }
}
