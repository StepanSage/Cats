using Player;

public class E_OrderStartProgress : EventBase
{
  public Order Order;

  public E_OrderStartProgress(Order order)
  {
    DebugX.LogForEvents($"E_OrderStartProgress : SEND");
    Order = order;
  }
}