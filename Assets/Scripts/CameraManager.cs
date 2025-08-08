using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] [Range(-5, 5)] private float _defaultSensitivity = 1.0f;
    [SerializeField][Range(-5, 5)] private float _aimingSensitivity = 0.5f;
    [SerializeField] private Camera _camera = null;
    [SerializeField] private CinemachineVirtualCamera _playerCamera = null;
    [SerializeField] private CinemachineVirtualCamera _aimingCamera = null;
    [SerializeField] private CinemachineBrain _cameraBrain = null;
    [SerializeField] private LayerMask _aimLayer;

    public static Camera mainCamera {  get { return singleton._camera; } }
    public static CinemachineVirtualCamera playerCamera { get { return singleton._playerCamera; } }
    public static CinemachineVirtualCamera aimingCamera { get { return singleton._aimingCamera; } }
    public static float defaultSensitivity { get { return singleton._defaultSensitivity; } }
    public static float aimSensitivity { get { return singleton._aimingSensitivity; } }

    private static CameraManager _singleton = null;
    public static CameraManager singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = FindObjectOfType<CameraManager>();
            }
            return _singleton;
        }
    }

    private bool _aiming = false; 
    public bool aiming { get { return _aiming; } set { _aiming = value; } }
    private Vector3 _aimTargetPoint = Vector3.zero;
    public Vector3 aimTargetPoint { get { return _aimTargetPoint; } }

    private void Awake()
    {
        _cameraBrain.m_DefaultBlend.m_Time = 0.1f;
    }

    private void Update()
    {
        _aimingCamera.gameObject.SetActive(_aiming);
        SetAimTarget();
    }

    private void SetAimTarget()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));

        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, _aimLayer))
        {
            _aimTargetPoint = hit.point;
        }
        else
        {
            _aimTargetPoint = ray.GetPoint(1000.0f);
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_aimTargetPoint, 0.1f);
    }
    #endif
}
