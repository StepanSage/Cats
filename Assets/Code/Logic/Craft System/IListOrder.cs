using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Logic.Craft_System
{
    public interface IListOrder
    {
        public void SetOrderList(List<GameObject> orderList);

        public List<GameObject> GetOrderList();

        public void CrearnElematOrderList(int index);
    }
}
