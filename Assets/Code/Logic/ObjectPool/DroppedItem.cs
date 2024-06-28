using DG.Tweening;
using Objects;
using Static;
using UnityEngine;
using UnityEngine.UI;
using static ObjectPoolManager;

public class DroppedItem : MonoBehaviour
{
  [SerializeField]
  private CanvasGroup _canvasGroup;

  [SerializeField]
  private Image _icon;

  private Vector3 _start;
  private Vector3 _end;
  private Transform _transform;

  private Consume _consume;

  private float _distance = 0f;
  private float _speed = 1500f;
  private bool _add = true;

  // ===================================================================================================
  public void Init(Consume consume, Vector3 start, Vector3 end, bool add = true)
  {
    if (consume == null)
      return;

    DebugX.LogForUI("DroppedItem : Init");

    gameObject.SetActive(true);

    _canvasGroup.alpha = 0f;
    _transform = transform;
    _start = start;
    _end = end;
    _consume = consume;
    _add = add;

    _transform.position = _start;
    _end = UIUtils.GetGetter(consume.Type);
    _distance = Vector3.Distance(_start, _end);

    _icon.sprite = Factory.Instance.GetIcon(consume.Type);

    var delay = Random.Range(0.1f, 0.7f);
    Invoke(nameof(StartDrop), delay);
  }

  // ===================================================================================================
  private void StartDrop()
  {
    DebugX.LogForUI("DroppedItem : StartDrop");

    _canvasGroup.alpha = 1f;

    transform.DOScale(0.7f, 1)
             .SetLink(gameObject);

    var circ = Random.insideUnitCircle;
    var endValue = transform.position + new Vector3(circ.x * 100, circ.y * 100, transform.position.z);
    var delay = Random.Range(0.1f, 0.7f);

    transform.DOJump(
      endValue,
      50,
      0,
      0.5f).OnComplete(() =>
      {
        Invoke(nameof(GoToTarget), delay);
      }).SetLink(gameObject);
  }

  // ===================================================================================================
  private void GoToTarget()
  {
    DebugX.LogForUI("DroppedItem : GoToTarget");

    _transform.DOMove(_end, _distance / _speed)
        .SetEase(Ease.Linear)
        .SetLink(gameObject)
        .OnComplete(() =>
        {
          EndMove();
        });
  }

  // ===================================================================================================
  private void EndMove()
  {
    DebugX.LogForUI($"DroppedItem : EndMove : _add = {_add}");

    if (_add)
    {
      ProjectUtils.AnimatedItemAddEnd(_consume);
    }
    _canvasGroup.alpha = 0f;
    _consume = null;
    DeSpawn(gameObject);
  }
}
