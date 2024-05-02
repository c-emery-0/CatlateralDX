private ParticleSystem trailPartical; 

public Color StartColor
{
    set
    {
        var main = trailPartical.main;
        main.startColor = value;
    }
}

void Start()
{
    trailPartical = GetComponent<ParticleSystem>();
    StartColor = Color.red;
}

OR {
    ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
main.startColor = Color.blue;
}

^^‌for fucked up start color not working?
vv for popup text

void ShowDamage(string text) {
    if (floatingTextPrefab) {
        GameObject prefab = Instantiate(floatingTextPrefab, position of where it is going to go, Quaternion.identity);
        prefab.GetComponentInChildren<TextMeshProGUIWhatever>().text = text;
    }
    //floating text prefab is a prefab of floattextParent
}