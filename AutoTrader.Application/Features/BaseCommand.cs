using AutoTrader.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features
{
    public abstract class BaseCommand
    {
        public Settings Settings { get; set; }
    }
}
