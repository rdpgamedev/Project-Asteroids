using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Score {
    [XmlAttribute("name")]
    public string name;
    public int score;

	public Score (string _name, int _score)
    {
        name = _name;
        score = _score;
    }
}
