using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Highscores")]
public class Highscores{
    [XmlArray("Scores")]
    [XmlArrayItem("Score")]
    public List<Score> Scores = new List<Score>(); //sorted descending

    public int TopScore ()
    {
        return Scores[0].score;
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

}
