using UnityEngine;
#if !UNITY_WEBPLAYER && !UNITY_IPHONE && !UNITY_ANDROID && !UNITY_WP8
using System.IO;
#endif
using System.Collections;
using System.Collections.Generic;

public class LugusConfig : LugusSingletonRuntime<LugusConfigDefault>
{
	
}

public class LugusConfigDefault : LugusSingletonRuntime<LugusConfigDefault>
{
	#region Properties
	public ILugusConfigProfile User
	{
		get
		{
			if (_currentUser == null)
				ReloadDefaultProfiles();
			
			return _currentUser;
		}
		set
		{
			_currentUser = value;
		}
	}
	public ILugusConfigProfile System
	{
		get
		{
			if (_systemProfile == null)
				ReloadDefaultProfiles();
			
			return _systemProfile;
		}
		set
		{
			_systemProfile = value;
		}
	}
	public List<ILugusConfigProfile> AllProfiles
	{
		get
		{
			return _profiles;
		}
		set
		{
			_profiles = value;
		}
	}
	#endregion 
	
	#region Protected
	protected ILugusConfigProfile _systemProfile = null;	// Profile holding system variables, i.e. graphics and sound options.
	protected ILugusConfigProfile _currentUser = null;		// Profile holding user specific variables, i.e. character health and strength.
	protected List<ILugusConfigProfile> _profiles = new List<ILugusConfigProfile>();	// All profiles registered in this configuration, incl. system profile.
	#endregion
	
	#if !UNITY_WEBPLAYER && !UNITY_IPHONE && !UNITY_ANDROID && !UNITY_WP8
	// Reload all profiles found in the Config folder.
	public void ReloadDefaultProfiles()
	{
		_profiles = new List<ILugusConfigProfile>();
		_systemProfile = null;
		_currentUser = null;
		
		// Load the profiles found in the config folder of the application datapath
		// and try to set the latest user as the current user.
		// If no profiles could be found in the folder,
		// then create a default system and user profile.
		
		string configpath = Application.dataPath + "/Config/";

		if (!Directory.Exists(configpath))
		{
			Debug.LogWarning("LugusConfigDefault: Config folder didn't exist yet. Creating it.");

			Directory.CreateDirectory(configpath);
		}

		DirectoryInfo directoryInfo = new DirectoryInfo(configpath);
		FileInfo[] files = directoryInfo.GetFiles("*.xml");
		
		System.DateTime mostRecentUserSaveTime = new global::System.DateTime(2000, 01, 01);
		
		if (files.Length > 0)
		{
			// Create and load profiles
			foreach (FileInfo fileInfo in files)
			{
				string profileName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(".xml"));
				LugusConfigProfileDefault profile = new LugusConfigProfileDefault(profileName);
				profile.Load();
				
				if (profileName == "System")
				{
					_systemProfile = profile;
					Debug.Log("LugusConfigDefault: Found system profile.");
				}
				else
				{
					Debug.Log("LugusConfigDefault: Found user profile: " + profileName);
					
					// In most cases, the System config profile will contain a reference to the latest player profile (see below), which will override this.
					// However, in some rare cases (System file is missing or does not contain User.Latest, the system can default to the last saved non-system profile.
					// mostRecentUserSaveTime.CompareTo will return -1 if new value is more recent than itself.
					if (mostRecentUserSaveTime.CompareTo(fileInfo.LastWriteTime) < 0)
					{
						mostRecentUserSaveTime = fileInfo.LastWriteTime;
						_currentUser = profile;
					}
				}
				
				_profiles.Add(profile);
			}
		}
		
		
		// If the system profile wasn't found, create it.
		if (_systemProfile == null)
		{
			Debug.LogWarning("LugusConfigDefault: A system config profile was not found. Now creating one at runtime.");
			LugusConfigProfileDefault sysProfile = new LugusConfigProfileDefault("System");
			this.System = sysProfile;
			_profiles.Add(sysProfile);
		}
		
		// Set current user to the one saved in system profile.
		string lastestUser = _systemProfile.GetString("User.Latest", string.Empty);
		if (!string.IsNullOrEmpty(lastestUser))
		{
			ILugusConfigProfile playerProfile = _profiles.Find(profile => profile.Name == lastestUser);
			
			if (playerProfile != null)
			{
				_currentUser = playerProfile;
			}
			else if (_currentUser != null)
			{
				// If a player file was deleted, no file can be returned. In that case, default to _currentUser's default value of the latest save (see above).
				
				Debug.LogWarning("LugusConfigDefault: The system config profile's last user value referred to a file that doesn't exist anymore. Defaulting to latest saved file instead: " + _currentUser.Name + ".");
				
				// If we're defaulting to the latest save, might as well change the latest user in the system profile to a file that does exist.
				_systemProfile.SetString("User.Latest", _currentUser.Name, true);
				_systemProfile.Store();
			}
		}
		// If the system profile did not have a value for latest user, but there are other profiles available, default to the latest save (see above).
		// Display a warning to make developers aware of this.
		else if (_currentUser != null)	
		{
			Debug.LogWarning("LugusConfigDefault: The system config profile did not (yet) contain a value indicating the last user. Defaulting to latest saved file instead: " + _currentUser.Name + ".");
			
			// If we're defaulting to the latest save, might as well save that name from now on.
			_systemProfile.SetString("User.Latest", _currentUser.Name, true);
			_systemProfile.Store();
		}
		
		// If a current user couldn't be found at all (either listed in the system profile or by defaulting to the latest saved profile), create a new one.
		if (_currentUser == null)
		{
			Debug.LogWarning("LugusConfigDefault: A latest user profile was not found. Now creating one named \"Player\".");
			_currentUser = new LugusConfigProfileDefault("Player");
			_profiles.Add(_currentUser);
			
			// If we're creating a new user profile, might as well save it to the system profile from now on.
			_systemProfile.SetString("User.Latest", "Player", true);
			_systemProfile.Store();
		}
	}
	#else
	
	// Reload all profiles found in the Config folder.
	public void ReloadDefaultProfiles()
	{
		_profiles = new List<ILugusConfigProfile>();
		_systemProfile = null;
		_currentUser = null;
		
		// TODO: in the case of playerprefs, we have to save a separate playerprefs key indicating which profiles are available
		// for now, we just take System
		
		_systemProfile = new LugusConfigProfileDefault("System", new LugusConfigProviderPlayerPrefs("System") );
		_systemProfile.Load();
		_profiles.Add( _systemProfile );
		
		string lastestUser = _systemProfile.GetString("User.Latest", string.Empty);
		if (!string.IsNullOrEmpty(lastestUser))
		{
			_currentUser = new LugusConfigProfileDefault(lastestUser, new LugusConfigProviderPlayerPrefs(lastestUser) );
		}
		else
		{
			_currentUser = new LugusConfigProfileDefault("Player", new LugusConfigProviderPlayerPrefs("Player") );
		}
		
		_currentUser.Load();
		
		_profiles.Add(_currentUser);
	}
	#endif
	
	public void SaveProfiles()
	{
		
		if ((_systemProfile != null) && (_currentUser != null))
			_systemProfile.SetString("User.Latest", _currentUser.Name, true);
		
		foreach (ILugusConfigProfile profile in _profiles)
			profile.Store();
	}
	
	public ILugusConfigProfile FindProfile(string name)
	{
		return _profiles.Find(profile => profile.Name == name);
	}
	
}