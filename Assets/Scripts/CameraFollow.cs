using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool lookAt;
    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -8f);

    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float rotationSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = lookAt?target.position + target.TransformDirection(offset):target.position+offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime);


        Quaternion desiredRotation = lookAt?Quaternion.LookRotation(
            target.position + target.forward * 10f - transform.position):Quaternion.LookRotation(Vector3.back);
        
        

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime);
    }
}