using System;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class DatabaseHandler 
{
    private const string projectId = "comp3018-team7";
    private static readonly string databaseUrl = $"https://{projectId}-default-rtdb.europe-west1.firebasedatabase.app/";

    public delegate void PostScoreCallback();
    public delegate void GetUserScoresCallback(List<string> results);
    public delegate void GetAllScoresCallback(UsersAndScores userScores);

    public static void postScore(string username, int score, PostScoreCallback callback)
    {
        RestClient.Post<Score>($"{databaseUrl}scores/{username}.json", new Score(score.ToString())).Then(response => { callback(); });
    }

    public static void getUserScores(string username, GetUserScoresCallback callback)
    {
        RestClient.Get($"{databaseUrl}scores/{username}.json").Then(response =>
        {
            var jsonObj = JObject.Parse(response.Text);
            List<string> scores = new List<string>();
            foreach (KeyValuePair<string, JToken> pair in jsonObj)
            {
                var key = pair.Key;
                string score = pair.Value["score"].ToString();

                scores.Add(score);
            }

            callback(scores);
        });

    }


    public static void getAllScores(GetAllScoresCallback callback)
    {

        UsersAndScores usersAndScores = new UsersAndScores();

        RestClient.Get($"{databaseUrl}scores.json").Then(response =>
        {
            var jsonObj = JObject.Parse(response.Text);
            
            foreach (KeyValuePair<string, JToken> user in jsonObj)
            {
                var username = user.Key;
                var scoreObj = JObject.Parse(user.Value.ToString());

                foreach (KeyValuePair<string, JToken> score in scoreObj)
                {
                    Debug.Log($"Score key - {score.Key}, score value - '{score.Value["score"]}'");

                    Score scoreValue = new Score(score.Value["score"].ToString());
                    usersAndScores.add(username, scoreValue);
                }
            }

            callback(usersAndScores);
        });
    }
}
