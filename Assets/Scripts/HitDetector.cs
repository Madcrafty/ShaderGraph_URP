using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    private Entity entity = null;
    private float m_damageMod = 1;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        if (entity.weakpointName == name)
        {
            m_damageMod = entity.weakpointMod;
        }
    }
    public void Hit(int baseDamage)
    {
        float totalDamage = baseDamage * m_damageMod;
        entity.TakeDamage(totalDamage);
    }
}
