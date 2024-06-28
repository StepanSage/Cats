using Player;

public class E_BuyerGoAway : EventBase
{
  public BuyerController BuyerController;

  public E_BuyerGoAway(BuyerController buyerController) 
  {
    DebugX.LogForEvents($"E_BuyerGoAway : SEND");
    BuyerController = buyerController;
  }
}