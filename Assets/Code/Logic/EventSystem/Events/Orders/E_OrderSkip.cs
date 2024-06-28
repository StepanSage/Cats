using Player;

public class E_OrderSkip : EventBase
{
  public Order Order;

  public E_OrderSkip(Order order)
  {
    DebugX.LogForEvents($"E_OrderSkip : SEND");
    Order = order;
  }
}