using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        var score1 = new Score("David", "100");
        DatabaseHandler.postScore(score1);
    }
}
