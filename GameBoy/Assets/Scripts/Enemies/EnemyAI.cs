using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyAI : MonoBehaviour
{
private Vector3 startPos;
private Vector3 roamPos;

private void Start(){
    startPos = transform.position;

}
private void Update(){

    
}
private Vector3 getRoamPos(){
    return startPos + UtilsClass.GetRandomDir() * Random.Range(10f,30f);
}
}
