using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSelf : MonoBehaviour
{
    public Transform playerPosition;
    void LateUpdate(){
        transform.position = playerPosition.position;
    }
}
