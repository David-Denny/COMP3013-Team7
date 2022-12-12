using System;
using System.Collections.Generic;
using UnityEngine;

public class UsersAndScores
{
    public Dictionary<string, List<Score>> _dict = new Dictionary<string, List<Score>>();
    public UsersAndScores()
    {
    }

    public void add(string username, Score score)
    {
        if (_dict.ContainsKey(username))
        {
            List<Score> list = _dict[username];

            if (!list.Contains(score))
            {
                list.Add(score);
            }
        }
        else
        {
         
            List<Score> list = new List<Score> { score };
            _dict.Add(username, list);
        }
    }

    public List<Score> getUserScores(string username)
    {
        if (_dict.ContainsKey(username))
        {
            List<Score> list = _dict[username];
            return list;
        }
        return null;
    }
}
