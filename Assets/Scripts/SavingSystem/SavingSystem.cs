using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class SavingSystem : Singleton<SavingSystem>
{
    [SerializeField] private string _fileName = "save";
    [SerializeField] private string _fileExtension = "json";

    public event Action onBeforeSave;
    public event Action onSaveCompleted;
    public event Action onLoadCompleted;

    private string fullPath => $"{Application.persistentDataPath}/{_fileName}.{_fileExtension}";

    public bool hasSave => File.Exists(fullPath);

    private bool _isSaving;

    public void Save(params string[] saveableIds)
    {
        onBeforeSave?.Invoke();
        CaptureState(saveableIds);
        PlayerPrefs.Save();
    }

    public void Load(params string[] saveableIds) => RestoreState(saveableIds);

    private void CaptureState(params string[] saveableIds)
    {
        if (_isSaving) return;

        _isSaving = true;

        var state = new Dictionary<string, object>();
        var saveableEntities = FindObjectsOfType<SaveableEntity>(true);

        foreach (var entity in saveableEntities)
        {
            var dataDict = entity.CaptureState(saveableIds);

            foreach (var curr in dataDict)
            {
                var pair = new SaveObjectTypePair
                {
                    obj = JsonConvert.SerializeObject(curr.Value),
                    type = curr.Value?.GetType()?.FullName ?? string.Empty,
                };

                var value = JsonConvert.SerializeObject(pair);

                if (state.ContainsKey(curr.Key))
                {
                    state[curr.Key] = value;
                }
                else
                {
                    state.Add(curr.Key, value);
                }
            }
        }

        var fileExists = File.Exists(fullPath);
        using var fileStream = File.Open(fullPath, fileExists ? FileMode.Truncate : FileMode.OpenOrCreate);
        using var sw = new StreamWriter(fileStream);
        using var jsonWriter = new JsonTextWriter(sw);

        var jsonSerializer = new JsonSerializer();
        jsonSerializer.Serialize(jsonWriter, state);

        _isSaving = false;
        onSaveCompleted?.Invoke();
    }

    private void RestoreState(params string[] saveableIds)
    {
        if (_isSaving) return;

        SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>(true);

        if (!File.Exists(fullPath))
        {
            Debug.LogError("No save file exists!");
        }
        else
        {
            string json = File.ReadAllText(fullPath);
            Dictionary<string, object> localSaveFile = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            RestoreFromFile(localSaveFile, saveableEntities, saveableIds);
        }

        onLoadCompleted?.Invoke();
    }

    private void RestoreFromFile(Dictionary<string, object> rawData, SaveableEntity[] saveableEntities, params string[] saveableIds)
    {
        var transformedDict = new Dictionary<string, object>();

        foreach (var item in rawData)
        {
            if (item.Value == null || (saveableIds != null && saveableIds.Length > 0 && !saveableIds.Contains(item.Key))) continue;

            var pair = JsonConvert.DeserializeObject<SaveObjectTypePair>(item.Value.ToString());

            if (pair == null) continue;

            if (!string.IsNullOrEmpty(pair.type))
            {
                var type = Type.GetType(pair.type);

                transformedDict.Add(item.Key, pair.obj == null ? null : JsonConvert.DeserializeObject(pair.obj, type));
            }
            else
            {
                transformedDict.Add(item.Key, pair.obj == null ? null : JsonConvert.DeserializeObject(pair.obj));
            }
        }

        foreach (var entity in saveableEntities)
        {
            entity.RestoreState(transformedDict);
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        try
        {
            PlayerPrefs.DeleteAll();
            File.Delete(fullPath);
        }
        catch (Exception exception)
        {
            _isSaving = false;
            Debug.LogError("The saving system's 'Clear()' failed with an exception: " + exception.Message);
        }
    }

    private class SaveObjectTypePair
    {
        public string obj;
        public string type;
    }
}