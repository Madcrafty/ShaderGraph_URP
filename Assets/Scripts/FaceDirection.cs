using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    private PlayerControls controls;
    private void Awake()
    {
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Turn.performed += ctx => Turn(ctx.ReadValue<Vector2>());
    }
    private void Turn (Vector2 dir)
    {
        transform.rotation = new Quaternion(transform.rotation.x + dir.y/180f, transform.rotation.y + dir.x/180f, 0, 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
