using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingItemUI : MonoBehaviour
{
   [SerializeField] private Image color;
   [SerializeField] private TextMeshProUGUI name;
   [SerializeField] private GameObject focus;
   private ColorType playerColor;
  
   public void OnInit(ColorType playerColor)
   {
      focus.SetActive(false);
      this.playerColor = playerColor;
   }

   public void SetColor(ColorType color,Material material)
   {
      name.text = color.ToString().ToUpper();
      this.color.color = material.color;
      if (color == this.playerColor)
      {
         focus.SetActive(true);
      }
      else
      {
         focus.SetActive(false);
      }
   }
}
