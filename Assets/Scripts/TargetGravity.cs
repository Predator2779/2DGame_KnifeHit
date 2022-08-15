using System.Collections.Generic;
using UnityEngine;

public class TargetGravity : MonoBehaviour
{
    [SerializeField] private List<Collider2D> _rbodys;
    [SerializeField] private float _gravity = 1.0f;

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        ApplyToAll();
    }

    /// <summary>
    /// ���������� �������, ��� ����� � ���� ��������.
    /// </summary>
    /// <param name="obj">������</param>
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag == "Massive")
            _rbodys.Add(obj);
    }

    /// <summary>
    /// �������� �������, ��� ������ �� ���� ��������.
    /// </summary>
    /// <param name="obj">������</param>
    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.tag == "Massive")
            _rbodys.Remove(obj);

    }

    /// <summary>
    /// ��������� ���������� � �������� � _rbodys.
    /// </summary>
    private void ApplyToAll()
    {
        if (_rbodys != null)
            foreach (var item in _rbodys)
            {
                Gravity(item);

                RotationFromTarget(item.transform, transform.parent);
            }
    }

    /// <summary>
    /// ���������� ������(��� �������).
    /// </summary>
    /// <param name="item">������ � ���� ��������</param>
    private void Gravity(Collider2D item)
    {
        var rbody = item.GetComponent<Rigidbody2D>();

        Vector2 vec = transform.position - rbody.transform.position;
        rbody.AddForce(vec * _gravity, ForceMode2D.Force);
    }

    /// <summary>
    /// ��������� ����� � ������.
    /// </summary>
    /// <param name="item">������</param>
    /// <param name="target">������</param>
    private void RotationFromTarget(Transform item, Transform target)
    {
        Quaternion rotation = Quaternion.FromToRotation(-item.up, target.position - item.position);

        item.rotation = rotation * item.rotation;
    }
}
