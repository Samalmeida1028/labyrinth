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
    public GameObject PlayerInventoryUI;
    public string text ="";
    public bool endGame=false;
    public bool open = true;



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
            PlayerInventoryUI = player.GetComponent<PlayerInventory>().inventoryUI;
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            PlayerInventoryUI = player.GetComponent<PlayerInventory>().inventoryUI;
            canInteract = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(open==true)
        {
            if(endGame==true&&canInteract&& Input.GetKey("e"))
        {
            loadScreen.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "YOU WIN";
            PlayerInventoryUI.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            Instantiate(loadScreen, player.transform.position, Quaternion.identity);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (canInteract&& Input.GetKey("e"))
        {
            loadScreen.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = text;
            PlayerInventoryUI.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            Instantiate(loadScreen, player.transform.position, Quaternion.identity);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        }
    }
}
