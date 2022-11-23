using System;
using UnityEngine;
using Proyecto26;

public class DatabaseHandler 
{
    private const string projectId = "comp3018-team7";
    private static readonly string databaseUrl = $"https://{projectId}-default-rtdb.europe-west1.firebasedatabase.app/";

    public static void postScore(Score score)
    {
        RestClient.Put<Score>($"{databaseUrl}scores.json", score).Then(response =>
        {
            Debug.Log("Score has been uploaded");
        });
    }
}
