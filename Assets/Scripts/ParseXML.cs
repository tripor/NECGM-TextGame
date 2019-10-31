using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ParseXML : MonoBehaviour
{

    private Dictionary<int, Scene> storyList;
    private int actual_scene;

    public TextAsset xmlRawFile;
    public TextMeshProUGUI uiText;
    public Image image;
    public Button left;
    public Button right;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started parse xml ");
        try
        {
            this.Parse(xmlRawFile.text);
            this.actual_scene = 1;
            this.loadScene(actual_scene);
        }
        catch(ParsingError e)
        {
            Debug.LogError(e);
        }
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


    private void loadScene(int index)
    {
        this.uiText.text = storyList[index].GetText();
        Texture2D text = Resources.Load<Texture2D>("Images/" + storyList[index].GetImage());
        Rect re = new Rect(0, 0, text.width, text.height);
        Vector2 v = new Vector2(0.5f, 0.5f);
        this.image.sprite = Sprite.Create(text, re, v);
        this.left.GetComponentInChildren<TextMeshProUGUI>().text = storyList[index].GetOption(0).getText();
        this.right.GetComponentInChildren<TextMeshProUGUI>().text = storyList[index].GetOption(1).getText();
    }

    public void leftClick()
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

    public void rightClick()
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
}
