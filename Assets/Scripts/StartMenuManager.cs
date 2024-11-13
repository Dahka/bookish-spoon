using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] Button loadGameButton;

    // Start is called before the first frame update
    void Start()
    {
        string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + MemoryGameManager.SAVE_DATA_FILENAME;

        //We enable the load game button only if we have a save file
        loadGameButton.interactable = File.Exists(filePath);
    }

    //Function to start the game, with bool parameter to determine if we start a new game or load a saved one
    public void StartGame(bool loadSave)
    {
        PlayerPrefs.SetInt("LoadGame", loadSave ? 1 : 0);
        SceneManager.LoadScene("GameScene");
    }

    //Function to exit the game
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
