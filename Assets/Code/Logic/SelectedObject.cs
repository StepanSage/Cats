using System.Collections;
using UnityEngine;

namespace Code.Logic
{
    [RequireComponent(typeof(ClearCounter))]
    public class SelectedObject : MonoBehaviour
    {
        private GameObject _defult;
        private GameObject _select;

        private void Start()
        {
            _defult = transform.GetChild(0).gameObject;
            _select = transform.GetChild(1).gameObject;
            GetDefultState();
        }

        private void GetDefultState()
        {
            _defult.SetActive(true);
            _select.SetActive(false);
        }

        public  void currentStateSelected(bool isSelectd)
        {
            if(isSelectd)
            {
                Selected();
            }
            else
            {
                NotSlected();
            }
        }

        public void Selected()
        {
            _defult.SetActive(false);
            _select.SetActive(true);
        }

        public void NotSlected()
        {
            _defult.SetActive(true);
            _select.SetActive(false);
        }

    }
}