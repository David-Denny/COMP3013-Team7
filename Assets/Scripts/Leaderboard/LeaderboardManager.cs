using Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Text = UnityEngine.UI.Text;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private RectTransform _leftContainer, _rightContainer;
    [SerializeField] private GameObject _scorePrefab;

    // Start is called before the first frame update
    void Start()
    {
        DatabaseHandler db = new();

        // Call database to get the scores
        db.getAllScores(result =>
        {
            // Get list of username/score in descending order
            List<Tuple<string, string>> userScore = result.getAllScoresOrdered();

            for (int i = 0; i < 10; i++)
            {
                // Insantiate new score prefab
                GameObject scoreText = Instantiate(_scorePrefab);

                // Set content of score prefab
                scoreText.transform.GetChild(0).transform.GetComponent<Text>().text = (i + 1).ToString() + ".";
                scoreText.transform.GetChild(1).transform.GetComponent<Text>().text = userScore[i].Item1;
                scoreText.transform.GetChild(2).transform.GetComponent<Text>().text = userScore[i].Item2;

                // Add to UI
                scoreText.transform.SetParent(i < 5 ? _leftContainer : _rightContainer, false);
            }
        });
    }
}
