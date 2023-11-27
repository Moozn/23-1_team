using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class SaveLoadMgr : MonoBehaviour
{
    public static SaveLoadMgr instance = null;
    private string fullpth, filename;
    private void Start()
    {
        fullpth = Application.streamingAssetsPath + "/Save";//"Assets/Save/";
        filename = "save";
        if (Directory.Exists(fullpth) == false) Directory.CreateDirectory(fullpth);
    }
    public static SaveLoadMgr Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<SaveLoadMgr>();
                if (!instance) instance = new GameObject("SaveLoadManager").AddComponent<SaveLoadMgr>();
                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    private void Initialize() { }
    private void Awake() { if (this != Instance) Destroy(gameObject); }
    public void Save(PlayerStat savedata)
    {
        PlayerStat savedate = new PlayerStat();
        savedate = savedata;
        string jsonData = JsonUtility.ToJson(savedate);
        System.IO.File.WriteAllText(fullpth + filename, jsonData);
    }
    public bool LoadFileEmpty()
    {
        if (File.Exists(fullpth + filename))
        {
            if (!string.IsNullOrEmpty(File.ReadAllText(fullpth + filename))) return true;
        }
        return false;
    }
    public void deleteFile()
    {
        System.IO.File.Delete(fullpth + filename);
    }
    public PlayerStat Load()
    {
        string json = File.ReadAllText(fullpth + filename);
        PlayerStat saveData = JsonUtility.FromJson<PlayerStat>(json);
        return saveData;
    }

}
