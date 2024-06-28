using DG.Tweening;
using UnityEngine;

namespace Code.Logic.Objects.ProgrssLevel 
{
    public class SelectFlag : SelectUtilits, ISelect
    {
        [SerializeField] private Transform _flag;

        protected override void Start()
        {
            OFFSET = 1.5f;
            _normal = _flag.position;
            _select = new Vector3(_normal.x, _normal.y + OFFSET, _normal.z); 
        }

        public override void Select()
        {
            _flag.transform
                .DOMove(_select, VALUETIME);
        }

        public void SelectPin()
        {
            Select();
            _flag.transform.position = _select;
        }

        public override void UNSelecet()
        {
            _flag.transform
                .DOMove(_normal, VALUETIME);
        }

        public void UnSelectPin()
        {
            UNSelecet();
            _flag.transform.position = _normal;
        }
    }
}

