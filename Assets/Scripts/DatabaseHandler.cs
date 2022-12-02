namespace Database
{
    using System;
    using UnityEngine;
    using Proyecto26;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using UnityEditor;
    

    public class DatabaseHandler
    {
        private const string projectId = "comp3018-team7";
        private string databaseUrl = $"https://{projectId}-default-rtdb.europe-west1.firebasedatabase.app/";

        public delegate void PostScoreCallback();
        public delegate void GetUserScoresCallback(List<string> results);
        public delegate void GetAllScoresCallback(UsersAndScores userScores);
        public delegate void DeleteUserScoresCallback();
        public delegate void DeleteAtUrlCallback();


        /**
         * Constructor used for interacting with default url
         */
        public DatabaseHandler() { }

        /**
         * Constructor used for interacting with non-default url
         */
        public DatabaseHandler(string url)
        {         
            databaseUrl = url;
        }

        /**
         * Uploads the input score to the input user
         */
        public void postScore(string username, int score, PostScoreCallback callback)
        {
            RestClient.Post<Score>($"{databaseUrl}scores/{username}.json", new Score(score.ToString())).Then(response => { callback(); });
        }

        /**
         * Returns a list of scores for the input user
         */
        public void getUserScores(string username, GetUserScoresCallback callback)
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


        /**
         * Returns UsersAndScores object containing a dictionary of all users and a corresponding
         * list of their scores
         */
        public void getAllScores(GetAllScoresCallback callback)
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
                        Score scoreValue = new Score(score.Value["score"].ToString());
                        usersAndScores.add(username, scoreValue);
                    }
                }

                Debug.Log("Retrieved all scores");
                callback(usersAndScores);
            });
        }

        /**
        * Deletes all data for input username
        */
        public void deleteUserScores(string username, DeleteUserScoresCallback callback)
        {
            RestClient.Delete($"{databaseUrl}/scores/{username}.json").Then(response =>
            {
                Debug.Log($"Status of deleting '{username}' scores: {response.StatusCode.ToString()}");
                callback();
            });
        }

        /**
         * Deletes all data at input url
         */
        public void deleteAtUrl(string url, DeleteAtUrlCallback callback)
        {
            RestClient.Delete(url + ".json").Then(response =>
            {
                Debug.Log($"Status of deleting url '{url}': {response.StatusCode.ToString()}");
                callback();
            });
        }
    }
}