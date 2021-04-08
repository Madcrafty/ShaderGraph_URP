using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Raycast : MonoBehaviour
{
    public TextMeshProUGUI output;
    //public LayerMask mask;
    //private Camera cam;

    //private void Awake()
    //{
    //    cam = GetComponent<Camera>();
    //}

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 500, LayerMask.GetMask("Default")) == true)
        {
            output.text = hitInfo.transform.name;
            if (hitInfo.transform.GetComponent<Button>() != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hitInfo.transform.GetComponent<Button>().onClick.Invoke();
                }
                
            }
        }
    }
}
