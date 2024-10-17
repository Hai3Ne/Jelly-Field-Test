using Runtime.Manager;
using Runtime.Model;
using Runtime.ScriptTableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.View
{
    public class MissionVo : MonoBehaviour
    {
        [SerializeField] private Image imageColor;
        [SerializeField] private TextMeshProUGUI txtMission;
        
        [SerializeField] private GameObject complete;

        private LevelMission _data;
        public LevelMission Data => _data;
        public void SetData(LevelMission data)
        {
            _data = new LevelMission()
            {
                countColorMission = data.countColorMission,
                colorMission = data.colorMission
            };
            txtMission.text = data.countColorMission.ToString();
          
            imageColor.color = ColorForJelly(_data.colorMission);
            txtMission.gameObject.SetActive(!(_data.countColorMission <= 0));
            complete.SetActive(_data.countColorMission <= 0);
        }

        public void UpdateStateMission()
        {
            _data.countColorMission--;
            txtMission.gameObject.SetActive(!(_data.countColorMission <= 0));
            complete.SetActive(_data.countColorMission <= 0);
            txtMission.text = _data.countColorMission.ToString();
        }
        
        private Color ColorForJelly(JellyColor color)
        {
            switch (color)
            {
                case JellyColor.Red:
                    return Color.red;
                case JellyColor.Green:
                    return Color.green;
                case JellyColor.Blue:
                    return Color.blue;
                case JellyColor.Yellow:
                    return Color.yellow;
                case JellyColor.Purple:
                    return new Color(0.5f, 0, 0.5f);  // Màu tím
                default:
                    return Color.clear;
            }
        }
        
    }
}