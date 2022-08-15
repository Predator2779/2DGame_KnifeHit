using UnityEngine;

public class KnifeBehaviour : MonoBehaviour
{
    #region ����������

    public bool _rotate = false;
    private bool _rebound = false;
    private Transform fromObj;

    #endregion

    #region ������

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        if (_rebound)
            Rebound(fromObj);
        Removing();
        Rotation();
    }

    /// <summary>
    /// OnCollisionEnter2D.
    /// </summary>
    /// <param name="obj"></param>
    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.gameObject.tag == "Knife" && !obj.gameObject.GetComponent<KnifeBehaviour>())
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            fromObj = obj.transform;
            _rebound = true;
        }
    }

    /// <summary>
    /// ������������ ����, ��� ������������ � ������ �����.
    /// </summary>
    /// <param name="obj"></param>
    private void Rebound(Transform obj)
    {
        Destroy(gameObject.GetComponent<Collider2D>());
        transform.Translate(Vector2.up * Time.deltaTime * 30.0f, Space.World);
        _rotate = true;
    }

    /// <summary>
    /// ����������� ������� ��� �������� �� �������.
    /// </summary>
    private void Removing()
    {
        float distance = Vector2.Distance(Vector2.zero, transform.position);

        if (distance > 8.0f)
            Destroy(this.gameObject);
    }

    /// <summary>
    /// �������� ���� ��� ���������� �������.
    /// </summary>
    public void Rotation()
    {
        if (_rotate)
            transform.Rotate(0, 0, Time.deltaTime * -15000.0f);
    }

    #endregion
}
