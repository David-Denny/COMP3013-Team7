using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using UnityEditor.SearchService;
using UnityEditor;
using static System.Net.WebRequestMethods;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Assertions;
using System.Runtime.Serialization.Json;

public class TestDatabaseHandler : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        // Remove to run tests
        return;
        string databaseUrl = "https://comp3018-team7-default-rtdb.europe-west1.firebasedatabase.app/testing/";
        DatabaseHandler db = new DatabaseHandler(databaseUrl);

        db.deleteAtUrl(databaseUrl, () =>
        {
            // uncomment one test at a time and run
            testUploadScore();
            //testDeleteScore();
            //testGetUserScores();
            //testGetAllScores();
        });
    }

    public static void testUploadScore()
    {
        string databaseUrl = "https://comp3018-team7-default-rtdb.europe-west1.firebasedatabase.app/testing/";
        DatabaseHandler db = new DatabaseHandler(databaseUrl);
        
        int testInput = 100;
        string expectedOutput = "100";
        string testUser = "user-upload-test";

        db.postScore(testUser, testInput, () =>
        {
            Debug.Log("Uploaded score");
            db.getUserScores(testUser, result =>
            {
                Debug.Log("Getting all scores");
                if (result.Contains(expectedOutput))
                {
                    Debug.Log("Database contains uploaded score");
                }
                else
                {
                    Debug.Log("Database does not contain uploaded score");
                }
            });
        });

    }

    public static void testDeleteScore()
    {
        string databaseUrl = "https://comp3018-team7-default-rtdb.europe-west1.firebasedatabase.app/testing/";
        DatabaseHandler db = new DatabaseHandler(databaseUrl);

        string username = "user-test-2";
        int score = 50;

        bool userExists = false;
        db.postScore(username, score, () =>
        {
            db.postScore("other-username", score, () =>
            {
                Debug.Log($"Uploaded score for user {username}");
                db.deleteUserScores(username, () =>
                {
                    Debug.Log("getting all scores");
                    db.getAllScores(results =>
                    {

                        Debug.Log("Got all scores");
                        foreach (var resultUsername in results._dict.Keys)
                        {
                            if (resultUsername == username)
                            {
                                userExists = true;
                            }
                        }

                        if (userExists)
                        {
                            Debug.Log("User's scores not deleted");
                        }
                        else
                        {
                            Debug.Log("User's scores have been successfully deleted");
                        }
                    });
                });
            });
            
        });
    }

    public static void testGetUserScores()
    {
        string databaseUrl = "https://comp3018-team7-default-rtdb.europe-west1.firebasedatabase.app/testing/";
        DatabaseHandler db = new DatabaseHandler(databaseUrl);

        string username = "user-test-5";
        int[] scores = { 1, 2, 3, 4 };
        int count = 0;

        foreach (int score in scores)
        {
            if (count == scores.Length - 1)
            {
                Debug.Log($"Uploading score - {score} ");
                db.postScore(username, score, () => {
                    
                    db.getUserScores(username, results =>
                    {
                        Debug.Log($"User '{username}' has scores:");
                        foreach (string score in results)
                        {
                            Debug.Log(score);
                        }

                    });

                });
            }
            else
            {
                Debug.Log($"Uploading score - {score}");
                db.postScore(username, score, null);
            }

            count++;
        }
    }
    public static void testGetAllScores()
    {
        // Testing all user scores will be properly returned
        string databaseUrl = "https://comp3018-team7-default-rtdb.europe-west1.firebasedatabase.app/testing/";
        DatabaseHandler db = new DatabaseHandler(databaseUrl);

        string username1 = "user-test-3";
        string username2 = "user-test-4";
        int[] scores = { 1, 2, 3, 4 };
        int count = 0;

        db.deleteAtUrl(databaseUrl, () =>
        {
            foreach (int score in scores)
            {
                if (count == scores.Length - 1)
                {
                    Debug.Log($"Uploading score - {score} ");
                    db.postScore(username1, score, () => {
                        db.postScore(username2, score, () =>
                        {
                            db.getAllScores(results =>
                            {
                                foreach (var username in results._dict.Keys)
                                {

                                    Debug.Log($"Username '{username}' has scores:");
                                    foreach (Score score in results.getUserScores(username))
                                    {
                                        Debug.Log($"Score - {score._score}");
                                    }
                                }

                            });
                        });
                    });
                }
                else
                {
                    Debug.Log($"Uploading score - {score}");
                    db.postScore(username1, score, null);
                    db.postScore(username2, score, null);
                }

                count++;
            }
        });
    }
}
