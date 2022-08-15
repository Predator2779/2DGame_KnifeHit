using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThrowScript : ManagerScript
{
    public GameObject _cloneKnife;

    /// <summary>
    /// Start.
    /// </summary>
    private void Start() => SpawnKnife();

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        Timer();

        if (Input.GetKey(KeyCode.Space) && _fire)
            ThrowTimer();
        if (Input.GetKeyUp(KeyCode.Space))
            SelectThrow();
    }

    /// <summary>
    /// Метание ножа.
    /// </summary>
    private void ThrowKnife()
    {
        _fire = false;
        float force = 50.0f;
        _cloneKnife.AddComponent<KnifeBehaviour>();

        if (_isTwisted)
        {
            force = 15.0f;
            _cloneKnife.GetComponent<KnifeBehaviour>()._rotate = true;
        }
        _cloneKnife.GetComponent<Rigidbody2D>().AddForce(Vector2.down * force, ForceMode2D.Impulse);

        _holdTime = 0.15f;
        _holdThrow = true;
    }

    /// <summary>
    /// Время нажатия клавиши метания.
    /// </summary>
    public void ThrowTimer() => _holdTime += Time.deltaTime;

    /// <summary>
    /// Выбор стиля метания.
    /// </summary>
    public void SelectThrow()
    {
        if (_holdTime > 0.15f) _isTwisted = true;

        ThrowKnife();
    }

    /// <summary>
    /// Спаун ножей.
    /// </summary>
    private void SpawnKnife()
    {
        _cloneKnife = Instantiate(_knife, transform.position, transform.rotation);
        _cloneKnife.transform.rotation = Quaternion.identity;
        _cloneKnife.transform.SetParent(this.transform);
        _cloneKnife.transform.Find("KnifeSprite").GetComponent<SpriteRenderer>().sprite = _knifeSkin;
        _holdTime = 0;
        _fire = true;
    }

    /// <summary>
    /// Таймер спавна норжей.
    /// </summary>
    private void Timer()
    {
        if (_holdThrow)
        {
            _holdTime -= Time.deltaTime;

            if (_holdTime <= 0)
            {
                SpawnKnife();

                _holdThrow = false;
            }
        }
    }
}
