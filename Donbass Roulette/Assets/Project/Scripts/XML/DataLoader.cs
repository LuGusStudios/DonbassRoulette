using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DataLoader : FileManager
{
	public string m_fileFolder;
	public GameData m_game;

	override protected void P_Save(string filePath) { }
	override protected void P_Load(string filePath)
	{
		//LoadData(File.ReadAllText(filePath));

        

        TextAsset levelFile = LugusResources.use.Shared.GetTextAsset(filePath);

        if (levelFile != LugusResources.use.errorTextAsset)
        {            
            LoadData(levelFile.text);
        }
        else
        {
            Debug.LogError("DataLoader: Missing level file.");
        }
	}
	
	
	// load
    new public void Load(string fileName)
    {
       // base.Load(m_fileFolder + fileName);

        base.Load(fileName);
    }

	private void LoadData(string xmlData)
	{
		TinyXmlReader parser = new TinyXmlReader(xmlData);

		while(parser.Read())
		{
			if((parser.tagType == TinyXmlReader.TagType.OPENING))
			{
				switch(parser.tagName)
				{
					case "Game": LoadGameData(parser); break;
					case "LeftUser": LoadUserComponentsData(parser, m_game.m_leftUser, "LeftUser"); break;
					case "RightUser": LoadUserComponentsData(parser, m_game.m_rightUser, "RightUser"); break;
				}
			}
		}
        
		m_game.InstantiateUsersComponents();        
	}
	private void LoadGameData(TinyXmlReader parser)
	{
		while(parser.Read("Game"))
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				switch(parser.tagName)
				{
                    case "Camera": LoadCameraData(parser); break;
					case "Structures": LoadStructuresData(parser); break;
				}
			}
		}
	}
	private void LoadUserComponentsData(TinyXmlReader parser, User user, string endLoop)
	{
		//TODO : get the string here instead of receiving by parameter ?
		while( parser.Read(endLoop) )
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				switch(parser.tagName)
				{
					case "UserData": LoadUserData(parser, user); break;
					case "AIData":
						AI ai = user as AI;
						if( ai ) LoadAIData(parser, ai);
						break;
				}
			}
		}
	}
	
	// game data
    public void LoadCameraData(TinyXmlReader parser)
    {
        while(parser.Read("Camera"))
        {
            if(parser.tagType == TinyXmlReader.TagType.OPENING)
            {
                switch(parser.tagName)
                {
                    case "Position":
                        m_game.m_camera.gameObject.MoveTo(GetVector3(parser.content, ';')).Time(1f).Execute(); // TODO : lock the camera during that operation
                        break;
                }
            }
        }

    }


	public void LoadStructuresData(TinyXmlReader parser)
	{
		while(parser.Read("Structures"))
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				switch(parser.tagName)
				{
					case "Left": LoadSidingStructuresData(parser, "Left", Side.Left); break;
					case "Right": LoadSidingStructuresData(parser, "Right", Side.Right); break;
				}
			}
		}
	}

	public void LoadSidingStructuresData(TinyXmlReader parser, string endLoop, Side side)
	{
		while(parser.Read(endLoop))
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				LoadStructureData(parser, side);
			}
		}
	}
	public void LoadStructureData(TinyXmlReader parser, Side side)
	{
		GameObject obj = null;
		while(parser.Read("Structure"))
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				switch(parser.tagName)
				{
					case "Name":
						obj = Instantiate(m_game.FindStructure(parser.content)) as GameObject;
						obj.GetComponentInChildren<Body>().m_side = side;
						User user = null;
						if(side == Side.Left)
							user = m_game.m_leftUser;
						else if(side == Side.Right)
							user = m_game.m_rightUser;
						else
							Debug.LogError("LoadStructureData received an unused Side variable");

						obj.transform.parent = user.transform;//m_game.m_map.transform;
						if(parser.content.StartsWith("Spawner"))    // this is bad and you should feel bad
							user.m_spawner = obj.transform;
						break;
					case "Position":
						obj.transform.position = GetVector3(parser.content, ';') + m_game.m_map.GetRandomStartingGroundPos();
						break;
				}
			}
		}
	}

	public Vector3 GetVector3(string reference, char separator)
	{
		string[] splitText = reference.Split(separator);
        Vector3 output = Vector3.zero;

        float textLength = splitText.Length;

        if (textLength >= 1)
        {
            output.x = float.Parse(splitText[0]);
            if (textLength >= 2)
            {
                output.y = float.Parse(splitText[1]);
                if (splitText.Length >= 3)
                {
                    output.z = float.Parse(splitText[2]);
                }
            }
        }

        return output;
	}

	// user data
	private void LoadUserData(TinyXmlReader parser, User user)
	{
		while(parser.Read("UserData"))
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				switch(parser.tagName)
				{
					case "AvailableFactories": LoadFactoriesData(parser, user); break;
					case "AvailableSpells": LoadSpellsData(parser, user); break;
					case "Money": LoadMoneyData(parser, user); break;
					case "Mana": LoadManaData(parser, user); break;
				}
			}
		}
	}
	private void LoadFactoriesData(TinyXmlReader parser, User user)
	{
		while(parser.Read("AvailableFactories"))
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				LoadFactoryData(parser, user);
			}
		}
	}
	private void LoadFactoryData(TinyXmlReader parser, User user)
	{
		while(parser.Read("Factory"))
		{
			if(parser.tagType == TinyXmlReader.TagType.OPENING)
			{
				switch(parser.tagName)
				{
					case "Name": 
						Factory factory = m_game.FindFactory(parser.content);
                        if (factory)
                            user.m_factories.Add(factory);
						break;
					case "Weight":
						AI ai = (user as AI);
						if(ai)
							ai.m_factoriesWeight.Add(float.Parse(parser.content));
						break;
				}
			}
		}
	}
	private void LoadSpellsData(TinyXmlReader parser, User user)
	{
		while(parser.Read("AvailableSpells"))
		{
			if((parser.tagType == TinyXmlReader.TagType.OPENING))
			{
				LoadSpellData(parser, user);
			}
		}
	}
	private void LoadSpellData(TinyXmlReader parser, User user)
	{
		while(parser.Read("Spell"))
		{
			if((parser.tagType == TinyXmlReader.TagType.OPENING))
			{
				switch(parser.tagName)
				{
					case "Name":
						Spell spell = m_game.FindSpell(parser.content);
						if(spell)
							user.m_spells.Add(spell);
						break;
				}
			}
		}
	}
	private void LoadMoneyData(TinyXmlReader parser, User user)
	{
		while(parser.Read("Money"))
		{
			if((parser.tagType == TinyXmlReader.TagType.OPENING))
			{
				switch(parser.tagName)
				{
					case "Base": user.m_money = int.Parse(parser.content); break;
					case "Regen": user.m_income = int.Parse(parser.content); break;
					case "Cooldown": user.m_incomeCooldown = float.Parse(parser.content); break;
					case "Multiplicator": user.m_incomeMultiplicator = float.Parse(parser.content); break;
					case "Price": user.m_incomePrice = int.Parse(parser.content); break;
					case "PriceMultiplicator": user.m_incomePriceMultiplicator = float.Parse(parser.content); break;
					case "RateMultiplicator": user.m_incomeRateMultiplicator = float.Parse(parser.content); break;
					case "RatePrice": user.m_incomeRatePrice = int.Parse(parser.content); break;
					case "RatePriceMultiplicator": user.m_incomeRatePriceMultiplicator = float.Parse(parser.content); break;
				}
			}
		}
	}
	private void LoadManaData(TinyXmlReader parser, User user)
	{
		while(parser.Read("Mana"))
		{
			if((parser.tagType == TinyXmlReader.TagType.OPENING))
			{
				switch(parser.tagName)
				{
					case "Base": user.m_manaMax = int.Parse(parser.content); user.Initialize(); break;
					case "Regen": user.m_manaRegen = int.Parse(parser.content); break;
					case "Cooldown": user.m_manaRegenCooldown = float.Parse(parser.content); break;
					case "Multiplicator": user.m_manaRegenMultiplicator = float.Parse(parser.content); break;
					case "Price": user.m_manaRegenPrice = int.Parse(parser.content); break;
					case "PriceMultiplicator": user.m_manaRegenPriceMultiplicator = float.Parse(parser.content); break;
				}
			}
		}
	}

	// ai data
	private void LoadAIData(TinyXmlReader parser, AI ai)
	{
		while(parser.Read("AIData"))
		{
			if((parser.tagType == TinyXmlReader.TagType.OPENING))
			{
				switch(parser.tagName)
				{
					case "Behaviour": break; // TODO
					case "TrySpawnCooldown": ai.m_trySpawnCooldown = float.Parse(parser.content); break;
					case "ForcedSpawnCooldown": ai.m_forcedSpawnCooldown = float.Parse(parser.content); break;
					case "SpellCastCooldown": ai.m_spellCastCooldown = float.Parse(parser.content); break;
					case "SpellCastRange": ai.m_spellCastRange = float.Parse(parser.content); break;
					case "SafetyRange": ai.m_safetyRange = float.Parse(parser.content); break;
					case "SafetyMana": ai.m_safetyMana = float.Parse(parser.content); break;
				}
			}
		}
	}

}
