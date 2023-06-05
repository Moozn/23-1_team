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
    public void Save(SaveData savedata)
    {
        //  FileStream stream = new FileStream(fullpth, FileMode.OpenOrCreate);
        //Application.dataPath + "/Save.json", FileMode.OpenOrCreate);
        // SaveData savedate = new SaveData(_total_point, _survivarTime, _remainingHP);
        //  string jsonData = JsonConvert.SerializeObject(savedate);
        // byte[] data = Encoding.UTF8.GetBytes(jsonData);
        // stream.Write(data, 0, data.Length);
        // stream.Close();
        // SaveDataList savedatelist = new SaveDataList();
        SaveDataList savedatelist = new SaveDataList();
        // savedatelist.savedatalist.RemoveAll(s => s == null);
        if (LoadFileEmpty()) savedatelist = Load();
        if (savedata.total_point > 0) savedatelist.savedatalist.Add(savedata);
        savedatelist.savedatalist.Sort(Compare);
        if (savedatelist.savedatalist.Count > 10)
        {
            for (int i = 0; savedatelist.savedatalist.Count > 10; i++) savedatelist.savedatalist.RemoveAt(savedatelist.savedatalist.Count - 1);
        }
        //string jsonData = JsonConvert.SerializeObject(savedatelist);
        string jsonData = JsonUtility.ToJson(savedatelist);
        File.WriteAllText(fullpth + filename, jsonData);
        //byte[] data = Encoding.UTF8.GetBytes(jsonData);
        // stream.Write(data, 0, data.Length);
        // stream.Close();
    }
    public bool LoadFileEmpty()
    {
        if (File.Exists(fullpth + filename))
        {
            if (!string.IsNullOrEmpty(File.ReadAllText(fullpth + filename))) return true;
        }
        return false;
    }
    public SaveDataList Load()
    {
        string json = File.ReadAllText(fullpth + filename);
        SaveDataList saveDataList = JsonUtility.FromJson<SaveDataList>(json);
        return saveDataList;
        //   FileStream stream = new FileStream(fullpth + ".Json", FileMode.Open);
        //       //Application.dataPath + "/Save.json", FileMode.Open);
        //   byte[] data = new byte[stream.Length];
        //   stream.Read(data, 0, data.Length);
        //   stream.Close();
        //   string jsonData = Encoding.UTF8.GetString(data);
        //   SaveDataList savedata;
        //   //  SaveData savedata = JsonConvert.DeserializeObject<SaveData>(jsonData);
        //   if (string.IsNullOrEmpty(jsonData))  savedata = new SaveDataList();
        //   else  savedata = JsonConvert.DeserializeObject<SaveDataList>(jsonData);
        //   return savedata;
    }

    public int Compare(SaveData x, SaveData y) //내가 전에 만들던거라 스코어 포인트 생존시간으로 정렬했는데 이것도 필요한 정렬로 바꾸면됨 정렬이 필요없고 캐릭터 불러오기 같은 경우는 없애면 됨 세이브 데이터만 필요하니
    {
        if (x.score < y.score) return 1;
        else if (x.score > y.score) return -1;
        else
        {
            if (x.total_point < y.total_point) return 1;
            else if (x.total_point > y.total_point) return -1;
            else
            {
                if (x.survivarTime < y.survivarTime) return 1;
                else if (x.survivarTime > y.survivarTime) return -1;
            }
        }

        return 0;
    }
}
[System.Serializable]
public class SaveDataList
{
    public List<SaveData> savedatalist = new List<SaveData>(); //세이브 데이터 리스트로 제이슨 저장 하는데 
    //리스트가 10개보다 작으면 정렬만 해주고 10개보다 크면 정렬후 리사이즈 10해서 크기 맞춰주기
    //그리고 이 리스트를 제이슨에 저장
}
[System.Serializable]
public class SaveData //세이브 데이터는 필요한 데이터로 바꾸면 됨
{
    public float score;
    public int total_point;
    public float survivarTime;

    public SaveData(float _score, int _total_point, float _survivarTime)
    {
        score = _score; //score = total point * 생존시간(초로만)0.5
        total_point = _total_point;
        survivarTime = _survivarTime;
    }
}
