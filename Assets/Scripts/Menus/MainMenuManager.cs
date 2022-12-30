using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MainMenuPage
{
    public string pageName;
    public GameObject page;
}

/// <summary>
/// Class to manage the state of the main menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string _startPage;
    [SerializeField] private MainMenuPage[] _pages;

    private readonly Dictionary<string, GameObject> _pageDict = new();

    private GameObject _currentPage;

    private void Awake()
    {
        foreach(MainMenuPage page in _pages)
        {
            _pageDict[page.pageName] = page.page;
            page.page.SetActive(false);
        }
    }

    private void Start()
    {
        ShowPage(_startPage);
    }

    /// <summary>
    /// Swaps the currently shown page
    /// </summary>
    /// <param name="page">Key of the page to display</param>
    public void ShowPage(string page)
    {
        // If page key is valid
        if(_pageDict.ContainsKey(page))
        {
            // Hide current page
            if (_currentPage != null)
                _currentPage.SetActive(false);

            // Show page
            _currentPage = _pageDict[page];
            _currentPage.SetActive(true);
        }
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
