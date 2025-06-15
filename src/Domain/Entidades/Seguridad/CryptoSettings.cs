using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Seguridad
{
    public class CryptoSettings
    {
            public string AESKey { get; set; } = string.Empty;
            public string HMACKey { get; set; } = string.Empty;
        
    }
}
