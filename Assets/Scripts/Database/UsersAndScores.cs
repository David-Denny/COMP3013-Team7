using System;
using System.Collections.Generic;
using UnityEngine;

public class UsersAndScores
{
    public Dictionary<string, List<Score>> dict = new Dictionary<string, List<Score>>();
    public UsersAndScores()
    {
    }

    public void add(string username, Score score)
    {
        if (dict.ContainsKey(username))
        {
            List<Score> list = dict[username];

            if (!list.Contains(score))
            {
                list.Add(score);
            }
        }
        else
        {
         
            List<Score> list = new List<Score> { score };
            dict.Add(username, list);
        }
    }

    public List<Score> getUserScores(string username)
    {
        if (dict.ContainsKey(username))
        {
            List<Score> list = dict[username];
            return list;
        }
        return null;
    }
}
