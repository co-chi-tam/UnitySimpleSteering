using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SEEffect 
{
	public enum EEffectNodeType : byte {
		None = 0,
		YesNoCondition = 1,
		// TODO
		ExcuteEffectNode = 10
	}

	public class SEEffectReader {

		private int m_NodeCount;
		private SEBaseEffectNode m_CurrentNode;

		public SEBaseEffectNode CurrentNode {
			get { return m_CurrentNode; }
			set { m_CurrentNode = value; }
		}

		public SEEffectReader ()
		{
			m_NodeCount = 0;
		}

		public void LoadJson(string path) {
			var textAsset = Resources.Load <TextAsset> (path);
			if (textAsset != null) {
				var jsonDeserialize = MiniJSON.Json.Deserialize (textAsset.text) as Dictionary<string, object>;
				var effect = jsonDeserialize ["effect"] as Dictionary<string, object>;
				// Read current node - first node
				ReadNode (effect, true); 
			} else {
				throw new Exception ("Effect file not found. ERROR: " + path);
			}
		}

		private SEBaseEffectNode ReadNode(Dictionary<string, object> nodeData, bool firstNode = false) {
			m_NodeCount++;
			EEffectNodeType type = EEffectNodeType.None;
			if (nodeData.ContainsKey ("Method")) {
				type = EEffectNodeType.YesNoCondition;
			} else {
				type = EEffectNodeType.ExcuteEffectNode;
			}
			var id = type.ToString() + m_NodeCount;
            switch (type)
            {
                case EEffectNodeType.YesNoCondition:
                    {
                        SEYesNoNode yesNo = null;
                        if (firstNode)
                        {
                            m_CurrentNode = new SEYesNoNode();
                            yesNo = m_CurrentNode as SEYesNoNode;
                        }
                        else
                        {
                            yesNo = new SEYesNoNode();
                        }
                        yesNo.ID = id;
                        yesNo.NodeType = type;
                        var methodName = string.Empty;
                        var pars = ParseStringToObjects(nodeData["Method"].ToString(), out methodName);
                        yesNo.EffectParam.Method = methodName;
                        yesNo.EffectParam.Params = pars;
                        var yesMethods = nodeData["YesMethods"] as List<object>;
                        var noMethods = nodeData["NoMethods"] as List<object>;
                        for (int i = 0; i < yesMethods.Count; i++)
                        {
                            var yesData = yesMethods[i] as Dictionary<string, object>;
                            yesNo.YesMethods.Add(ReadNode(yesData));
                        }
                        for (int i = 0; i < noMethods.Count; i++)
                        {
                            var noData = noMethods[i] as Dictionary<string, object>;
                            yesNo.NoMethods.Add(ReadNode(noData));
                        }
                        return yesNo;
                    }
				default:
                case EEffectNodeType.ExcuteEffectNode:
                    {
                        SEExcuteEffectNode excuteMethod = null;
                        if (firstNode)
                        {
                            m_CurrentNode = new SEExcuteEffectNode();
                            excuteMethod = m_CurrentNode as SEExcuteEffectNode;
                        }
                        else
                        {
                            excuteMethod = new SEExcuteEffectNode();
                        }
                        excuteMethod.ID = id;
                        excuteMethod.NodeType = type;
                        var pars = nodeData["ExcuteMethods"] as List<object>;
                        for (int i = 0; i < pars.Count; i++)
                        {
                            var methodName = string.Empty;
                            var par = ParseStringToObjects(pars[i].ToString(), out methodName);
                            excuteMethod.ExcuteMethods.Add(new SEEffectParam() { Method = methodName, Params = par });
                        }
                        return excuteMethod;
                    }
			}
			return null;
		}

		private Dictionary<string, object> ParseStringToObjects (string data, out string methodName) {
            var dataMethod = data.Split('(');
            methodName = dataMethod [0];
            var paramsName = dataMethod [1].Remove (dataMethod [1].Length - 1, 1);
            var paramsDatas = paramsName.Split (',');
			var result = new Dictionary<string, object>();
            if (string.IsNullOrEmpty (paramsDatas[0]))
                return null;
			for (int i = 0, j = 0; i < paramsDatas.Length * 2; i+=2, j++) {
                var argument = paramsDatas[j].Split(':');
				result.Add (argument[0], argument[1])	; 
            }
            return result;
        }
	}
}
