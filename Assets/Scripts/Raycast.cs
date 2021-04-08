using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Raycast : MonoBehaviour
{
    public TextMeshProUGUI output;
    //public GameObject lookfocus;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 500, LayerMask.GetMask("Default")) == true)
        {
            output.text = hitInfo.transform.name;
            //lookfocus.transform.position = hitInfo.transform.position;
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
