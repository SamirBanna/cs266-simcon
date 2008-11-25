using System;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.Core.Transforms;

#if NET_CF20
[assembly: ServiceDeclaration(DssServiceDeclaration.Transform, SourceAssemblyKey = @"cf.simulationtutorial1.y2006.m06, version=0.0.0.0, culture=neutral, publickeytoken=1e84430c676e9380")]
#else
[assembly: ServiceDeclaration(DssServiceDeclaration.Transform, SourceAssemblyKey = @"simulationtutorial1.y2006.m06, version=0.0.0.0, culture=neutral, publickeytoken=1e84430c676e9380")]
#endif
#if !URT_MINCLR
[assembly: System.Security.SecurityTransparent]
[assembly: System.Security.AllowPartiallyTrustedCallers]
#endif

namespace Dss.Transforms.TransformSimulationTutorial1
{

    public class Transforms: TransformBase
    {
        static Transforms()
        {
            Register();
        }
        public static void Register()
        {
        }
    }
}

