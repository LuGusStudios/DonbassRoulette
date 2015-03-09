using UnityEngine;
using System.Collections;

public class SocialShare : LugusSingletonRuntime<SocialShareBasic> 
{
			
}

public class SocialShareBasic : LugusSingletonRuntime<SocialShareBasic>
{
	public FacebookBasic facebook = null;
	public TwitterBasic twitter = null;

	public override void InitializeSingleton ()
	{
		base.InitializeSingleton ();
		facebook = FacebookBasic.use;
		twitter = TwitterBasic.use;

        string linkUrl = "www.battlefordonetsk.com";
        string linkName = "BattleForDonetsk.com";
        string imageUrl = "https://lh3.googleusercontent.com/-gjxoCu8Fu3c/AAAAAAAAAAI/AAAAAAAAAAA/Uji17DdykF4/photo.jpg";

        facebook.linkUrl = linkUrl;
        facebook.linkName = linkName;
        facebook.imageUrl = imageUrl;

        twitter.linkUrl = linkUrl;
        twitter.linkName = linkName;
	}

	public void InitializeSocial()
	{
		facebook.Initialize();
	}
}