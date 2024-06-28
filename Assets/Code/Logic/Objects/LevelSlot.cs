using System;
using UnityEngine;

[Serializable]
public class LevelSlot : MonoBehaviour
{
  public int Index;
  public Transform Transform { get; private set; }

  // ===================================================================================================
  private void Awake()
  {
    Transform = transform;
  }
}
