using System.Collections;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    AudioSource audiosource;
    [Header("AudioClips")]
    [SerializeField] AudioClip tireScreech;
    [SerializeField] float tireScreechVolume;
    [SerializeField] private float tireScreechAngle;
    [SerializeField]bool canTireScreech;

    [Header("Particles")]
    [SerializeField] private GameObject particle;
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.rotation.y)<=0.1)
            canTireScreech=true;
        if (canTireScreech&&Mathf.Abs(transform.rotation.y)>=tireScreechAngle) 
        {
            canTireScreech = false;
            audiosource.PlayOneShot(tireScreech, tireScreechVolume);
            StartCoroutine(DriftParticle());
        }

    }

    IEnumerator DriftParticle()
    {
        particle.SetActive(true);
        yield return new WaitForSeconds(3);
        particle.SetActive(false);
    }
}
