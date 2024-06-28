using TMPro;
using UnityEngine;

namespace Code.Logic.GameShop
{
    public class DescriptionPanel : MonoBehaviour
    {
        private TMP_Text _text;
        private OnHover[] descriptionOnHovers;

        public void Start()
        {
            _text= gameObject.GetComponentInChildren<TMP_Text>();
            descriptionOnHovers = FindObjectsOfType<OnHover>();
            foreach (var item in descriptionOnHovers)
            {
                item.IsHoveringCallBack += ApplyDiscription;
            }
            gameObject.SetActive(false);
        }

        private void ApplyDiscription(bool isAction, Vector3 endPosition, string discription)
        {
            if(isAction)
            {
                gameObject.SetActive(true);
                _text.text =discription;
                transform.position = endPosition  - new Vector3(0,100,0);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

   
    }
}
