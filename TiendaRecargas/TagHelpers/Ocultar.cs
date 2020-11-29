using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(Ocultar))]
    public class OcultarHelper : TagHelper
    {       
        public bool Ocultar { get; set; }     
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {           
            if (Ocultar)
            {
                output.SuppressOutput();
            }
        }
    }
}
