using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelChangeScript : MonoBehaviour
{
    public bool canInteract;
    public bool hasInteract;
    public GameObject player;
    public GameObject loadScreen;
    public string text ="";



    // Start is called before the first frame update
    void Start()
    {
        text = "Floor " + (SceneManager.GetActiveScene().buildIndex);
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
        if (canInteract&& Input.GetKey("e"))
        {
            loadScreen.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = text;
            Instantiate(loadScreen, player.transform.position, Quaternion.identity);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
