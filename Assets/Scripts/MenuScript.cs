using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    private int current_menu;
    public GameObject MenuScreen;
    public GameObject AudioScreen;
    public Text loading;


    public AudioMixer mixer;
    public Slider slider;

    void Start()
    {
        this.loading.gameObject.SetActive(false);
        slider.value = ParseXML.volume;
        mixer.SetFloat("MusicVolume", Mathf.Log10(ParseXML.volume) * 20);
        this.current_menu = 0;
        this.MenuScreen.SetActive(true);
        this.AudioScreen.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if (this.current_menu == 0)
            {
                this.current_menu = 1;
                this.MenuScreen.SetActive(false);
                this.AudioScreen.SetActive(true);
            }
            else if (this.current_menu == 1)
            {
                this.current_menu = 0;
                this.MenuScreen.SetActive(true);
                this.AudioScreen.SetActive(false);
            }
        }
    }
    public void PlayGame()
    {
        this.loading.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void setLevel(float sliderValue)
    {
        ParseXML.volume = sliderValue;
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
