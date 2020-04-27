using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Domain.OutputXML
{
    public enum command_id
    {
        connect,
        disconnect,
        server_status,
        get_securities,
        subscribe,
        unsubscribe,
        gethistorydata,
        neworder,
        newcondorder,
        newstoporder,
        cancelorder,
        cancelstoporder,
        get_forts_positions,
        get_client_limits,
        get_markets,
        get_servtime_difference,
        change_pass,
        subscribe_ticks,
        get_connector_version,
        get_securities_info,
        moveorder,
        get_portfolio,
        get_max_buy_sell_tplus,
        get_united_portfolio
    }
}
