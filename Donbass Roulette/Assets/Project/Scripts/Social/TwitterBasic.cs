using UnityEngine;
using System.Collections;

public class TwitterBasic {

	// Make this a singleton
	protected static TwitterBasic _instance;
	private TwitterBasic() {}
	public static TwitterBasic use {
		get {
			if (_instance == null)
				_instance = new TwitterBasic();
			return _instance;
		}
	}

	private string Address = "http://www.twitter.com/intent/tweet";
    public string linkUrl = "";
    public string linkName = "";
    public string imageUrl = "";

	public void Share (string message)
	{	
		//string Address = "http://twitter.com/intent/tweet";
		Application.OpenURL(Address +
		                    "?text=" + WWW.EscapeURL(message + " " + linkUrl + " #LuGusStudios") +		                    
		                    "&amp;related=" + WWW.EscapeURL("LuGusStudios") +
		                    "&amp;lang=" + WWW.EscapeURL("en"));		
		Debug.Log ("Shared");
	}
		
	public void Share(string text, string url, string related, string lang="en")
	{
		Application.OpenURL(Address +
		                    "?text=" + WWW.EscapeURL(text) +
		                    "&amp;url=" + WWW.EscapeURL(url) +
		                    "&amp;related=" + WWW.EscapeURL(related) +
		                    "&amp;lang=" + WWW.EscapeURL(lang));
		Debug.Log ("Shared");
	}
}
