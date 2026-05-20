using UnityEngine;

public class BackgroundWiggler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundImage;
    [SerializeField, Min(0f)] private float _smoothness = 5f;
    [SerializeField, Min(0f)] private float _speed = 0.005f;

    private float _timer = 0f;
    private float _perlinNoiseHorizontalStep = 0.01f;
    private float _perlinNoiseVerticalStep = 0.01f;
    private float _perlinNoiseShift = 0.5f;
    private Vector3 _offset;

    private void Update()
    {
        _timer += Time.deltaTime / _smoothness;

        _offset = new Vector3(
            (Mathf.PerlinNoise(_timer, (int)_timer * _perlinNoiseHorizontalStep) - _perlinNoiseShift) * _speed,
            (Mathf.PerlinNoise((int)_timer * _perlinNoiseVerticalStep, _timer) - _perlinNoiseShift) * _speed,
            0f);

        transform.position += _offset;

        if (transform.position.y <= -_backgroundImage.size.y / 4f)
            transform.position += _backgroundImage.size.y / 2f * Vector3.up;
        else if (transform.position.y >= _backgroundImage.size.y / 4f)
            transform.position += _backgroundImage.size.y / 2f * Vector3.down;

        if (transform.position.x <= -_backgroundImage.size.x / 4f)
            transform.position += _backgroundImage.size.x / 2f * Vector3.right;
        else if (transform.position.x >= _backgroundImage.size.x / 4f)
            transform.position += _backgroundImage.size.x / 2f * Vector3.left;
    }
}
