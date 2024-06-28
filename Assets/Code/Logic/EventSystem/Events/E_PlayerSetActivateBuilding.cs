using Player;

public class E_PlayerSetActivateBuilding : EventBase
{
  public BuildingBehaviour BuildingBehaviour;

  public E_PlayerSetActivateBuilding(BuildingBehaviour buildingBehaviour) 
  {
    DebugX.LogForEvents($"E_PlayerSlotUpdate : SEND");
    BuildingBehaviour = buildingBehaviour;
  }
}