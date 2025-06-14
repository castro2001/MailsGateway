﻿using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Mail
{
    public interface ICredentialProvider
    {
        Task<(string Correo, string ClaveAplicacion)> ObtenerCredencialesAsync(string correo);

    }
}
