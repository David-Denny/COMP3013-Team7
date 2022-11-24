using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {

        DatabaseHandler.postScore("david-denny", 20, null);
               
        // 
        DatabaseHandler.getAllScores((UsersAndScores userScores) =>
        {
            Debug.Log($"Got all scores - {userScores.dict}");

            
            foreach (var user in userScores.dict)
            {
                string username = user.Key;
                List<Score> scores = user.Value;

                Debug.Log($"User '{username}' has scores:");
                foreach(Score score in scores)
                {
                    Debug.Log(score.score);
                }
            }
        });
    }
}
