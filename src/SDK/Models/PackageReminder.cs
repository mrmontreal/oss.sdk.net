//
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace OneSpanSign.API
{
	
	
	internal class PackageReminder
	{
		
		// Fields
		
		// Accessors
		    
    [JsonProperty("date")]
    public Nullable<DateTime> Date
    {
                get; set;
        }
    
		    
    [JsonProperty("sentDate")]
    public Nullable<DateTime> SentDate
    {
                get; set;
        }
    
		
	
	}
}