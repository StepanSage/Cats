using UnityEngine;
using Newtonsoft.Json;
using System;
using Player;
using GreenLeaf;

public class DataStoreManager : Singleton<DataStoreManager>
{
  public class SaveData
  {
    public PlayerData PlayerData = new PlayerData();
  }

  public SaveData Data
  {
    get
    {
      if (_data == null)
      {
        DataLoad();
      }
      return _data;
    }
  }

  protected DataStoreManager() { }
  private SaveData _data = null;

  private string _saveData = "SaveData";

  private int _tryLoadCount = 0;
  private int _tryLoadCountMax = 5;

  // ===================================================================================================
  public void Init(Action onComplete = null, Action onCreate = null)
  {
    DataLoad(onComplete, onCreate);
  }

  // ===================================================================================================
  public void DataSave(Action onComplete = null)
  {
    if (_data == null)
      return;

    try
    {
      var data = JsonConvert.SerializeObject(_data);
      AdvancedPlayerPrefs.SetString(_saveData, data);
      AdvancedPlayerPrefs.Save();
    }
    catch (Exception e)
    {
      Debug.LogError($"Deserialize Data Error: {e.Message}  ");
    }

    onComplete?.Invoke();
  }

  // ===================================================================================================
  public void DataReLoad()
  {
    DataLoad();
  }

  // ===================================================================================================
  public void DataLoad(Action onComplete = null, Action onCreate = null)
  {
    _tryLoadCount++;

    if (_tryLoadCount > _tryLoadCountMax)
    {
      Debug.LogError($"DataLoad LIMIT!!!");
    }

    try
    {
      if (AdvancedPlayerPrefs.HasKey(_saveData))
      {
        var str = AdvancedPlayerPrefs.GetString(_saveData, "");

        if (!string.IsNullOrEmpty(str))
        {
          _data = JsonConvert.DeserializeObject<SaveData>(str);
        }
        onComplete?.Invoke();
      }
      else
      {
        _data = new SaveData();
        var data = JsonConvert.SerializeObject(_data);
        AdvancedPlayerPrefs.SetString(_saveData, data);
        AdvancedPlayerPrefs.Save();

        onCreate?.Invoke();
      }
    }
    catch (Exception e)
    {
      Debug.LogError($"Deserialize Data Error: {e.Message}");

      _data = new SaveData();
      var data = JsonConvert.SerializeObject(_data);
      AdvancedPlayerPrefs.SetString(_saveData, data);
      AdvancedPlayerPrefs.Save();

      onCreate?.Invoke();
    }
  }

  // ===================================================================================================
  public void ClearData()
  {
    _data = new SaveData();
    DataSave();
  }
}