using System;
using System.Collections;

namespace SEEffect
{
	public struct SEObjectProperty<T> {

	   	private string m_Name;
		private T m_Value;
		private bool m_EnableEdit;
	    private Action<T, T> m_EventOnChange;

	    public string Name { 
			get { return m_Name; } 
			set { 
				if (m_EnableEdit == false)
					return;
				m_Name = value; 
			} 
		}

	    public T Value { 
			get { return m_Value; } 
			set { 
				if (m_EnableEdit == false)
					return;
				if (m_EventOnChange != null) {  
					m_EventOnChange(m_Value, value); 
				} 
				m_Value = value; 
			} 
		}

		public Action<T, T> OnChange { 
			get { return m_EventOnChange; } 
			set { m_EventOnChange = value; } 
		}

		public bool EnableEdit
		{
			get { return m_EnableEdit; }
			set { m_EnableEdit = value; }
		}

		public SEObjectProperty(string name, bool enable = true)
	    {
	        this.m_Name = name;
	        this.m_Value = default(T);
			this.m_EventOnChange = null;
			this.m_EnableEdit = enable;
	    }

		public SEObjectProperty(string name, T value, bool enable = true)
		{
			this.m_Name = name;
			this.Value = value;
			this.m_EventOnChange = null;
			this.m_EnableEdit = enable;
		}

		public void AddEventListener(Action<T, T> listener) {
			m_EventOnChange -= listener;
			m_EventOnChange += listener;
		}

		public void RemoveEventListener(Action<T, T> listener) {
			m_EventOnChange += listener;
		}

		public void RemoveAllListener() {
			m_EventOnChange = null;
		}

	    public void SetName(string name) {
			if (m_EnableEdit == false)
				return;
	        m_Name = name;
	    }

	    public string GetName() {
	        return m_Name;
	    }

	    public void SetValue(T value)
	    {
			if (m_EnableEdit == false)
				return;
	        if (m_EventOnChange != null)
	        {
	            m_EventOnChange(m_Value, value);
	        }
	        m_Value = value;
	    }

	    public T GetValue() {
	        return m_Value;
	    }

	}
}
