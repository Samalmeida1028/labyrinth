using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{


    public GameObject FrontLayer;
    public GameObject BackLayer;

    private Animator FrontAnimator;
    private Animator BackAnimator;

    private bool shotBow = false;
    private bool isShooting = false;

    const string Bow_Front = "Front_Layer";
    const string Bow_Back = "Back_Layer";

    // Start is called before the first frame update
    void Start()
    {
        FrontAnimator = FrontLayer.GetComponent<Animator>();
        BackAnimator = BackLayer.GetComponent<Animator>();

        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shotBow)
        {
            shotBow = false;

            //Fire Bow
            if (!isShooting)
            {
                isShooting = true;

                gameObject.SetActive(true);

                FrontAnimator.Play(Bow_Front);
                BackAnimator.Play(Bow_Back);

                Invoke("StopShooting", 0.6f); 
            }
        }

        //Roate bow with character
    }

    void StopShooting()
    {
        isShooting = false;

        //gameObject.SetActive(false);
    }

    public void Shoot()
    {
        shotBow = true;
    }
}
