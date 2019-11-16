using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ParseXML : MonoBehaviour
{

    private Dictionary<int, Scene> storyList;
    private int actual_scene;
    private int current_menu;
    private string full_text;
    private string current_text="";
    private bool first_time = true;

    public TextAsset xmlRawFile;
    public Text uiText;
    public Image image;
    public Button ButtonA;
    public Button ButtonB;
    public Button ButtonC;
    public Button ButtonD;
    public GameObject GameScreen;
    public GameObject MenuScreen;
    public AudioSource letterSound;
    public AudioSource buttonSound;
    public Slider slider;

    static public float volume = 0.1f;



    // Start is called before the first frame update
    void Start()
    {
        slider.value = volume;
        mixer.SetFloat("MusicVolume", Mathf.Log10(ParseXML.volume) * 20);
        Debug.Log("Started parse xml ");
        try
        {
            this.Parse(xmlRawFile.text);
            this.actual_scene = 1;
            this.current_menu = 0;
            this.GameScreen.SetActive(true);
            this.MenuScreen.SetActive(false);
            this.loadScene(actual_scene);
        }
        catch(ParsingError e)
        {
            Debug.LogError(e);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) ==true)
        {
            if(this.current_menu==0)
            {
                this.current_menu = 1;
                this.GameScreen.SetActive(false);
                this.MenuScreen.SetActive(true);
            }
            else if(this.current_menu == 1)
            {
                this.current_menu = 0;
                this.GameScreen.SetActive(true);
                this.MenuScreen.SetActive(false);
            }
        }
    }

    public void clickResume()
    {
        this.current_menu = 0;
        this.GameScreen.SetActive(true);
        this.MenuScreen.SetActive(false);
    }

    public void clickReturnMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void Parse(string name)
    {
        this.storyList = new Dictionary<int, Scene>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(name));
        XmlNode story = xmlDoc.DocumentElement;
        if(story.Name == "story")
        {
            foreach(XmlNode node in story.ChildNodes)
            {
                try
                {
                    int id = int.Parse(node.Attributes["id"].Value);
                    this.storyList[id] = new Scene(node);
                }
                catch(FormatException)
                {
                    throw new ParsingError("id of story is not a number");
                }
            }
        }
        else
        {
            throw new ParsingError("root node invalid name");
        }
    }

    public float delay = 0.2f;


    private IEnumerator showText(int index)
    {
        for(int i=0;i<=full_text.Length;i++)
        {
            current_text = full_text.Substring(0, i);
            this.uiText.text = current_text;
            if(i%2==0 && current_menu==0)
                letterSound.Play();
            yield return new WaitForSeconds(delay);
        }
        if(this.uiText.text == full_text)
        {
            int options_size = storyList[index].OptionsSize();
            if (options_size >= 1)
            {
                this.ButtonA.gameObject.SetActive(true);
                this.ButtonA.GetComponentInChildren<Text>().text = storyList[index].GetOption(0).getText();
            }
            if (options_size >= 2)
            {
                this.ButtonB.gameObject.SetActive(true);
                this.ButtonB.GetComponentInChildren<Text>().text = storyList[index].GetOption(1).getText();
            }
            if (options_size >= 3)
            {
                this.ButtonC.gameObject.SetActive(true);
                this.ButtonC.GetComponentInChildren<Text>().text = storyList[index].GetOption(2).getText();
            }
            if (options_size == 4)
            {
                this.ButtonD.gameObject.SetActive(true);
                this.ButtonD.GetComponentInChildren<Text>().text = storyList[index].GetOption(3).getText();
            }
        }
    }


    private void loadScene(int index)
    {
        this.ButtonA.gameObject.SetActive(false);
        this.ButtonB.gameObject.SetActive(false);
        this.ButtonC.gameObject.SetActive(false);
        this.ButtonD.gameObject.SetActive(false);
        this.uiText.text = "";
        this.current_text = "";
        full_text = storyList[index].GetText().Trim();
        Texture2D text = Resources.Load<Texture2D>("Images/" + storyList[index].GetImage());
        Rect re = new Rect(0, 0, text.width, text.height);
        Vector2 v = new Vector2(0.5f, 0.5f);
        this.image.sprite = Sprite.Create(text, re, v);
        if (first_time)
        {
            StartCoroutine(showText(index));
            first_time = false;
        }
        else
            StartCoroutine(playButtonSound(index));

    }

    public void clickA()
    {
        string scene = this.storyList[actual_scene].GetOption(0).toScene();
        if(scene == "end")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            int id = int.Parse(scene);
            actual_scene = id;
            this.loadScene(actual_scene);
        }
    }
    public void clickB()
    {
        string scene = this.storyList[actual_scene].GetOption(1).toScene();
        if (scene == "end")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            int id = int.Parse(scene);
            actual_scene = id;
            this.loadScene(actual_scene);
        }
    }
    public void clickC()
    {
        string scene = this.storyList[actual_scene].GetOption(2).toScene();
        if (scene == "end")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            int id = int.Parse(scene);
            actual_scene = id;
            this.loadScene(actual_scene);
        }
    }
    public void clickD()
    {
        string scene = this.storyList[actual_scene].GetOption(3).toScene();
        if (scene == "end")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            int id = int.Parse(scene);
            actual_scene = id;
            this.loadScene(actual_scene);
        }
    }

    private IEnumerator playButtonSound(int index)
    {
        buttonSound.Play();
        yield return new WaitWhile(() => buttonSound.isPlaying);
        StartCoroutine(showText(index));
    }

    public AudioMixer mixer;

    public void setLevel(float sliderValue)
    {
        volume = sliderValue;
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
