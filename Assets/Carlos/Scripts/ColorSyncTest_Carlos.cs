using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSyncTest_Carlos : MonoBehaviour
{
    [SerializeField]
    private Color _color;
    private Color _previousColor = default;

    private ColorSync_Carlos _colorSync;

    private bool _isColorInit;

    [SerializeField]
    private float _speed = 0.1f;

    [SerializeField]
    private Quaternion _rotation = default;
    private Quaternion _previousRotation = default;

    private void Awake()
    {
        // Get a reference to the color sync component
        _colorSync = GetComponent<ColorSync_Carlos>();
    }

    private void Update()
    {
        // init with a random color
        if (!_isColorInit)
        {
            _color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
                );
            _colorSync.SetColor(_color);
            _previousColor = _color;
            _isColorInit = true;
        }

        // If the color has changed (via the inspector), call SetColor on the color sync component.
        if (_color != _previousColor)
        {
            _colorSync.SetColor(_color);
            _previousColor = _color;
        }

        // Update rotation is changed via the inspector
        if (_rotation != _previousRotation)
        {
            _colorSync.SetRotation(_rotation);
            _previousRotation = _rotation;
        }

        // Update position with wasd and scale with qe
        if (Input.GetKey(KeyCode.W))
        {
            _colorSync.SetPosition(_colorSync.transform.position += Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _colorSync.SetPosition(_colorSync.transform.position += Vector3.left * _speed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            _colorSync.SetPosition(_colorSync.transform.position += Vector3.down * _speed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {
            _colorSync.SetPosition(_colorSync.transform.position += Vector3.right * _speed * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.Q))
        {
            _colorSync.SetScale(_colorSync.transform.localScale += Vector3.down * _speed * Time.deltaTime);


        }

        if (Input.GetKey(KeyCode.E))
        {
            _colorSync.SetScale(_colorSync.transform.localScale += Vector3.up * _speed * Time.deltaTime);


        }

    }


}
