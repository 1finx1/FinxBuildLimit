using Rocket.API;
using System.Collections.Generic;

namespace FinxBuildLimit
{
    public class Config : IRocketPluginConfiguration
    {
       
        public float MaxHeight { get; set; }
        public List<RestrictedStructureInfo> RestrictedStructures { get; set; }

        public void LoadDefaults()
        {
            
            MaxHeight = 10.0f;
            
            {
                new RestrictedStructureInfo
                {

                    MaxHeight = 10.0f
                };
                
            };
        }
    }

    public class RestrictedStructureInfo
    {
        
        public float MaxHeight { get; set; }
    }
}


// Restrictedstructureinfo is not used for anything just too laxy to remove rn
