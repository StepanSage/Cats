using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Code.Logic.Objects.ProgrssLevel
{
    public class Star : MonoBehaviour
    {
        private const float TimeScale = 0.1f;

        private Vector2 _endScale;
        public int ID => _id;

        [SerializeField] private int _id;

        private void Start()
        {
            _endScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void StartScale(float timeWaits)
        {
            StartCoroutine(Waits(timeWaits));
        }

        public void EndScale()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }

        private IEnumerator Waits(float time)
        {
            yield return new WaitForSeconds(time);
            Scale(TimeScale);
        }

        private void Scale(float timeScale)
        {
            transform
                .DOScale(_endScale, timeScale);
        }
    }
}