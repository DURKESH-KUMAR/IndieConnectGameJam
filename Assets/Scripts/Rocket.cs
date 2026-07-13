using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] private GameObject explosion;
    private CameraFollow cam;
    AudioSource audioSource;
    [SerializeField]AudioClip explosionClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam=GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CameraFollow>();
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        explosion.SetActive(true);
        audioSource.PlayOneShot(explosionClip, 0.5f);
        cam.StartCoroutine(cam.CameraShake());
    }

}
