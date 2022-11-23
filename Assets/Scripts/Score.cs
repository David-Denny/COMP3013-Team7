using System;

[Serializable]
public class Score
{
    public string username;
    public string score;

    public Score(string username, string score)
    {
        this.username = username;
        this.score = score;
    }
}