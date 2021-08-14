using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject AudioManagerOBJ;

    private AudioManager Audio;

    void Start()
    {
        Audio = AudioManagerOBJ.GetComponent<AudioManager>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventoryUI.SetActive(false);
    }

    public void PlayGame()
    {
        Audio.Play("UI_Click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() 
    {
        Audio.Play("UI_Click");
        Application.Quit();
    }
}
