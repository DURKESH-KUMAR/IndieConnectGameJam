using UnityEngine;

public class MouseControlls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject obstraclePrefab;
    [SerializeField] private Camera mouseControllCamera;
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ray=mouseControllCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.position= hit.point;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(obstraclePrefab,transform.position+Vector3.up*5,Quaternion.identity);
        }

    }


    
}
