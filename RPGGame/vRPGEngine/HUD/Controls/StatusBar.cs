using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD.Controls
{
    [Flags()]
    public enum TextType : int
    {
        None                = 0,
        Value               = 1,
        Percentage          = 2,
        OutOfMaxValue       = 4,
        OutOfMaxPercentage  = 8,
        Both                = Value | Percentage
    }

    public sealed class StatusBar : Control
    {
        #region Fields
        private IDisplayElement element;

        private int min;
        private int max;
        private int value;

        private bool showText;

        private TextType textType;
        #endregion

        #region Properties
        public int Min
        {
            get
            {
                return min;
            }
            set
            {
                min = value;

                NotifyPropertyChanged("Min");

                if (this.value < min) Value = min;
            }
        }
        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;

                NotifyPropertyChanged("Max");

                if (this.value > max) Value = max;
            }
        }
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = MathHelper.Clamp(value, min, max);

                NotifyPropertyChanged("Value");
            }
        }

        public bool ShowText
        {
            get
            {
                return showText;
            }
            set
            {
                showText = value;

                NotifyPropertyChanged("ShowText");
            }
        }
        public TextType TextType
        {
            get
            {
                return textType;
            }
            set
            {
                textType = value;

                NotifyPropertyChanged("TextType");
            }
        }
        #endregion

        public StatusBar(int min, int max, int initial = 0)
                : base()
        {
            RegisterProperty("Min", () => Min, (o) => Min = (int)o);
            RegisterProperty("Max", () => Max, (o) => Max = (int)o);
            RegisterProperty("Value", () => Value, (o) => Value = (int)o);
            RegisterProperty("ShowText", () => ShowText, (o) => ShowText = (bool)o);
            RegisterProperty("TextType", () => TextType, (o) => TextType = (TextType)o);
        }
    }
}
