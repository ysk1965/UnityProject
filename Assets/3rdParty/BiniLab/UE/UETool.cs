/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;

public class UETool {

	public static string ConvertToColorRichText(Color color, string str)
	{
		string colorStr = string.Format ("#{0:X2}{1:X2}{2:X2}", (int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
		return ConvertToColorRichText (colorStr, str);
	}

	public static string ConvertToColorRichText(string color, string str)
	{
		string format = "<color={0}>{1}</color>";
		return string.Format (format, color, str);
	}
}
