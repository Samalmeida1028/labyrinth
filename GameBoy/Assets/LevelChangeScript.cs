using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeScript : MonoBehaviour
{
    public bool canInteract;
    public bool hasInteract;
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        canInteract = false;
        hasInteract = false;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            canInteract = true;
            Debug.Log("Player went to next level");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract&& Input.GetKey("e")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
