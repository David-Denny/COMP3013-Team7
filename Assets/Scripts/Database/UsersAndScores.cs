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

    /**
     * Adds a tuple of the (username, score) to the list of all users/scores
     */
    public void add(string username, Score score)
    {
        _allUsersAndScores.Add(new Tuple<string, string>(username, score.score));
    }

    /**
     * Returns a list of all the scores assigned to the input username
     */
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

    /**
     * Returns the list of all users and their scores
     */
    public List<Tuple<string, string>> getAllScores()
    {
        return _allUsersAndScores;
    }

    /**
     * Returns the list of all users and their scores in descending order of score
     */
    public List<Tuple<string, string>> getAllScoresOrdered()
    {
        return _allUsersAndScores.OrderByDescending(i => i.Item2).ToList();
    }

}
