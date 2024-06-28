using System.Collections;
using UnityEngine;

namespace Code.Logic
{
    public class ClearCounter : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _food;

        

        public void Interaction()
        {
            var prefab = Instantiate(_food, _spawnPoint);
            prefab.transform.localPosition = Vector3.zero;
        }
    }
}