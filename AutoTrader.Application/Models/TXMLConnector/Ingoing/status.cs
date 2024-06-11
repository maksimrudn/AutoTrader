﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTrader.Application.Models.TXMLConnector.Ingoing
{
    public enum status
    {
        active,
        cancelled,
        denied,
        disabled,
        expired,
        failed,
        forwarding,
        inactive,
        matched,
        refused,
        rejected,
        removed,
        wait,
        watching
    }

    //active          Активная
    //cancelled       Снята трейдером (заявка уже попала на рынок и была отменена)
    //denied          Отклонена Брокером
    //disabled        Прекращена трейдером (условная заявка, которую сняли до наступления условия)
    //expired         Время действия истекло
    //failed          Не удалось выставить на биржу
    //forwarding      Выставляется на биржу
    //inactive        Статус не известен из-за проблем со связью с биржей
    //matched         Исполнена
    //refused         Отклонена контрагентом
    //rejected        Отклонена биржей
    //removed         Аннулирована биржей
    //wait            Не наступило время активации
    //watching        Ожидает наступления условия
}
