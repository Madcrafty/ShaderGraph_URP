using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public GameObject thing;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Spawn);
    }
    void Spawn()
    {
        thing.SetActive(!thing.activeInHierarchy);
    }
}
