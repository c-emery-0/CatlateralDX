    using UnityEngine;
    using UnityEngine.UI;

    public class PointCounter : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshPro _title;
        public int points = 0;

        private Start()‌{
            _title = GetComponent<TextMeshPro>();
        }
        public void UpdatePoints(int pts)
        {
            points += pts;
            _title.text = "points: "+(points‌);
        }
    }