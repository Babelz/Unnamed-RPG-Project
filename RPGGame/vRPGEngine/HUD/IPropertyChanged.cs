using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public sealed class PropertyChangedEventArgs : EventArgs
    {
        #region Properties
        public string PropertyName
        {
            get;
            private set;
        }
        #endregion

        public PropertyChangedEventArgs(string ropertyName)
        {
            PropertyName = ropertyName;
        }
    }

    public interface IPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }

    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs args);
}
