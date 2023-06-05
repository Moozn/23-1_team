using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance = null;
    [SerializeField] Monster monster;
    public List<Monster> monsterList = new List<Monster>();
    public static MonsterSpawner Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<MonsterSpawner>();
                if (!instance) instance = new GameObject("MosterSpawner").AddComponent<MonsterSpawner>();
                instance.Initialize();
            }
            return instance;
        }
    }
    private void Initialize()
    {
    
    }

    private void Start()
    {
        Initialize();
        Monster ememy = Instantiate(monster);
        ememy.Deactivation();
        monsterList.Add(ememy);
    }
    public void InsertList(Monster p_object)
    {
        monsterList.Add(p_object);
        p_object.Deactivation();
    }
    public void GetList()
    {
        Monster ememy = monsterList[0];
        monsterList.RemoveAt(0);
        ememy.Activation();
    }
}
