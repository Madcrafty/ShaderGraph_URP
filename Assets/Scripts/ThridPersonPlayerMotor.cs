using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonPlayerMotor : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private Vector3 direction;
    private GameManager gm;
    float turnSmoothVelocity;

    public Transform cam;
    public float speed = 10f;
    public float turnSmoothTime = 0.1f;
    [Tooltip("Incarnates in this radius will be brought into battle")]
    public float battleRadius = 10f;
    public List<GameObject> battleTargets;
    RaycastHit[] hitInfo;
    
    
    private void Awake()
    {
        //gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        controls = new PlayerControls();
        controller = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed -= ctx => Move(ctx.ReadValue<Vector2>());
    }
    private void Move(Vector2 input)
    {
        direction = new Vector3(input.x, 0, input.y).normalized;
    }
    // Update is called once per frame
    void Update()
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
    private void OnDrawGizmos()
    {
        if (hitInfo != null)
        {
            Gizmos.DrawWireSphere(transform.position, battleRadius);
        }
        
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Incarnate"))
    //    {
    //        //Physics.SphereCast(transform.position, battleRadius, transform.forward, out hitInfo);
    //        hitInfo = Physics.SphereCastAll(transform.position, battleRadius, transform.forward,0);
    //        List<GameObject> temp = new List<GameObject>();
    //        //GameObject[] temp = new GameObject[hitInfo.Length];
    //        for (int i = 0; i < hitInfo.Length; i++)
    //        {
    //            if (hitInfo[i].transform.CompareTag("Incarnate"))
    //            {
    //                if (hitInfo[i].transform.GetComponent<OverworldStateSystem>().target == transform)
    //                {
    //                    temp.Add(hitInfo[i].transform.gameObject);
    //                }   
    //            }
    //            else if (hitInfo[i].transform.CompareTag("Player"))
    //            {
    //                temp.Add(hitInfo[i].transform.gameObject);
    //            }
    //        }
    //        battleTargets = temp;
    //        gm.SceneChange(1);
    //    }
    //}
}
