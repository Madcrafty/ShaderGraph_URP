using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactDamageObject : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<HitDetector>() != null)
        {
            collision.transform.GetComponent<HitDetector>().Hit((int)Damage());
        }
        else if (collision.transform.GetComponent<Entity>() != null)
        {
            collision.transform.GetComponent<Entity>().TakeDamage(Damage());
        }
    }
    float Damage()
    {
        return Vector3.Magnitude(rb.velocity) * rb.mass;
    }
}
