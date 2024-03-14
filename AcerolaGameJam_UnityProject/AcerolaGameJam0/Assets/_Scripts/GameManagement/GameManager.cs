using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : SingletonPersistent<GameManager>
{
    public static GameManager _GameManager;

    //Serielizable Data ---
    private Dictionary<string, bool> Section_Status;
    public Dictionary<string, bool> Get_SectionStatus => Section_Status;

    private Dictionary<string, bool> SpawnPoint_Status;
    public Dictionary<string, bool> Get_SpawnPointStatus => SpawnPoint_Status;

    private Dictionary<string, bool> Medkit_CollectionStatus;
    public Dictionary<string, bool> Get_Medkit_CollectionStatus => Medkit_CollectionStatus;
    public HashSet<string> Temporary_TillSaveMedkitData;

    private Dictionary<string, bool> TimeShard_Status;
    public Dictionary<string, bool> Get_TimeShardStatus => TimeShard_Status;

    private Dictionary<string, bool> Distortion_Status;
    public Dictionary<string, bool> Get_DistortionStatus => Distortion_Status;

    public GenericCallChannel Respawn_Channel_UI;
    Respawn_Data CurrentRespawnData;

    [SerializeField] private GenericCallChannel Level_InitChannel;

    //Scene Changing --- Setting Targets
    [HideInInspector] public GenericCallChannel SceneChange_SetTarget;

    //Player Save Values
    [HideInInspector] public float Saved_PlayerHealth;
    [HideInInspector] public int Saved_PlayerMedkits;

    protected override void Awake()
    {
        base.Awake();
        _GameManager = this;
        Section_Status = new Dictionary<string, bool>();
        SpawnPoint_Status = new Dictionary<string, bool>();
        Medkit_CollectionStatus = new Dictionary<string, bool>();
        Temporary_TillSaveMedkitData = new HashSet<string>();
        Distortion_Status = new Dictionary<string, bool>();

        TimeShard_Status = new Dictionary<string, bool>();

        //Initial Load of the Scene ---
        SceneManagement.ChangeScene(2);
        Wait_RespawnData();
    }

    public void Wait_RespawnData()
    {
        //Start a Courotine that waits until CurrentRespawnData is not null
        StartCoroutine(WaitForRespawnData());
    }

    public void Wait_SceneChange(int index)
    {
        StartCoroutine(WaitForSceneLoad_SceneChange(index));
    }

    public void Set_Spawn(Respawn_Data data)
    {
        CurrentRespawnData = data;
        foreach(string element in Temporary_TillSaveMedkitData)
        {
            Medkit_CollectionStatus[element] = true;
        }

        Temporary_TillSaveMedkitData.Clear();

        Saved_PlayerHealth = PlayerController.Get_Controller.Get_Health;
        Saved_PlayerMedkits = PlayerController.Get_Controller.Current_MedicalCount;
    }

    public void Check_SceneRespawn(InputAction.CallbackContext context)
    {
        if (CurrentRespawnData.Scene_Index != SceneManagement.Get_SceneIndex())
        {
            //Wait for Scene to Load ---
            SceneManagement.ChangeScene(CurrentRespawnData.Scene_Index);
            StartCoroutine(WaitForSceneLoad_Respawn(CurrentRespawnData.Scene_Index));
        }
        else
        {
            //Load the Scene --- Player Respawn ---
            Respawn();
        }
    }

    IEnumerator WaitForSceneLoad_Respawn(int sceneNumber)
    {
        while (SceneManagement.Get_SceneIndex() != sceneNumber)
        {
            yield return null;
        }

        Respawn();
    }

    IEnumerator WaitForSceneLoad_SceneChange(int sceneNumber)
    {
        while (SceneManagement.Get_SceneIndex() != sceneNumber)
        {
            yield return null;
        }

        Scene_ChangeRespawn();
    }

    IEnumerator WaitForRespawnData()
    {
        while (CurrentRespawnData == null)
        {
            yield return null;
        }

        Respawn();
    }

    protected void Respawn()
    {
        Temporary_TillSaveMedkitData.Clear();
        Level_InitChannel.RaiseEvent();
        PlayerController.Get_Controller.Reset_Player();
        PlayerController.Get_Controller.Get_PlayerRB.position = CurrentRespawnData.Respawn_Location;
        Respawn_Channel_UI.RaiseEvent();

        PlayerController.Get_Controller.OnSceneChange();
    }

    protected void Scene_ChangeRespawn()
    {
        Level_InitChannel.RaiseEvent();
        Respawn_Channel_UI.RaiseEvent();
        PlayerController.Get_Controller.OnSceneChange();

        if (SceneChange_SetTarget != null)
            SceneChange_SetTarget.RaiseEvent();
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
