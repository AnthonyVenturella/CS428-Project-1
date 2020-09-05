using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timeDate : MonoBehaviour
{
    public GameObject timeTextObject;
    public GameObject dateTextObject;

    // Start is called before the first frame update
    void Start() {
        InvokeRepeating("UpdateTime", 0f, 10f);
    }

    // Update is called once per frame
    void UpdateTime() {
        //timeTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("h:mmtt MM/dd/yyyy");
        timeTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("h:mmtt");
        dateTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("MM/dd/yy");
    }
}