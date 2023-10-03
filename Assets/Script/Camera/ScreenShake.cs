using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public CinemachineVirtualCamera _camV;
    public float ShakeIntensity;
    public float ShakeTime;
    float _timer;
    private CinemachineBasicMultiChannelPerlin _clmp;

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin _clmp = _camV.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _clmp.m_AmplitudeGain = ShakeIntensity;
        _timer = ShakeTime;
    }

    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin _clmp = _camV.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _clmp.m_AmplitudeGain = 0f;
        _timer = 0;

        transform.position = new Vector3(transform.position.x, transform.position.y, -35);
        transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 0));
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0 ) 
            { 
                StopShake();
            }
        }
    }
}
