using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;

    private PlayerControls _playerControls;
    private Vector2 _previousValue;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        _playerControls.Enable();
    }

    void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Start()
    {
        _playerControls.Touch.PrimaryContact.canceled += Ended;
        _playerControls.Touch.PrimaryContact.started += Ended;
        _playerControls.Touch.PrimaryPosition.performed += ProcessedTouchPrimary;
    }

    private void Ended(InputAction.CallbackContext obj)
    {
        _previousValue = Vector2.zero;
    }

    private void ProcessedTouchPrimary(InputAction.CallbackContext ctx)
    {
        Vector2 currentValue = ctx.ReadValue<Vector2>();

        if (_previousValue == Vector2.zero)
        {
            _previousValue = currentValue;
        }

        if (_previousValue == currentValue)
        {
            return;
        }

        Vector2 delta = currentValue - _previousValue;
        _previousValue = currentValue;

        transform.position += new Vector3(delta.x, 0, 0) * Time.deltaTime * _speed;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        _playerControls.Touch.PrimaryContact.canceled -= Ended;
        _playerControls.Touch.PrimaryPosition.performed -= ProcessedTouchPrimary;
    }
}