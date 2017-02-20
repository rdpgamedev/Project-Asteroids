using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Highscores")]
public class Highscores
{
    [XmlArray("Scores")]
    [XmlArrayItem("Score")]
    public List<Score> Scores = new List<Score>(); //sorted descending

    public int TopScore ()
    {
        return Scores[0].score;
    }

    public int BottomScore ()
    {
        return Scores[9].score;
    }

    public void Push (Score score) //inserts new score in descending order
    {
        int i = 0;
        for (; i < Scores.Count; ++i)
        {
            if (score.score > Scores[i].score)
            {
                break;
            }
        }
        Scores.Insert(i, score);
        Save(Path.Combine(Application.persistentDataPath, "highscores.xml"));
    }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(Highscores));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }

    public static Highscores Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Highscores));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Highscores;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Highscores LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Highscores));
        return serializer.Deserialize(new StringReader(text)) as Highscores;
    }

    public static Highscores CreateDefault()
    {
        Highscores highscores = new global::Highscores();
        List<Score> Scores = highscores.Scores;
        Scores.Add(new Score("RDP", 10));
        Scores.Add(new Score("J9",  9));
        Scores.Add(new Score("OLI", 8));
        Scores.Add(new Score("MIK", 7));
        Scores.Add(new Score("COL", 6));
        Scores.Add(new Score("ALX", 5));
        Scores.Add(new Score("GAB", 4));
        Scores.Add(new Score("VEG", 3));
        Scores.Add(new Score("FLY", 2));
        Scores.Add(new Score("ORE", 1));
        highscores.Save(Path.Combine(Application.persistentDataPath, "highscores.xml"));
        return highscores;
    }

}
