using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.TagHelpers.Enums;

namespace TiendaRecargas.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(Validate))]
    public class ValidateHelper : TagHelper
    {       
        public ValidateType Validate { get; set; }     
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {           
            if (Validate == ValidateType.Decimal)
            {
                var sdfsdf = output.TagName;
                var atr = output.Attributes["data-val"];

                ;
            }
        }
    }
}
