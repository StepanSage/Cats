using Player;

public class E_OrderCreate : EventBase
{
  public Order Order;

  public E_OrderCreate(Order order)
  {
    DebugX.LogForEvents($"E_OrderCreate : SEND");
    Order = order;
  }
}