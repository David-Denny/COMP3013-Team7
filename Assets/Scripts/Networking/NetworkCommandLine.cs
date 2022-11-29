using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = GetComponentInParent<NetworkManager>();

        if (Application.isEditor) return;

        var args = GetArgs();

        if (args.TryGetValue("-mode", out string mode))
        {
            switch(mode)
            {
                case "server": networkManager.StartServer(); break;
                case "host": networkManager.StartHost(); break;
                case "client": networkManager.StartClient(); break;
            }
        }
    }

    private Dictionary<string, string> GetArgs()
    {
        Dictionary<string, string> argsDict = new();
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if(arg.StartsWith('-'))
            {
                var val = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                val = (val?.StartsWith('-') ?? false) ? null : val;

                argsDict[arg] = val;
            }
        }
        return argsDict;
    }
}
