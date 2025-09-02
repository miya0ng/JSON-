using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[System.Serializable]
public class RecordData
{
    public Vector3 position;
    public Vector3 scale;
    public Vector3 rotation;
}
public class JsonTest3 : MonoBehaviour
{
    public Button recordButton;
    public Button playButton;
    private List<RecordData> recordDataList = new List<RecordData>();

    public static readonly string SaveFileName = "cube.json";
    public static string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public List<string> jsonObjectDatas = new List<string>();

    public GameObject cube;
    private bool isRecord = false;
    private bool isPlaying = false;

    public void Update()
    {
        if(isRecord)
        {
            StartCoroutine(Record());
        }
    }

    public void RecordButton()
    {
        isRecord = !isRecord;
        if (isRecord)
        {
            recordButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop Record";
        }
        else
        {
            recordButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Record";
            File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(recordDataList, Formatting.Indented, new Vector3Converter()));
            if (jsonObjectDatas.Count > 0)
            {
                jsonObjectDatas.Clear();
            }
        }
    }
    public IEnumerator Record()
    {
        var currentRecord = new RecordData();
        currentRecord.position = cube.transform.position;
        currentRecord.scale = cube.transform.localScale;
        currentRecord.rotation = cube.transform.rotation.eulerAngles;
        // objdata.rotation = obj.transform.rotation;
        recordDataList.Add(currentRecord);
        Debug.Log($"Saving to: {SaveFilePath}");


        yield return new WaitForSeconds(0.002f);
    }

    public IEnumerator Play()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogError($"File not found: {SaveFilePath}");
        }
        var json = File.ReadAllText(SaveFilePath);
        var transform = JsonConvert.DeserializeObject<List<RecordData>>(json, new Vector3Converter());
            
        foreach (var item in transform)
        {
           cube.transform.position = item.position;
           cube.transform.localScale = item.scale;
            cube.transform.rotation = Quaternion.Euler(item.rotation);
            yield return new WaitForSeconds(0.002f);
        }
    }

    public void PlayButton()
    {

        isPlaying = !isPlaying;
        if (isPlaying)
        {
            StartCoroutine(Play());
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop Play";
        }
        else
        {
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Play";
            
        }
    }
}