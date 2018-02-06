using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollWithSelection : MonoBehaviour 
{
	[SerializeField]
	private float               m_lerpTime;
	private ScrollRect          m_scrollRect;
	private Button[]            m_buttons;
	public int                 m_index;
	public float               m_verticalPosition;
	private bool                m_up;
	private bool                m_down;
	private bool                m_left;
	private bool                m_right;
	public int 				level;
	public int 				ammountOfLevels;

	public void Start()
	{
		m_scrollRect        = GetComponent<ScrollRect>();
		m_buttons           = GetComponentsInChildren<Button>();
		m_buttons[m_index].Select();
		level = 0;
		if(m_buttons.Length%4 == 0){
			ammountOfLevels = m_buttons.Length / 4;
		}
		else {
			ammountOfLevels = (m_buttons.Length / 4) + 1;
		}
		m_verticalPosition  = 1f - ((float)m_index / (m_buttons.Length - 1));
		
	}

	public void Update()
	{
		m_up    = Input.GetKeyDown(KeyCode.UpArrow);
		m_down  = Input.GetKeyDown(KeyCode.DownArrow);
		m_left    = Input.GetKeyDown(KeyCode.LeftArrow);
		m_right  = Input.GetKeyDown(KeyCode.RightArrow);

		if (m_up ^ m_down ^ m_left ^ m_right)
		{
			if (m_up)
			{
				if(m_index>=4)
					m_index = Mathf.Clamp(m_index - 4, 0, m_buttons.Length - 1);
				if(level>0)
					level--;
//				m_buttons[m_index].Select();
//				m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
			}
			else if (m_down)
			{
				if(m_index<= m_buttons.Length - 5)
					m_index = Mathf.Clamp(m_index + 4, 0, m_buttons.Length - 1);
				if(level < ammountOfLevels - 1)
					level++;
//				m_buttons[m_index].Select();
//				m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
			}
			else if(m_left)
			{
				if(m_index % 4 != 0)
					m_index = Mathf.Clamp(m_index - 1, 0, m_buttons.Length - 1);
			}
			else
			{
				if(m_index % 4 != 3)
					m_index = Mathf.Clamp(m_index + 1, 0, m_buttons.Length - 1);
			}

			m_buttons[m_index].Select();
//			m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
//			m_verticalPosition = 1 - (m_index / ((m_buttons.Length-1)/ 4));
			if(ammountOfLevels > 2)
            	m_verticalPosition = 1f - ((float)level / (ammountOfLevels - 1));

		}

		m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition, m_verticalPosition, Time.deltaTime / m_lerpTime);
	}
}
