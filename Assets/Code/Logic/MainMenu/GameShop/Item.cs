using UnityEngine;

namespace Code.Logic.GameShop
{
    public class Item : MonoBehaviour
    {
        public string ID { get; private set; }
        public float Price;
        public string Discription;
        public TypeItem TypeItem;
        public Sprite Sprite;

        private void Start()
        {
            ID = transform.name;
        }


    }
}
