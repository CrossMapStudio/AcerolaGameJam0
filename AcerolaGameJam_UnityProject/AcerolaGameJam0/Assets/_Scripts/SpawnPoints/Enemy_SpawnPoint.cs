using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpawnPoint : MonoBehaviour
{
    //This list is used to Spawn Enemies in the World on Reset if the Section Has Not Been Cleared
    [SerializeField] private string Section_ID; //Gamemanager Dictionary of Sections for Reload ---

    [SerializeField] private float Spawn_Radius;
    
    [SerializeField] private List<SpawnGroup> Initial_SpawnList;
    private Dictionary<Guid, Enemy_Driver> CurrentSpawnList;
    [SerializeField] private Guid_Channel GuidCall_Channel;

    [SerializeField] private GenericCallChannel GameManagerReset_LevelChannel;

    private void Awake()
    {
        GuidCall_Channel = Instantiate(GuidCall_Channel);
        GuidCall_Channel.OnEventRaised.AddListener(Remove_EnemyInstance);
        CurrentSpawnList = new Dictionary<Guid, Enemy_Driver>();

        //Add on the Awake ---
        GameManagerReset_LevelChannel.OnEventRaised.AddListener(Restart_Section);
    }

    private void Start()
    {
        if (!GameManager._GameManager.Get_SectionStatus.ContainsKey(Section_ID))
        {
            //Will Have to Change if we Save Information ---
            GameManager._GameManager.Get_SectionStatus.Add(Section_ID, false);
        }

        Repopulate_SpawnSection();
    }

    public void Restart_Section()
    {
        //Delete All Enemies --- Respawn Enemies
        Depopulate_SpawnSection();
        Repopulate_SpawnSection();
    }

    private void Repopulate_SpawnSection()
    {
        //Section has been Cleared.
        if (GameManager._GameManager.Get_SectionStatus[Section_ID])
            return;

        foreach (SpawnGroup element in Initial_SpawnList)
        {
            for (int i = 0; i < element.Spawn_Count; i++)
            {
                if (element.Spawn_Point != null)
                {
                    var clone = Instantiate(element.Enemy, element.Spawn_Point.position + (UnityEngine.Random.insideUnitSphere * element.Spawn_Radius), Quaternion.identity);
                    Guid Enemy_ID = Guid.NewGuid();
                    clone.EnemyInstance_Key = Enemy_ID;
                    clone.Spawn_PointLink = GuidCall_Channel;
                    CurrentSpawnList.Add(Enemy_ID, clone);
                }
                else
                {
                    var clone = Instantiate(element.Enemy, transform.position + UnityEngine.Random.insideUnitSphere * Spawn_Radius, Quaternion.identity);
                    Guid Enemy_ID = Guid.NewGuid();
                    clone.EnemyInstance_Key = Enemy_ID;
                    clone.Spawn_PointLink = GuidCall_Channel;
                    CurrentSpawnList.Add(Enemy_ID, clone);
                }
            }
        }
    }

    private void Depopulate_SpawnSection()
    {
        if (GameManager._GameManager.Get_SectionStatus[Section_ID])
            return;

        List<Enemy_Driver> ToDestroy = new List<Enemy_Driver>();
        List<Guid> KeysToRemove = new List<Guid>();

        foreach(KeyValuePair<Guid, Enemy_Driver> element in CurrentSpawnList)
        {
            ToDestroy.Add(element.Value);
            KeysToRemove.Add(element.Key);
        }

        for (int i = 0; i < ToDestroy.Count; i++)
        {
            CurrentSpawnList.Remove(KeysToRemove[i]);
            Destroy(ToDestroy[i].gameObject);
        }
    }

    private void Remove_EnemyInstance(Guid ID)
    {
        CurrentSpawnList.Remove(ID);
        if (CurrentSpawnList.Count == 0)
        {
            //Section was Cleared ---
            GameManager._GameManager.Get_SectionStatus[Section_ID] = true;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, Spawn_Radius);
    }
}

[Serializable]
public class SpawnGroup
{
    public Enemy_Driver Enemy;
    public int Spawn_Count;
    public Transform Spawn_Point;
    public float Spawn_Radius;
}
