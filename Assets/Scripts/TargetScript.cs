using UnityEngine;

public class TargetScript : ManagerScript
{
    #region Переменные

    private ManagerScript _gameManager;
    private bool _scaling = false;
    private Vector2 _localScale;
    private Vector2 _position;

    #endregion

    #region Методы

    /// <summary>
    /// Start.
    /// </summary>
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<ManagerScript>();

        _localScale = transform.localScale;
        _position = transform.position;

        ChangeSkin();
    }

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        Rotation();

        DestroyTarget();

        Scale();
    }

    /// <summary>
    /// Вращение мишени.
    /// </summary>
    private void Rotation() => transform.Rotate(0, 0, _targetSpeedRotation * Time.deltaTime * 100.0f);

    /// <summary>
    /// Уничтожение мишени.
    /// </summary>
    private void DestroyTarget()
    {
        if (_isDestroy)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "TargetSprite")
                    Destroy(child.gameObject);
                else
                {
                    Vector2 movingVec = new Vector2(0, 0);
                    transform.Translate(movingVec, Space.World);
                    child.parent = null;
                    Vector2 explosionVec = child.transform.position - transform.position;
                    child.gameObject.AddComponent<Rigidbody2D>();
                    child.gameObject.AddComponent<KnifeBehaviour>();
                    child.GetComponent<Rigidbody2D>().gravityScale = 10.0f;
                    child.GetComponent<Rigidbody2D>().mass = 10.0f;
                    child.GetComponent<Rigidbody2D>().AddForce(explosionVec * 50.0f, ForceMode2D.Impulse);
                }
            }

            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Фиксация ножа на мишени.
    /// </summary>
    /// <param name="obj"></param>
    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.transform.tag == "Knife")
        {
            if (_gameManager._throwScript._isTwisted)
            {
                _hits++;
                _gameManager.IncreaseScore(2.0f);
                _gameManager._throwScript.GetComponent<ThrowScript>()._isTwisted = false;
            }

            _hits++;
            _gameManager.IncreaseScore(1.5f);

            Transform freezeTransform = obj.transform;

            Destroy(obj.transform.GetComponent<Rigidbody2D>());
            Destroy(obj.transform.GetComponent<KnifeBehaviour>());

            obj.transform.SetParent(this.transform);
            obj.transform.position = freezeTransform.transform.position;
            obj.transform.rotation = freezeTransform.transform.rotation;

            _timeRestore = 0.06f;
            _scaling = true;
        }
    }

    /// <summary>
    /// Скейлинг мишени при попадании.
    /// </summary>
    private void Scale()
    {
        if (_scaling)
        {
            var xScale = transform.localScale.x + _displaceScale;
            var yPos = transform.position.y - _displaceYPos;

            transform.localScale = new Vector2(xScale, xScale);
            transform.position = new Vector2(transform.position.x, yPos);

            RestoreScale();
        }
    }

    /// <summary>
    /// Возврат размера и позиции.
    /// </summary>
    private void RestoreScale()
    {
        _timeRestore -= Time.deltaTime;

        if (_timeRestore < 0)
        {
            transform.localScale = _localScale;
            transform.position = _position;

            _scaling = false;
        }
    }

    /// <summary>
    /// Смена скина мишени.
    /// </summary>
    private void ChangeSkin()
    {
        transform.Find("TargetSprite").gameObject.GetComponent<SpriteRenderer>().sprite = _targetSkin;
    }

    #endregion
}
