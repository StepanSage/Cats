using Player;

public class E_OrderComplete : EventBase
{
  public Order Order;

  public E_OrderComplete(Order order) 
  {
    DebugX.LogForEvents($"E_OrderComplete : SEND");
    Order = order;
  }
}