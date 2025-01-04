using System;

namespace Yannyo.Common
{
	/// <summary>
	/// SmiliesInfo 的摘要说明。
	/// </summary>
	public class SmiliesInfo
	{

		private int m_id;	//Smilies的id
		private int m_displayorder;	//显示顺序
		private int m_type;	//类型,表情或主题图标
		private string m_code;	//代码
		private string m_url;	//对应图片URL

		///<summary>
		///Smilies的id
		///</summary>
		public int Id
		{
			get { return m_id;}
			set { m_id = value;}
		}
		///<summary>
		///显示顺序
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///类型,表情或主题图标
		///</summary>
		public int Type
		{
			get { return m_type;}
			set { m_type = value;}
		}
		///<summary>
		///代码
		///</summary>
		public string Code
		{
			get { return m_code;}
			set { m_code = value;}
		}
		///<summary>
		///对应图片URL
		///</summary>
		public string Url
		{
			get { return m_url;}
			set { m_url = value;}
		}
	}
}
