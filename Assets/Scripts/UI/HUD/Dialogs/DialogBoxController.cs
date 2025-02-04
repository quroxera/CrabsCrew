using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Data;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Scripts.UI.HUD.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [Header("Player UI Elements")] [SerializeField]
        private GameObject _playerDialogContainer;

        [SerializeField] private Text _playerText;
        [SerializeField] private Animator _playerAnimator;

        [Header("NPC UI Elements")] [SerializeField]
        private GameObject _npcDialogContainer;

        [SerializeField] private Text _npcText;
        [SerializeField] private Text _npcNameText;
        [SerializeField] private Image _npcPortrait;
        [SerializeField] private Animator _npcAnimator;

        [Header("Popup UI Elements")] [SerializeField]
        private GameObject _popupDialogContainer;

        [SerializeField] private Text _popupText;
        [SerializeField] private Animator _popupAnimator;

        [Header("Settings")] [SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")] [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private DialogData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;
        private Coroutine _currentPopupRoutine;
        private DialoguePostEffectsComponent _postEffects;
        private UnityEvent _onComplete;
        private bool _usePopup;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
            _postEffects = GetComponent<DialoguePostEffectsComponent>();
        }

        public void ShowDialog(object data, bool usePopup = false, UnityEvent onComplete = null)
        {
            _onComplete = onComplete;
            _data = null;
            _usePopup = usePopup;
            _currentSentence = 0;

            _playerText.text = string.Empty;
            _npcText.text = string.Empty;
            _popupText.text = string.Empty;

            _sfxSource.PlayOneShot(_open);

            if (_currentPopupRoutine != null)
            {
                StopCoroutine(_currentPopupRoutine);
                _currentPopupRoutine = null;
            }

            switch (data)
            {
                case DialogData dialogData:
                    _data = dialogData;
                    _postEffects.StartIncreaseToValue();
                    UpdateDialogUI();
                    break;
                case List<string> popupsContent:
                    _currentPopupRoutine = StartCoroutine(ShowPopupsSequentially(popupsContent));
                    break;
            }
        }

        private IEnumerator ShowPopupsSequentially(List<string> popupsContent)
        {
            var popupsCopy = new List<string>(popupsContent);

            foreach (string popupText in popupsCopy)
            {
                _popupText.text = popupText;

                ShowPopup();

                yield return new WaitUntil(() => !_popupAnimator.GetBool(IsOpen));
            }

            HideDialogBoxes();
            _currentPopupRoutine = null;
        }

        private IEnumerator TypeDialogText(Text targetText, string sentence)
        {
            targetText.text = string.Empty;

            foreach (var letter in sentence)
            {
                targetText.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void OnSkip()
        {
            if (_typingRoutine != null)
                return;

            StopTypeAnimation();

            if (_usePopup)
                _popupText.text = _popupText.text;
            else
            {
                var currentSentence = _data.Sentences[_currentSentence];
                if (currentSentence.IsHeroSpeaking)
                    _playerText.text = currentSentence.Sentence;
                else
                    _npcText.text = currentSentence.Sentence;
            }
        }

        public void OnContinue()
        {
            StopTypeAnimation();

            if (_usePopup)
            {
                _popupAnimator.SetBool(IsOpen, false);
                return;
            }

            _currentSentence++;

            var isDialogCompleted = _currentSentence >= _data.Sentences.Length;

            if (isDialogCompleted)
            {
                _onComplete?.Invoke();
                HideDialogBoxes();
            }
            else
                UpdateDialogUI();
        }

        private void UpdateDialogUI()
        {
            var currentSentence = _data.Sentences[_currentSentence];

            if (currentSentence.IsHeroSpeaking)
            {
                StopTypeAnimation();
                _playerDialogContainer.SetActive(true);
                _npcDialogContainer.SetActive(false);

                _playerAnimator.SetBool(IsOpen, true);
                _npcAnimator.SetBool(IsOpen, false);

                OnStartDialogAnimation(_playerText, currentSentence.Sentence);
            }
            else
            {
                StopTypeAnimation();
                _npcDialogContainer.SetActive(true);
                _playerDialogContainer.SetActive(false);

                _npcNameText.text = _data.NpcName;
                _npcPortrait.sprite = _data.NpcPortrait;
                _npcAnimator.SetBool(IsOpen, true);
                _playerAnimator.SetBool(IsOpen, false);

                OnStartDialogAnimation(_npcText, currentSentence.Sentence);
            }
        }

        private void ShowPopup()
        {
            StopTypeAnimation();

            _popupDialogContainer.SetActive(true);
            _npcDialogContainer.SetActive(false);
            _playerDialogContainer.SetActive(false);

            _popupAnimator.SetBool(IsOpen, true);

            OnStartDialogAnimation(_popupText, _popupText.text);
        }

        private void HideDialogBoxes()
        {
            _postEffects.StartFadeToZero();
            _playerAnimator.SetBool(IsOpen, false);
            _npcAnimator.SetBool(IsOpen, false);
            _popupAnimator.SetBool(IsOpen, false);

            _popupDialogContainer.SetActive(false);
            _sfxSource.PlayOneShot(_close);
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);

            _typingRoutine = null;
        }

        private void OnStartDialogAnimation(Text targetText, string sentence)
        {
            _typingRoutine = StartCoroutine(TypeDialogText(targetText, sentence));
        }
    }
}