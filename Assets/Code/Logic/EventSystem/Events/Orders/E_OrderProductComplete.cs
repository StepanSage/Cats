using Player;

public class E_OrderProductComplete : EventBase
{
  public OrderProduct OrderProduct;

  public E_OrderProductComplete(OrderProduct orderProduct) 
  {
    DebugX.LogForEvents($"E_OrdersComplete : SEND");
    OrderProduct = orderProduct;
  }
}