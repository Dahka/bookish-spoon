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

    public void StartGame(bool loadSave)
    {
        PlayerPrefs.SetInt("LoadGame", loadSave ? 1 : 0);
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
