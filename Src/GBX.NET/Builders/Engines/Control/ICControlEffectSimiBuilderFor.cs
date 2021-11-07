using GBX.NET.Engines.Control;
using System.Collections.Generic;

namespace GBX.NET.Builders.Engines.Control
{
    public interface ICControlEffectSimiBuilderFor :
        IBuilderForTMSX<ICControlEffectSimiBuilder, CControlEffectSimi>,
        IBuilderForTMU<ICControlEffectSimiBuilder, CControlEffectSimi>,
        IBuilderForTMUF<ICControlEffectSimiBuilder, CControlEffectSimi>,
        IBuilderForTM2<ICControlEffectSimiBuilder, CControlEffectSimi>
    {
        
    }
}