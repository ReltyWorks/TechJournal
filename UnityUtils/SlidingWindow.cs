using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityUtils
{
    public class SlidingWindow : MonoBehaviour, IPointerEnterHandler,
                                 IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private float _openPosX = 0f;
        [SerializeField] private float _openPosY = 0f;
        [SerializeField] private float _closePosX = 0f;
        [SerializeField] private float _closePosY = 0f;
        [SerializeField] private float _moveSpeed = 1000f;

        private RectTransform _rectTransform;
        private Coroutine _moveWindow;
        private bool _isFix = false;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OpenWindow();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isFix)
                return;

            CloseWindow();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _isFix = !_isFix;

            if (_isFix)
                OnFixWindow();
            else
                UnFixWindow();
        }

        private void OpenWindow()
        {
            if (_moveWindow != null)
                StopCoroutine(_moveWindow);

            _moveWindow = StartCoroutine(MoveWindow(true));
        }

        private void CloseWindow()
        {
            if (_moveWindow != null)
                StopCoroutine(_moveWindow);

            _moveWindow = StartCoroutine(MoveWindow(false));
        }

        private void OnFixWindow()
        {
            if (_moveWindow != null)
                StopCoroutine(_moveWindow);

            _rectTransform.anchoredPosition = new Vector2(_openPosX, _openPosY);
        }

        private void UnFixWindow()
        {
            CloseWindow();
        }


        IEnumerator MoveWindow(bool isOpen)
        {
            Vector2 targetPos = isOpen ? new Vector2(_openPosX, _openPosY)
                                       : new Vector2(_closePosX, _closePosY);

            // 현재 위치와 목표 위치 사이의 거리가 0.1f보다 크면 계속 이동
            while (Vector2.Distance(_rectTransform.anchoredPosition,
                                    targetPos) > 0.1f)
            {
                _rectTransform.anchoredPosition = Vector2.MoveTowards(
                                                  _rectTransform.anchoredPosition,
                                                  targetPos,
                                                  _moveSpeed * Time.deltaTime);

                yield return null;
            }

            _rectTransform.anchoredPosition = targetPos;
            _moveWindow = null;
        }
    }
}