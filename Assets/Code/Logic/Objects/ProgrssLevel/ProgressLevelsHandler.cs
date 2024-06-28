using Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Code.ProgressLevel
{
    public class ProgressLevelsHandler : MonoBehaviour
    {
        [SerializeField]
        private CameraMainMenuHandler _cameraMainMenuHandler;
        [SerializeField] 
        private Color _selectColor;
        private string _idPoint = "Point_1";
        private int _countLevels;
        private List<Level> _levels;
        private int _currentLevel = 0;

        // ===================================================================================================
        private void Start()
        {
            _countLevels = Global.Instance.StaticData.LevelProgressDatas.Count;
            
            _levels = new List<Level>(_countLevels);
            SpawnPointLevel();
            ActiveLevel();
        }

        // ===================================================================================================
        private void SpawnPointLevel()
        {
            
            for (int i = 0; i < _countLevels; i++)
            {
                GameObject prefab = Factory.Instance.GetPointLevel(_idPoint);
                Level levelPoint = prefab?.GetComponent<Level>();
                _idPoint = Global.Instance.StaticData.LevelProgressDatas[i].Prefab;
                Global.Instance.PlayerData.LevelStars.Add(new(levelPoint.AmountStar));
                Debug.Log("Count stars in saves =============" + Global.Instance.PlayerData.LevelStars.Count);

                levelPoint.Initialized(i, Global.Instance.PlayerData.LevelStars[i].CountStars);
                _levels.Add(levelPoint);

                prefab.transform.SetParentWithParam(transform);
                prefab.transform.localPosition += new Vector3(i * 5, 0.5f, 0);
      

                if (_levels[i].IsCompleted)
                {
                    _currentLevel++;
                }
            }

            _cameraMainMenuHandler.SetMinMoovingTrancform(_levels[0].transform);  
            _cameraMainMenuHandler.SetMaxMoovingTrancform(_levels[_countLevels-1].transform);
            

        }

        // ===================================================================================================
        private void ActiveLevel()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                var _level = _levels[i].GetComponent<EnterLevel>();          

                if (i <= _currentLevel )
                {
                    _level.enabled = true;
                    _levels[i].transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    _level.IsActivate = false;
                    _levels[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}
