using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timeDate : MonoBehaviour
{
    public GameObject timeTextObject;
    //public GameObject dateTextObject;
    public GameObject monthTextObject;
    public GameObject dayTextObject;
    public GameObject yearTextObject;

    private System.DateTime _currTimeDate;

    // Start is called before the first frame update
    void Start() {
        InvokeRepeating("UpdateTime", 0f, 10f);
    }

    // Update is called once per frame
    void UpdateTime() {
        //timeTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("h:mmtt MM/dd/yyyy");

        //timeTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("h:mmtt");
        //dateTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("MM/dd/yy");

        _currTimeDate = System.DateTime.Now;
        timeTextObject.GetComponent<TextMeshPro>().text = _currTimeDate.ToString("h:mmtt");
        monthTextObject.GetComponent<TextMeshPro>().text = _currTimeDate.ToString("MMMM");
        dayTextObject.GetComponent<TextMeshPro>().text = _currTimeDate.Day.ToString();
        yearTextObject.GetComponent<TextMeshPro>().text = _currTimeDate.Year.ToString();
    }
}