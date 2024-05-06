using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Codebase.UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private Button _clickButton;
        [SerializeField] private TMP_Text _clicksCountLabel;

        private int _clicksCount;

        private void Start()
        {
            UpdateClicksCountLabel();
        }

        private void OnEnable()
        {
            _clickButton.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _clickButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _clicksCount++;
            UpdateClicksCountLabel();
        }

        private void UpdateClicksCountLabel()
        {
            _clicksCountLabel.text = _clicksCount.ToString();
        }
    }
}
