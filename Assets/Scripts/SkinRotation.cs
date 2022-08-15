using UnityEngine;

public class SkinRotation : MonoBehaviour
{
    [SerializeField] private float _targetSpeedRotation = 1;

    /// <summary>
    /// Update.
    /// </summary>
    void Update()
    {
        transform.Rotate(0, 0, _targetSpeedRotation * Time.deltaTime * 100.0f);
    }
}
