using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : SingletonPersistent<GameManager>
{
    public static GameManager _GameManager;

    //Serielizable Data ---
    private Dictionary<string, bool> Section_Status;
    public Dictionary<string, bool> Get_SectionStatus => Section_Status;

    protected override void Awake()
    {
        base.Awake();
        _GameManager = this;
        Section_Status = new Dictionary<string, bool>();

        SceneManagement.ChangeScene(1);
    }

    protected void Respawn()
    {
        
    }

    #region Serielization
    // Save game state
    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameSaveData.cms";
        FileStream stream = new FileStream(path, FileMode.Create);

        //GameStateData data = new GameStateData(sectionClearStatus, enemyDefeatedStatus);
        //formatter.Serialize(stream, data);
        stream.Close();
    }

    // Load game state
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/gameState.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //GameStateData data = formatter.Deserialize(stream) as GameStateData;
            stream.Close();

            //sectionClearStatus = data.sectionClearStatus;
           //enemyDefeatedStatus = data.enemyDefeatedStatus;
        }
        else
        {
            Debug.Log("No saved game found.");
        }
    }

    // Mark section as cleared
    public void MarkSectionCleared(string sectionName)
    {
        //sectionClearStatus[sectionName] = true;
        SaveGame();
    }

    // Mark enemy as defeated
    public void MarkEnemyDefeated(string enemyName)
    {
        //enemyDefeatedStatus[enemyName] = true;
        SaveGame();
    }
    #endregion
}
