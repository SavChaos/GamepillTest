using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static Main _instance;
    public PlayerManager currentPlayer;
    public SpawnManager spawnManager;

    private void Awake()
    {
        _instance = this;
    }

   
    public static Main GetInstance()
    {
        return _instance;
    }
	
}
