using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{

    public string displayText;
    public TextMeshProUGUI text;
    GameObject displayTextOBJ;

    public void Start(){
    displayText = "Hello";
    text.GetComponent<TextMeshProUGUI>().text = displayText;
    text.enabled = false;
    }


    public void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("hi");
        text.enabled = true;

    }

}
