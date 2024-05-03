using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointCounter : MonoBehaviour
{
    public int points = 0;
    public int propscountInitial, propscountCurrent;

    [SerializeField] private GameObject floatingTextPrefab;

    void Start() {
        propscountInitial = GameObject.Find("Props").transform.childCount;
    }

    public void UpdatePoints(int pts, Vector3 position)
    {
        propscountCurrent = GameObject.Find("Props").transform.childCount;
        
        points += pts;

        if (floatingTextPrefab) {
            GameObject prefab = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMeshProUGUI>().text = "" + pts;
        }
    }
}