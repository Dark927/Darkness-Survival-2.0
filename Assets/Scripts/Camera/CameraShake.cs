using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Settings.CameraManagement.Shake;
using Utilities.ErrorHandling;

namespace Settings.CameraManagement
{
    public class CameraShake
    {
        #region Fields 

        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineBasicMultiChannelPerlin _cameraPerlin;
        private float _defaultShakeFrequency;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion


        #region Methods 

        public CameraShake(CinemachineVirtualCamera virtualCamera)
        {
            _virtualCamera = virtualCamera;
            _cameraPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _defaultShakeFrequency = _cameraPerlin != null ? _cameraPerlin.m_FrequencyGain : 0;
        }


        public void Invoke(float timeInSec, float amplitude)
        {
            Invoke(timeInSec, amplitude, _defaultShakeFrequency); 
        }


        public void Invoke(ShakeSettings settings)
        {
            Invoke(settings.Time, settings.Amplitude, settings.Frequency);
        }


        public void Invoke(float timeInSec, float amplitude, float frequency)
        {
            if (_cameraPerlin == null)
            {
                ErrorLogger.LogComponentIsNull(nameof(_cameraPerlin), nameof(CinemachineBasicMultiChannelPerlin));
                return;
            }

            if ((_cancellationTokenSource == null) || _cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
            }

            ShakeTask(timeInSec, amplitude, frequency, _cancellationTokenSource.Token).Forget();
        }

        public void StopAll()
        {
            _cancellationTokenSource.Cancel();
        }

        private async UniTask ShakeTask(float timeInSec, float amplitude, float frequency, CancellationToken token)
        {
            _cameraPerlin.m_AmplitudeGain += amplitude;
            _cameraPerlin.m_FrequencyGain = frequency;

            await UniTask.WaitForSeconds(timeInSec, cancellationToken: token);

            _cameraPerlin.m_AmplitudeGain -= amplitude;

            if (_cameraPerlin.m_AmplitudeGain < 0)
            {
                _cameraPerlin.m_AmplitudeGain = 0;
            }

            return;
        }

        #endregion
    }
}
