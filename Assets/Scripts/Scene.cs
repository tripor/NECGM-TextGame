using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Scene
{
    private XmlNode self;
    private string title;
    private string text;
    private string image;
    private List<Option> options;

    public Scene(XmlNode node)
    {
        this.options = new List<Option>();
        this.self = node;
        XmlNodeList childs = node.ChildNodes;
        if(childs[0].Name == "title")
        {
            try
            {
                this.title = childs[0].FirstChild.Value;
            }
            catch (Exception e)
            {
                this.title = "";
            }
        }
        else
        {
            throw new ParsingError("1º element of story must be title");
        }
        if (childs[1].Name == "text")
        {
            try
            {
                this.text = childs[1].FirstChild.Value;
            }
            catch(Exception e)
            {
                this.text = "";
            }
        }
        else
        {
            throw new ParsingError("2º element of story must be text");
        }
        if (childs[2].Name == "image")
        {   
            try
            {
                this.image = childs[2].Attributes["name"].Value;
            }
            catch(System.Exception)
            {
                throw new ParsingError("image must have atribute name");
            }
        }
        else
        {
            throw new ParsingError("3º element of story must be image");
        }
        if (childs[3].Name == "options")
        {
            if(childs[3].ChildNodes.Count == 0)
                this.options.Add(new Option());
            foreach (XmlNode option_node in childs[3].ChildNodes)
            {
                if(option_node.Name == "option")
                {
                    this.options.Add(new Option(option_node));
                }
                else
                {
                    throw new ParsingError("options must have option");
                }
            }
        }
        else
        {
            throw new ParsingError("4º element of story must be options");
        }
    }

    public string GetText()
    {
        return this.text;
    }

    public string GetImage()
    {
        return this.image;
    }

    public Option GetOption(int index)
    {
        return this.options[index];
    }

    public int OptionsSize()
    {
        return this.options.Count;
    }

    public List<Option> GetOptions()
    {
        return this.options;
    }

    public class Option
    {
        private string to_scene;
        private string text;
        public Option(XmlNode node)
        {
            try
            {
                this.to_scene = node.Attributes["scene"].Value;
                if(this.to_scene != "end")
                {
                    try
                    {
                        int.Parse(this.to_scene);
                    }
                    catch (FormatException)
                    {
                        throw new ParsingError("scene of option must be \"end\" or a number");
                    }
                }
            }
            catch (System.Exception)
            {
                throw new ParsingError("option must have atribute scene");
            }
            this.text = node.FirstChild.Value;
        }
        public Option()
        {
            this.to_scene = "end";
            this.text = "End";
        }

        public string toScene()
        {
            return this.to_scene;
        }

        public string getText()
        {
            return this.text;
        }
    }
}
