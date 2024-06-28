using Code.Logic.Objects.ProgrssLevel;
using System;

public class DialogStartLevel : Dialog
{
    private StarsHandler _starsHandler;
    private int _countStars = 0;

  // ===================================================================================================
  protected void Awake()
  {
    _group = (byte) SortingGroup.Top;
    _starsHandler = FindObjectOfType<StarsHandler>();
  }

  // ===================================================================================================
  public void Initialized(int countStar)
  {
    _countStars = countStar;
  }

  // ===================================================================================================
  public void Activate()
  {
    Open(RenderStars);
  }

  private void RenderStars()
  {
    _starsHandler.OpenStars(_countStars);
  }

  // ===================================================================================================
  public void BtnOk_Click()
  {
    Global.Instance.Game.StartBattleState();
    Close();
    Global.Instance.SoundService.PlayOne("popup");
  }

  // ===================================================================================================
  public void BtnClose_Click()
  {
    Close(CloseStars);
    Global.Instance.SoundService.PlayOne("popup");
  }

  public void CloseStars()
  {
    _starsHandler.CloseStars();
    Global.Instance.Game.GameInput.StartInput();
        
  }


}
