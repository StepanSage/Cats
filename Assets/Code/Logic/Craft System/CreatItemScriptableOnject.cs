using System.Collections.Generic;
using UnityEngine;

namespace Code.Logic.CraftSystem 
{ 
    [CreateAssetMenu(fileName = "Item", menuName = "Creat Item")]
    public class CreatItemScriptableOnject : ScriptableObject
    {
        [SerializeField] private GameObject _finalItem;

        [SerializeField] private List<GameObject> _resursesCract;

        public GameObject _FanalItem => _finalItem;

        public List<GameObject> ResursesCraft => _resursesCract;
    }
}

