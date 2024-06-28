using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Logic.Craft_System;

public class ListOrder : IListOrder
{
    private List<GameObject> _listOrder;
    private System.Random rng = new();
    public void SetOrderList(List<GameObject> orderList)
    {
        _listOrder = orderList;
        SortOrder();
    }

    public void CrearnElematOrderList(int index)
    {
        _listOrder.RemoveAt(index);
    }

    public List<GameObject> GetOrderList()
    {
        return _listOrder;
    }

    private void SortOrder()
    {   
        _listOrder.Sort((a, b) => rng.Next(-1, 2));
    }
}
