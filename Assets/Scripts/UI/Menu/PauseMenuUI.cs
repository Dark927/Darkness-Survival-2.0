using Assets;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Utilities.Math;

public class PauseMenuUI : MonoBehaviour, IMenuUI
{
    [SerializeField] private GameObject _pausePanel;

    [Header("Video Back - Transition Settings")]
    [SerializeField] private float _transitionDuration = 1f;
    [SerializeField] private Vector2 _firstEasePoint = new Vector2(0.897f, 0.023f);
    [SerializeField] private Vector2 _secondEasePoint = new Vector2(0.153f, 0.973f);

    private VideoPlayer _videoPlayer;
    private RawImage _rawImage;
    private Coroutine _activeRoutine = null;


    private void Awake()
    {
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        _rawImage = GetComponentInChildren<RawImage>();

    }

    private void Start()
    {
        _rawImage.color = new Color(0, 0, 0, 0);
        _videoPlayer.prepareCompleted += VideoPlayerReady;
        _videoPlayer.Prepare();
    }

    private void VideoPlayerReady(VideoPlayer videoPlayer)
    {
        _videoPlayer.Play();

        if (_activeRoutine != null)
        {
            StopCoroutine(_activeRoutine);
        }

        _activeRoutine = StartCoroutine(FadeOutBackgroundClipRoutine(_transitionDuration));
    }

    private IEnumerator FadeOutBackgroundClipRoutine(float duration)
    {
        float elapsedTime = 0f;
        Color startColor = _rawImage.color;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _rawImage.color = Color.Lerp(startColor, Color.white, BezierUtils.Bezier(elapsedTime / duration, _firstEasePoint, _secondEasePoint));

            yield return null;
        }

        _rawImage.color = Color.white;
        _activeRoutine = null;
    }

    private void OnDestroy()
    {
        _videoPlayer.targetTexture.Release();
    }
}
