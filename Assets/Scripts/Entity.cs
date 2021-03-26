using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header(header: "Hit Points")]
    public int maxHealth = 100;

    [Header(header: "Weak Points")]
    public string weakpointName;
    public float weakpointMod = 1;

    protected float hp;
    protected Ragdoll ragdoll;
    protected GameManager gm;

    protected virtual void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        ragdoll = GetComponentInChildren<Ragdoll>();
        if (ragdoll.rigidbodies != null && ragdoll.rigidbodies.Count > 0)
        {
            foreach (Rigidbody rb in ragdoll.rigidbodies)
            {
                rb.gameObject.AddComponent<HitDetector>();
            }
        }
        else
        {
            gameObject.AddComponent<HitDetector>();
        }
        hp = maxHealth;
        RagdollState(false);
    }
    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
    virtual protected void Die()
    {
        ragdoll.RagdollOn = true;
    }
    virtual public void Respawn()
    {
        hp = maxHealth;
        ragdoll.RagdollOn = false;
    }
    virtual public void SetActive(bool toggle)
    {
        enabled = toggle;
        RagdollState(!toggle);
    }
    virtual public void RagdollState(bool toggle)
    {

    }
}
