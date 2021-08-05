using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // Start is called before the first frame update

    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    public Rigidbody2D rb;
    public float mSpeed = 10f;
    void Start()
    {
        rb =GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement(){
        if(pathVectorList != null){
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            Debug.Log(pathVectorList.Count);
            Debug.Log(Vector3.Distance(transform.position,targetPosition));

            if(Vector3.Distance(transform.position,targetPosition) > 1f){

                Vector3 moveDir = (targetPosition-transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                rb.transform.position += moveDir * Time.deltaTime * mSpeed;

            }
            else{
                Debug.Log("OOOOOOOOOOOOOOOOF");
                currentPathIndex++;

                if(currentPathIndex >= pathVectorList.Count){
                    Debug.Log("Stopped");
                    StopMoving();

                }
            }

        }


    }

    private void StopMoving(){
        pathVectorList = null;
    }


    public Vector3 GetPosition(){
        return transform.position;
    }
    public void SetTargetPosition(Vector3 tPosition){
        pathVectorList = Pathfinder.Instance.FindPath(GetPosition(),tPosition);
        Debug.Log("lengthof move" + pathVectorList.Count);
        currentPathIndex = 0;
        if(pathVectorList != null && pathVectorList.Count >1){
            pathVectorList.RemoveAt(0);

        }


    }


}
