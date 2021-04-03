/*
 *  BitmapFont Importer
 *  Choi yoon bin, NHN StarFish
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.IO;

public class UEBitmapFontImporter : EditorWindow{

	//////////////////////////////////////////////////////////////////////////////////////////
	// EditorWindow

	protected Texture texture;

	protected Sprite[] sprites = null;
	protected string spriteName = "";

	protected void OnGUI () {
		
		GUILayout.Label ("Sprite", EditorStyles.boldLabel);
		this.texture = (Texture)EditorGUILayout.ObjectField (this.texture, typeof(Texture), true);
		
		if(this.texture != null)
		{
			string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(texture));
			string spritePath = rootPath + "/" + Path.GetFileNameWithoutExtension(texture.name);

			if(this.sprites == null && !this.spriteName.Equals(texture.name))
			{
				string path = spritePath.Substring(spritePath.IndexOf("Resources/") + 10);
				this.sprites = Resources.LoadAll<Sprite> (path);
				this.spriteName = texture.name;
			}

			if(GUILayout.Button ("UPDATE ALL"))
			{
				if (!System.IO.Directory.Exists(Application.dataPath + rootPath.TrimStart("Assets".ToCharArray()) + "/export")) {
					AssetDatabase.CreateFolder(rootPath, "export");
				}
				
				foreach(Sprite sp in sprites)
				{
					TextAsset textAsset = AssetDatabase.LoadAssetAtPath (rootPath + "/" + sp.name + ".txt", typeof(TextAsset)) as TextAsset;
					if(textAsset == null)
						textAsset = AssetDatabase.LoadAssetAtPath (rootPath + "/data/" + sp.name + ".txt", typeof(TextAsset)) as TextAsset;
					if(textAsset != null)
					{
						string exportPath = rootPath + "/export/" + Path.GetFileNameWithoutExtension(textAsset.name);
						string materialPath = rootPath + "/export/" + Path.GetFileNameWithoutExtension(texture.name);
						
						Generate(textAsset ,exportPath, materialPath, sp.texture, sp.rect.x, (sp.texture.height - sp.rect.y - sp.rect.height));
					}
				}
			}

			foreach(Sprite sp in sprites)
			{
				Debug.Log("sp.name " + sp.name);

				TextAsset textAsset = AssetDatabase.LoadAssetAtPath (rootPath + "/" + sp.name + ".txt", typeof(TextAsset)) as TextAsset;
				if(textAsset == null)
					textAsset = AssetDatabase.LoadAssetAtPath (rootPath + "/data/" + sp.name + ".txt", typeof(TextAsset)) as TextAsset;
				if(textAsset != null)
				{
					string exportPath = rootPath + "/export/" + Path.GetFileNameWithoutExtension(textAsset.name);
					string materialPath = rootPath + "/export/" + Path.GetFileNameWithoutExtension(texture.name);
				
					Font font = AssetDatabase.LoadAssetAtPath (exportPath + ".fontsettings", typeof(Font)) as Font;

					if(font == null)
					{

						GUILayout.BeginHorizontal();
						GUILayout.Label(sp.name, GUILayout.MaxWidth(position.width - BUTTON_WIDTH - 10));
						if(GUILayout.Button ("CREATE", GUILayout.Width(BUTTON_WIDTH)))
						{
							Generate(textAsset ,exportPath, materialPath, sp.texture, sp.rect.x, (sp.texture.height - sp.rect.y - sp.rect.height));

						}
						GUILayout.EndHorizontal();
					}
					else
					{
						GUILayout.BeginHorizontal();
						GUILayout.Label(sp.name, GUILayout.MaxWidth(position.width - BUTTON_WIDTH - 10));
						if(GUILayout.Button ("UPDATE", GUILayout.Width(BUTTON_WIDTH)))
						{
							Generate(textAsset ,exportPath, materialPath, sp.texture, sp.rect.x, (sp.texture.height - sp.rect.y - sp.rect.height));

						}
						GUILayout.EndHorizontal();
					}
				}
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////
	// public

	[MenuItem("Assets/Import Bitmap Font")]
	public static void ShowImportBitmapFontWindow()
	{
		UEBitmapFontImporter window = (UEBitmapFontImporter)EditorWindow.GetWindow (typeof (UEBitmapFontImporter));
		window.minSize = new Vector2 (200, 200);
		window.maxSize = new Vector2 (200, 200);
		window.Show ();
	}

	//////////////////////////////////////////////////////////////////////////////////////////
	// private

	private const float BUTTON_WIDTH = 80f; 

	/// <summary>
	/// Generate the bitmap font.
	/// </summary>
	/// <param name="textAsset">Text file including bitmap font information.</param>
	/// <param name="exportPath">Path for exported font files.</param>
	/// <param name="materialPath">Material path for all fonts.</param>
	/// <param name="texture">Full Texture for bitmap font.</param>
	/// <param name="offsetX">Start position X of this font's texture area in full texture.</param>
	/// <param name="offsetY">Start position Y of this font's texture area in full texture.</param>
	private static void Generate(TextAsset textAsset, string exportPath, string materialPath, Texture2D texture, float offsetX = 0f, float offsetY = 0f)
	{

		Debug.Log("Proccess Font " + textAsset.name );

		////////////////////////////// Parsing Bitmap font information //////////////////////////////
		//Get base info
		string[] lines = textAsset.text.Split(new string[]{"\n"}, StringSplitOptions.RemoveEmptyEntries);

		List<string> chaInfoList = new List<string> ();
		string face = "BitmapFont";

		foreach(string line in lines)
		{
			if (line.StartsWith ("char id"))
				chaInfoList.Add (line);

			if(line.StartsWith("info="))
			{
				string[] values = line.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
				foreach(string value in values)
				{
					if(value.StartsWith("face")) face = GetStringValue(value, "face");
				}
			}
		}

		//Get texuture size
		float texW = texture.width;
		float texH = texture.height;

		//Get  Character infomation from textfile
		CharacterInfo[] charInfos = new CharacterInfo[chaInfoList.Count];
		Rect uvRect, vertRect;

		for (int i = 0; i < chaInfoList.Count; i++)
		{
			string[] values = chaInfoList[i].Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);

			CharacterInfo charInfo = new CharacterInfo();

			uvRect = new Rect();
			vertRect = new Rect();

			foreach(string value in values)
			{
				if(value.StartsWith("id=")) charInfo.index = (int)GetFloatValue(value, "id");
				if(value.StartsWith("xadvance=")) charInfo.advance = (int)GetFloatValue(value, "xadvance");

				if(value.StartsWith("x=")) uvRect.x = (offsetX + GetFloatValue(value, "x")) / texW;
				if(value.StartsWith("y=")) uvRect.y = (offsetY + GetFloatValue(value, "y")) / texH;
				if(value.StartsWith("width=")) uvRect.width = ((float) GetFloatValue(value, "width")) / texW;
				if(value.StartsWith("height=")) uvRect.height = ((float) GetFloatValue(value, "height")) / texH;

				if(value.StartsWith("xoffset=")) vertRect.x = GetFloatValue(value, "xoffset");
				if(value.StartsWith("yoffset=")) vertRect.y = GetFloatValue(value, "yoffset");
				if(value.StartsWith("width=")) vertRect.width = (float) GetFloatValue(value, "width");
				if(value.StartsWith("height=")) vertRect.height = (float) GetFloatValue(value, "height");
			}

			uvRect.y = 1f - uvRect.y - uvRect.height;
			charInfo.uvTopLeft = new Vector2 (uvRect.x, uvRect.yMax);
			charInfo.uvTopRight = new Vector2 (uvRect.xMax, uvRect.yMax);
			charInfo.uvBottomLeft = new Vector2 (uvRect.x, uvRect.y);
			charInfo.uvBottomRight = new Vector2 (uvRect.xMax, uvRect.y);

			vertRect.y = -vertRect.y;
			vertRect.height = -vertRect.height;
			charInfo.minX = (int)vertRect.xMin;
			charInfo.maxX = (int)vertRect.xMax;
			charInfo.minY = (int)vertRect.yMin;
			charInfo.maxY = (int)vertRect.yMax;

			charInfos[i] = charInfo;
		}

		////////////////////////////// Create Material //////////////////////////////
		//Check exist
		Material mat = AssetDatabase.LoadAssetAtPath (materialPath + ".mat", typeof(Material)) as Material;
		if(mat == null)
		{
			//Create new Material
			mat = new Material(Shader.Find("UI/Default"));
			mat.mainTexture = texture;
			AssetDatabase.CreateAsset(mat, materialPath + ".mat");
		}
		else 
		{
			//Update Exist Material
			mat.mainTexture = texture;
			AssetDatabase.SaveAssets();
		}

		////////////////////////////// Create Font //////////////////////////////
		//Check exist
		Font font = AssetDatabase.LoadAssetAtPath (exportPath + ".fontsettings", typeof(Font)) as Font;
		if(font == null)
		{
			//Create new Font
			font = new Font();
			font.material = mat;
			font.name = face;
			font.characterInfo = charInfos;
			AssetDatabase.CreateAsset(font, exportPath + ".fontsettings");

			Debug.Log("Created");
		}
		else
		{
			//Update Exist font
			font.material = mat;
			font.name = face;
			font.characterInfo = charInfos;
			AssetDatabase.SaveAssets();

			Debug.Log("Updated");
		}
	}

	private static float GetFloatValue(string content, string name)
	{
		return float.Parse(content.Split (new string[]{"="}, StringSplitOptions.RemoveEmptyEntries) [1]);
	}

	private static string GetStringValue(string content, string name)
	{
		return content.Split (new string[]{"="}, StringSplitOptions.RemoveEmptyEntries) [1];
	}	
}