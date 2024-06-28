public class E_BuyerStandingAtCash : EventBase
{
  public BuyerController BuyerController;

  public E_BuyerStandingAtCash(BuyerController buyerController) 
  {
    DebugX.LogForEvents($"E_BuyerStandingAtCash : SEND");
    BuyerController = buyerController;
  }
}