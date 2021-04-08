using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Ragdoll : MonoBehaviour
{
    //private Animator animator = null;
    //private CharacterController characterController = null;
    //private NavMeshAgent agent = null;
    //private Canvas GUI = null;
    //private Enemy enemy = null;
    //private Player player = null;
    private Entity entity = null;
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();

    public bool RagdollOn
    {
        get { return !entity.enabled; }
        set
        {
            //animator.enabled = !value;
            //agent.enabled = !value;
            //GUI.enabled = !value;
            //enemy.enabled = !value;
            entity.SetActive(!value);
            //player.enabled = !value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        //enemy = GetComponent<Enemy>();
        //GUI = transform.GetChild(2).GetComponent<Canvas>();
        //characterController = GetComponent<CharacterController>();
        entity = GetComponent<Entity>();
        Rigidbody[] tmp = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in tmp)
        {
            //if (rb != entity.GetComponent<Rigidbody>())
            //{
                rigidbodies.Add(rb);
            //} 
        }
        //player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
