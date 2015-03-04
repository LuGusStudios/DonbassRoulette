using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class LuGusUIText : MonoBehaviour {

    public string key = "";
    public Text uiText = null;
    public bool richText = false;

    public void FindReferences()
    {
        if (uiText == null)
        {
            uiText = GetComponent<Text>();

            if (uiText == null)
            {
                Debug.LogError(name + " : Text was null!");
            }
        }
    }

    protected void AssignKey()
    {
        if (string.IsNullOrEmpty(key))
        {
            key = uiText.text;

            Debug.LogWarning(name + " : key was empty! using Text.text as key : " + key);
        }
    }

    // Use this for initialization
    void Start()
    {
        FindReferences();
        AssignKey();

        LugusResources.use.Localized.onResourcesReloaded += UpdateText;

        UpdateText();
    }

    protected void UpdateText()
    {
        FindReferences();
        string txt = LugusResources.use.GetText(key);

        if (richText)
        {
            txt = txt.Replace("{", "<");
            txt = txt.Replace("}", ">");
        }

        uiText.text = txt;
    }
}
