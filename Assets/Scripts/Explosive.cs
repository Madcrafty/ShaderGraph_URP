using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public int damage;
    public float radius;
    public float detonationTime;
    [Tooltip("Time for explosive to play wind-up effects")]
    public float windUpTime;
    [Tooltip("Time the hit-box is avtive for")]
    public float activeExplosionTime;
    [Tooltip("Time for explosive to finish effects")]
    public float windDownTime;

    private float timer;
    private MeshRenderer mr;
    private SphereCollider grenadeHitBox;
    private Rigidbody grenadeBody;

    private ParticleSystem explosionFX;
    private AudioSource explosionSFX;
    private bool hasExploded = false;
    // Start is called before the first frame update
    void Start()
    {
        grenadeHitBox = GetComponent<SphereCollider>();
        grenadeBody = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();

        explosionFX = transform.GetChild(0).GetComponent<ParticleSystem>();
        explosionFX.Stop();
        explosionSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        detonationTime -= Time.deltaTime;
        if (detonationTime <= windUpTime && !hasExploded)
        {
            //Explode
            StartCoroutine(Explode());
        }
    }
    IEnumerator Explode()
    {
        hasExploded = true;
        explosionSFX.Play();
        yield return new WaitForSeconds(windUpTime);

        grenadeHitBox.isTrigger = true;
        grenadeHitBox.radius = 12;
        grenadeBody.isKinematic = true;
        mr.enabled = false;
        explosionFX.Emit(1);
        
        yield return new WaitForSeconds(activeExplosionTime);
        grenadeHitBox.enabled = false;

        yield return new WaitForSeconds(windDownTime);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HitDetector>() != null)
        {
            other.GetComponent<HitDetector>().Hit(damage);
        }
    }
}