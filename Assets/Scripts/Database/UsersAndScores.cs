using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UsersAndScores
{
    private List<Tuple<string, string>> _allUsersAndScores = new List<Tuple<string, string>>();
    public UsersAndScores()
    {
    }

    public void add(string username, Score score)
    {
        _allUsersAndScores.Add(new Tuple<string, string>(username, score.score));
    }

    public List<Tuple<string, string>> getUserScores(string username)
    {

        List<Tuple<string, string>> userScores = new List<Tuple<string, string>>();

        foreach (var userScore in _allUsersAndScores)
        {
            if (userScore.Item1 == username)
            {
                userScores.Add(userScore);
            }
        }

        return userScores;
    }

    public List<Tuple<string, string>> getAllScores()
    {
        return _allUsersAndScores.OrderBy(i => i.Item2).ToList();
    }

    public List<Tuple<string, string>> getAllScoresOrdered()
    {
        return _allUsersAndScores.OrderByDescending(i => i.Item2).ToList();
    }

}
