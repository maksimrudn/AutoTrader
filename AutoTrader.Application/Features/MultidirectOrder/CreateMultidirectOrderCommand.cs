﻿using AutoTrader.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.MultidirectOrder
{
    public class CreateMultidirectOrderCommand: BaseCommand, IRequest
    {
    }
}
