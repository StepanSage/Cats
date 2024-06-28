using Static;

public class BinBehaviour : BuildingBehaviour
{
  // ===================================================================================================
  public BinBehaviour(View view, BuildData buildData) : base(view, buildData) { }

  // ===================================================================================================
  public override void Activate()
  {
    UIUtils.NeedConfirm(
      "Вы действительно хотите очистить все слоты?",
      "Очистить",
      "Отмена",
      () =>
      {
        ProjectUtils.ClearAllPlayerSlots(
          (consums) =>
          {
            ProjectUtils.RemoveConsumeFromPlayerSlot(consums);
          }
          );
      });
  }
}

