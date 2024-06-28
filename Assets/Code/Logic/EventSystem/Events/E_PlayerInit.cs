public class E_PlayerInit : EventBase
{
  public PlayerController PlayerController;

  public E_PlayerInit(PlayerController playerController) 
  {
    DebugX.LogForEvents($"E_PlayerInit : SEND");
    PlayerController = playerController;
  }
}