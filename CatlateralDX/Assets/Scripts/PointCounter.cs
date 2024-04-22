    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class PointCounter : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI titletext;
        public int points = 0;

        void Start() {
            titletext.text = "points: "+points‌;
        }
        public void UpdatePoints(int pts)
        {
            points += pts;
            titletext.text = "points: "+points‌;
            Debug.Log(titletext.text);
        }
    }