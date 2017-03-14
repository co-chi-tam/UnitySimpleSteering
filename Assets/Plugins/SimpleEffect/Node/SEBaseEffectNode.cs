
using System;

namespace SEEffect
{
	public class SEBaseEffectNode : SEEffectInfo
	{
		protected string m_ID;

		public string ID { 
			get { return m_ID; } 
			set { m_ID = value; } 
		}

		protected EEffectNodeType m_NodeType;

		public EEffectNodeType NodeType {
			get { return m_NodeType; }
			set { m_NodeType = value; }
		}

		public SEBaseEffectNode () : base ()
		{
			this.m_NodeType = EEffectNodeType.None;
			this.m_ID = string.Empty;
		}
	}
}

