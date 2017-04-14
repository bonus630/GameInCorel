using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameInCorel
{
    public static class Extensions
    {
        public static Corel.Interop.VGCore.Layer GetLayerByName(this Corel.Interop.VGCore.Page p,string name)
        {
            for (int i = 1; i <= p.AllLayers.Count; i++)
			{
			    if(p.AllLayers[i].Name.Equals(name))
                    return p.AllLayers[i];
			}
            return null;
        }
    }
}
