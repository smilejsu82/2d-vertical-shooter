using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private List<SpawnData> spawnDatas;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadData()
    {
        var ta = Resources.Load<TextAsset>("stage_data");
        var json = ta.text;
        spawnDatas = JsonConvert.DeserializeObject<List<SpawnData>>(json);
    }

    public List<SpawnData> GetSpawnDatas()
    {
        return spawnDatas;
    }
}
