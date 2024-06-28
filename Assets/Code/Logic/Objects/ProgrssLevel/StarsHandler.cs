using UnityEngine;

namespace Code.Logic.Objects.ProgrssLevel
{
    public class StarsHandler : MonoBehaviour
    {
        [SerializeField] private Star[] _stars = new Star[3];

        private float _timeScale = 0.1f;
        private float _countTime = 0;

        public void OpenStars(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _stars[i].gameObject.SetActive(true);
                _countTime += _timeScale;
                _stars[i].StartScale(_countTime);
            }
            _countTime = 0;
        }

        public void CloseStars()
        {
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].EndScale();
            }
        }   
    }
}