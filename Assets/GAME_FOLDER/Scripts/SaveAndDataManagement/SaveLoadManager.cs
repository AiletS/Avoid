using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    // éQçl : https://qiita.com/kiku09020/items/8429d8693c761e0da4a1
    public SaveData data;
    string filepath;
    string fileName = "SaveData.json";

    private void Awake()
    {
        filepath = Application.dataPath + "/" + fileName;
        if (!File.Exists(filepath))
        {
            data_init();
            Save(data);
        }
        data = Load(filepath);
        /*
        string prefs_data = PlayerPrefs.GetString("SaveData", "");
        if (prefs_data == "") Save(data);
        data = Load();
        */
    }

    /*
    public void Save(SaveData data)
    {
        string json_string = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", json_string);
    }

    public SaveData Load()
    {
        return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
    }
    */

    public void Save(SaveData data)
    {
        string json_string = JsonUtility.ToJson(data);
        StreamWriter wr = new StreamWriter(filepath, false);
        wr.WriteLine(json_string);
        wr.Close();
    }

    public SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json_string = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<SaveData>(json_string);
    }

    private void OnDestroy()
    {
        Save(data);
    }

    private void data_init()
    {
        const int len = SaveData.TrialLen;
        data.score_multiply = 1f;
        for(int i = 0; i < len; i++)
        {
            data.TrialList[i] = 0;
        }
        data.PlayCount = 0;
        data.HighScore = 0;
    }
}
