using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class MenuItemUIModel {
        
        public object Data { get; set; }
        public string Text { get; set; }
        public Color? Color { get; set; }
        public Action<MenuItemUI> ContextAction { get; set; } 
        public string SelectSound { get; set; }

        public MenuItemUIModel(object data, string text, Color? color = null, Action<MenuItemUI> contextAction = null, string selectSound = null) 
        {
            Data = data;
            Text = text;
            Color = color;
            ContextAction = contextAction;
            SelectSound = selectSound;
        }
    }
}