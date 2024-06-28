using Code.Service;
using UnityEngine;

public class CameraMainMenuHandler : MonoBehaviour
{
  [SerializeField] private Transform _minMovingCamera;
  [SerializeField] private Transform _maxMovingCamera;
  [SerializeField] private Transform _camera;

  private float _speed = 25f;
  private GameInput _gameInput;
  private float _offsetVisible;
  private Vector3 _originPosition;

  // ===================================================================================================
  private void Start()
  {
    foreach (var c in Global.Instance.StaticData.ConstDatas) 
    {
      DebugX.LogWarning($"c.Id {c.Id} / c.IntVal = {c.IntValue}");
    }
    
    _speed = ProjectUtils.GetConstInt(Static.EConstType.CAMERA_SPEED);
    _offsetVisible = ProjectUtils.GetConstInt(Static.EConstType.CAMERA_VISIBLE);

    DebugX.LogWarning($"_speed = {_speed} / _offsetVisible   = {_offsetVisible}");

    _gameInput = Global.Instance.Game.GameInput;
    _gameInput.MouseDiractionCallBack += MovingCamera;
  }

  // ===================================================================================================
  public void SetMinMoovingTrancform(Transform transform)
  {
    _minMovingCamera = transform;
  }

  // ===================================================================================================
  public void SetMaxMoovingTrancform(Transform transform)
  {
    _maxMovingCamera = transform;
  }

  // ===================================================================================================
  public void Init(Vector3 startPosition)
  {
    _originPosition = startPosition;
    SetCameraPosition(_originPosition);
  }

  // ===================================================================================================
  private void OnDestroy() => _gameInput.MouseDiractionCallBack -= MovingCamera;

  // ===================================================================================================
  public void SetCameraPosition(Vector3 valuePosition)
  {
    _camera.position = valuePosition;
  }

  // ===================================================================================================
  public Vector3 GetCameraPosition() => _camera.position;

  // ===================================================================================================
  private void MovingCamera(float diractionX)
  {
    diractionX = _camera.position.x + diractionX * _speed * Time.deltaTime;
    float newDiractionX = Mathf.Clamp(diractionX, _minMovingCamera.position.x, _maxMovingCamera.position.x);
    _camera.position = new Vector3(newDiractionX, _camera.position.y, _camera.position.z);
  }
}
