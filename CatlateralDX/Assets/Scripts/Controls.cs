using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

//by using a .json for controls, it should save players' settings across different executions of the game ..?
//for reference, unity keycodes!:
//https://gist.github.com/Extremelyd1/4bcd495e21453ed9e1dffa27f6ba5f69

public class Controls : MonoBehaviour {
    const StreamReader CONTROLSFILE = new StreamReader("controls.json").ReadToEnd();
    //JSON is a list of Name:obj. ie., Default:obj and Current:obj
    //each obj is a list of KeyName:KeyCode

    private Array<Object> file = JsonConvert.DeserializeObject<Array<Object>>(CONTROLSFILE); //sub something legit under object :sob:


    //are these public or private????does it matter
    const Dictionary<String, String> DEFAULT = file[0]; // first object in array File !
    Dictionary<String, String> current = file[1];

    
    void Start() {
        //


    }

    public EditControl(String button, String new) {
        current[button] = new;
    }
    public ResetControl(String button) {
        current.Remove(button);
    }

}