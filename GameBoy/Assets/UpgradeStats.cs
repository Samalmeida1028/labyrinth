using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStats : MonoBehaviour
{
    public bool canInteract;
    public bool hasInteract;
    public Color interactedColor = Color.blue;
    public GameObject player;
    public GameObject Panel;


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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
    }
        // Update is called once per frame
        void Update()
    {
        if (hasInteract) {
            GetComponent<SpriteRenderer>().color = interactedColor;
        }
        if (canInteract && Input.GetKey("e")) {
            OpenPanel();
            hasInteract = true;
        }
    }

    private void OpenPanel() {

        Panel.SetActive(true);
    }
}
