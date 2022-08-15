using UnityEngine;

public class BackgrndScript : MonoBehaviour
{
    public float _xRot, _yRot, _zRot;
    public float _xPos, _yPos;
    public float _xScale, _yScale;
    public float _speedBackgrndRotation = 10.0f;
    public float _timeChange = 5.0f;
    public bool _timerChange = false;

    private void Start()
    {
        RandoRot(); RandoPos(); RandoScale();
    }

    private void Update()
    {
        BackRotation();
    }

    private void Timer()
    {
        _timeChange -= Time.deltaTime;

        if (_timeChange <= 0)
        {
            _timerChange = false;

            RandoRot(); RandoPos(); RandoScale();
        }
    }

    private void BackRotation()
    {
        float rot = Time.deltaTime * _speedBackgrndRotation;
        transform.Rotate(rot * _xRot, rot * _yRot, rot * _zRot);

        if (_timerChange) Timer();
    }

    private void RandoRot()
    {
        _xRot = Random.Range(0, 2); _yRot = Random.Range(0, 2); _zRot = Random.Range(0, 2);

        _timeChange = 5.0f;
        _timerChange = true;
    }

    private void RandoPos()
    {
        _xPos = Random.Range(-4.5f, 4.6f); _yPos = Random.Range(-5.0f, 5.1f);
        transform.position = new Vector2(_xPos, _yPos);
    }

    private void RandoScale()
    {
        _xScale = Random.Range(2.5f, 6.6f); _yScale = Random.Range(2.5f, 6.6f);
        transform.localScale = new Vector2(_xScale, _yScale);
    }
}
