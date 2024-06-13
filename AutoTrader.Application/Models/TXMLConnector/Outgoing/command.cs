using AutoTrader.Application.Helpers;
using AutoTrader.Domain.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace AutoTrader.Application.Models.TXMLConnector.Outgoing
{
    [XmlRoot(ElementName = "command")]
    public class command
    {
        #region FABRICS
        public static command CreateConnectionCommand(string login, string password, string server = "213.247.141.133", int port = 3900)
        {
            command res = new command();

            res.id = command_id.connect;

            res.login = login;
            res.password = password;

            res.host = server;
            res.portValue = port;
            res.logsdir = MainHelper.GetWorkFolder();// +"\0";
            res.rqdelayValue = 100;
            res.session_timeoutValue = 25000;
            res.request_timeoutValue = 10000;            

            return res;
        }

        public static command CreateDisconnectCommand()
        {
            command res = new command();

            res.id = command_id.disconnect;
            
            return res;
        }

        public static command CreateGetMCPortfolioCommand(string client)
        {
            command res = new command();
            res.clientAttribute = client;
            res.id = command_id.get_mc_portfolio;

            return res;
        }

        public static command CreateChangePasswordCommand(string oldpass, string newpass)
        {
            command res = new command();

            res.id = command_id.change_pass;
            res.oldpass = oldpass;
            res.newpass = newpass;

            return res;
        }

        public static command CreateMoveOrderCommand(int transactionid, double price, int moveflag, int quantity)
        {
            command res = new command();

            res.id = command_id.moveorder;
            res.transactionidValue = transactionid;
            res.priceValue = price;
            res.moveflagValue = moveflag;
            res.quantityValue = quantity;

            return res;
        }

        public static command CreateCancelOrderCommand(int id)
        {
            command res = new command();

            res.id = command_id.cancelorder;
            res.transactionidValue = id;

            return res;
        }

        /// <summary>
        /// подписка на изменения «стакана»
        /// </summary>
        /// <param name="board"></param>
        /// <param name="seccode"></param>
        /// <returns></returns>
        public static command CreateSubscribeQuotationsCommand(boardsCode board, string seccode)
        {
            command res = new command();

            res.id = command_id.subscribe;
            res.quotations = new command_ns.quotations();
            res.quotations.security.Add(new command_ns.security()
            {
                board = board.ToString(),
                seccode = seccode
            });

            return res;
        }

        /// <summary>
        /// подписка на изменения «стакана»
        /// </summary>
        /// <param name="board"></param>
        /// <param name="seccode"></param>
        /// <returns></returns>
        public static command CreateSubscribeQuotesCommand(boardsCode board, string seccode)
        {
            command res = new command();

            res.id = command_id.subscribe;
            res.quotes = new command_ns.quotes();            
            res.quotes.security.Add(new command_ns.security()
            {
                board = board.ToString(),
                seccode = seccode
            });            

            return res;
        }

        public static command CreateGetHistoryDataCommand(boardsCode board, string seccode, SecurityPeriods periodId = SecurityPeriods.M1, int count = 1, bool reset=true)
        {
            command res = new command();

            res.id = command_id.gethistorydata;
            res.security = new command_ns.security();
            res.security.board = board.ToString();
            res.security.seccode = seccode;
            res.periodValue = (int)periodId;
            res.countValue = count;
            res.resetValue = reset;

            return res;
        }

        public static command CreateNewCondOrderCommand()
        {
            command res = new command();

            res.id = command_id.newcondorder;
            res.security = new command_ns.security();
            res.bymarketValue = AutoTrader.Application.Models.TXMLConnector.Outgoing.bymarket.yes;
            res.validafter = "0";
            res.validbeforeValue = AutoTrader.Application.Models.TXMLConnector.Outgoing.validbefore.till_canceled;

            return res;
        }

        public static command CreateNewOrder(string clientId, boardsCode board, string seccode, buysell buysell, bymarket bymarket, double price, int volume)
        {
            command res = new command();
            res.security = new command_ns.security();

            res.id = command_id.neworder;
            res.security.board = board.ToString();
            res.security.seccode = seccode;
            res.priceValue = price;
            res.quantityValue = volume;
            res.buysellValue = buysell;
            res.client = clientId;

            if (bymarket == bymarket.yes)
            {
                res.bymarket = "yes";
            }
            return res;
        }

        public static command CreateGetSecurities()
        {
            command res = new command();

            res.id = command_id.get_securities;

            return res;
        }


        #endregion

        #region CONSTRUCTORS
        public command()
        {

        }
        #endregion


        [XmlAttribute]
        public command_id id { get; set; }

        [XmlAttribute]
        public string oldpass { get; set; }

        [XmlAttribute]
        public string newpass { get; set; }

        [XmlElement(IsNullable=false)]
        public string login { get; set; }

        [XmlElement(IsNullable = false)]
        public string password { get; set; }

        [XmlElement(IsNullable = false)]
        public string host { get; set; }

        [XmlElement(IsNullable = false)]
        public string port { get; set; }
        [XmlIgnore]
        public int? portValue 
        {
            get
            {
                int res = 0;

                if (port != null) res = int.Parse(port);

                return res;
            }
            set
            {
                port = value.ToString();
            }
        }

        [XmlElement(IsNullable = false)]
        public string autopos { get ; set; }
        [XmlIgnore]
        public bool? autoposValue
        {
            get
            {
                bool? res = null;

                if (autopos != null) res = bool.Parse(autopos);

                return res;
            }
            set
            {
                autopos = value.ToString().ToLower();
            }
        }

        

        //Параметр autopos указывает на необходимость
        //автоматического запроса клиентских позиций на срочном
        //рынке после каждой клиентской сделки. Если autopos не
        //указан, по умолчанию он принимается равным true.
        //Использование <autopos>false</autopos> при активной
        //торговле ускоряет взаимодействие с сервером.
        [XmlElement(IsNullable = false)]
        public string micex_registers { get; set; }
                    //Значение элемента micex_registers определяет набор
                    //данных, передаваемый в структурах <money_position> и
                    //<sec_position> (см. раздел "Позиции клиента").

        [XmlElement(IsNullable = false)]
        public string milliseconds { get; set; }
                    //Значение элемента milliseconds определяет формат
                    //элементов типа "Дата и время" в некоторых структурах. Если
                    //задано <milliseconds>true</milliseconds>, то следующие
                    //данные отдаются в формате “DD.MM.YYYY hh:mm:ss.mmm”:
                    // элемент time структуры alltrade
                    // элемент time структуры ticks
                    // элемент time структуры trade
                    // элементы time и withdrawtime структуры order

        [XmlElement(IsNullable = false)]
        public string utc_time { get; set; }
                    //Значение элемента utc_time определяет таймзону некоторых
                    //    элементов "Дата и время". Если задано <utc_time>true</
                    //    utc_time>, то следующие элементы передаются в UTC:
                    //     элемент time структуры alltrade
                    //     элемент tradetime структуры tick
                    //     элемент time структуры trade
                    //     элемент time структуры quotation
                    //     элемент date структуры candle
                    //     элементы time и withdrawtime структуры order
                    //     элемент conditionvalue (если condition="Time" и в нем
                    //    задано дата-время)
                    //     элементы accepttime, validafter и validbefore структуры
                    //    order
                    //     элементы validbefore, withdrawtime и accepttime
                    //    структуры stoporder
                    //     элемент date структуры message
                    //     элемент time_stamp структуры news_header
                    //    Значение utc_time НЕ влияет на следующие структуры
                    //     элементы mat_date, coupon_date структуры sec_info
                    //     expdate структуры order
                    //    Если задано <utc_time>true</ utc_time>, то следующие
                    //    элементы необходимо указывать в UTC:
                    //     параметры validafter, validbefore и cond_value команды
                    //    newcondorder (если они заданы и не заданы
                    //    спец.значения validafter=0 или validbefore=0)
                    //     параметр validfor команды newstoporder (если не
                    //    задано спец.значения validfor=0)
                    //    Независимо от значения <utc_time>:
                    //     в структуру securities добавляется элемент sec_tz,
                    //    содержащий имя таймзоны инструмента (типа "Russian
                    //    Standard Time", "USA=Eastern Standard Time")
                    //     в структуру server_status добавляется элемент
                    //    server_tz, содержащий имя таймзоны сервера


        [XmlElement(IsNullable = false)]
        public proxy proxy { get; set; }

        [XmlElement(IsNullable = false)]
        public string rqdelay { get; set; }
        [XmlIgnore]
        public int? rqdelayValue
        {                    
            get
            {
                int? res = null;

                if (rqdelay != null) res = int.Parse(rqdelay);

                return res;
            }
            set
            {
                rqdelay = value.ToString().ToLower();
            }
        }
                    //Rqdelay задает частоту обращений Коннектора к серверу
                    //Transaq в миллисекундах. Минимальные значения:
                    //Торговый сервер Transaq 100 мс
                    //Торговый сервер HFT Transaq 10 мс


        [XmlElement(IsNullable = false)]
        public string session_timeout { get; set; }
        [XmlIgnore]
        public int? session_timeoutValue
        {
            get
            {
                int? res = null;

                if (session_timeout != null) res = int.Parse(session_timeout);

                return res;
            }
            set
            {
                session_timeout = value.ToString().ToLower();
            }
        }

                    //Session_timeout – интервал времени, в течении которого
                    //коннектор в случае ошибок связи будет выполнять попытки
                    //переподключения к серверу без повторной закачки
                    //информации о доступных периодах исторической ценовой
                    //информации – свечах/барах (candlekinds), финансовых
                    //инструментах (securities) и рынках (markets). Если данный
                    //параметр не будет указан, используется значение по
                    //умолчанию равное 120 секундам.

        [XmlElement(IsNullable = false)]
        public string request_timeout { get; set; }
        [XmlIgnore]
        public int? request_timeoutValue
        {
            get
            {
                int? res = null;

                if (request_timeout != null) res = int.Parse(request_timeout);

                return res;
            }
            set
            {
                request_timeout = value.ToString().ToLower();
            }
        }
                    //Request_timeout - таймаут на выполнение запроса. Если
                    //данный параметр не будет указан, используется значение по
                    //умолчанию равное 20 секундам. Значение параметра
                    //session_timeout должно быть больше значения параметра
                    //request_timeout.


        [XmlElement(IsNullable = false)]
        public string logsdir { get; set; }

        #region ORDER
        // todo проверить будут ли проблемы, если этот параметр заполнять в конструкторе
        [XmlElement(IsNullable=false)]
        public command_ns.security security { get; set; }


        //0: не менять количество;
        //1: изменить количество;
        //2: при несовпадении количества с текущим – снять заявку.

        [XmlElement(IsNullable = false)]
        public string moveflag { get; set; }
        [XmlIgnore]
        public int? moveflagValue
        {
            get
            {
                int? res = null;

                if (moveflag != null) res = int.Parse(moveflag);

                return res;
            }
            set
            {
                moveflag = value.ToString().ToLower();
            }
        } 

        [XmlElement(IsNullable = false)]
        public string price { get; set; }
        [XmlIgnore]
        public double? priceValue
        {
            get
            {
                double? res = null;

                if (price != null) res = double.Parse(price);

                return res;
            }
            set
            {
                price = value.ToString().ToLower();
            }
        }

        [XmlElement(IsNullable = false)]
        public string quantity { get; set; }
        [XmlIgnore]
        public int? quantityValue
        {
            get
            {
                int? res = null;

                if (quantity != null) res = int.Parse(quantity);

                return res;
            }
            set
            {
                quantity = value.ToString().ToLower();
            }
        }


        [XmlElement(IsNullable = false)]
        public string buysell { get; set; }
        [XmlIgnore]
        public buysell? buysellValue
        {
            get
            {
                buysell? res = null;

                if (buysell != null) res = (buysell)Enum.Parse(typeof(buysell), buysell,true); ;

                return res;
            }
            set
            {
                buysell = value.ToString().ToUpper();
            }
        }

        [XmlAttribute(attributeName:"client")]
        public string clientAttribute { get; set; }

        [XmlElement(IsNullable = false)]
        public string client { get; set; }

        [XmlElement(IsNullable = false)]
        public string unfilled { get; set; }
        [XmlIgnore]
        public unfilled? unfilledValue
        {
            get
            {
                unfilled? res = null;

                if (unfilled != null) res = (unfilled)Enum.Parse(typeof(unfilled), unfilled, true); ;

                return res;
            }
            set
            {
                unfilled = value.ToString().ToLower();
            }
        }


        //Usecredit, nosplit и bymarket должны быть заданы пустым тегом, либо отсутствовать вообще.
        [XmlElement(IsNullable = false)]
        public string bymarket { get; set; }  //При наличии тега bymarket, тег price игнорируется и может отсутствовать.
        [XmlIgnore]
        public bymarket? bymarketValue
        {
            get
            {
                bymarket? res = null;

                if (bymarket != null) res = (bymarket)Enum.Parse(typeof(bymarket), bymarket, true); ;

                return res;
            }
            set
            {
                bymarket = value.ToString().ToLower();
            }
        }

        [XmlElement(IsNullable = false)]
        public string usecredit { get; set; }

        [XmlElement(IsNullable = false)]
        public string nosplit { get; set; }
        

        [XmlElement(IsNullable = false)]
        public string brokerref { get; set; }

        [XmlElement(IsNullable = false)]
        public string hidden { get; set; }      //скрытое количество в лотах. является необязательным.

        [XmlElement(IsNullable = false)]
        public string expdate { get; set; }     //дата экспирации (только для ФОРТС)

        #endregion


        #region STOPORDER
        [XmlElement(IsNullable = false)]
        public string linkedorderno { get; set; }
        [XmlIgnore]
        public Int64? linkedordernoValue
        {
            get
            {
                Int64? res = null;

                if (linkedorderno != null) res = Int64.Parse(linkedorderno); ;

                return res;
            }
            set
            {
                linkedorderno = value.ToString().ToLower();
            }
        }

        // только для Stop заявок
        //0 – до конца торговой сессии
        //till_canceled – до отмены
        //ДД.ММ.ГГГГ ЧЧ:ММ:СС – до указанной даты и времени
        [XmlElement(IsNullable = false)]
        public string validfor { get; set; }                    
        [XmlIgnore]
        public validbefore? validforValue
        {
            get
            {
                validbefore? res = null;

                if (validfor != null) res = (validbefore)Enum.Parse(typeof(validbefore), validfor, true); ;

                return res;
            }
            set
            {
                validfor = value.ToString().ToLower();
            }
        }

        [XmlElement(IsNullable = false)]
        public stoploss stoploss { get; set; }

        [XmlElement(IsNullable = false)]
        public takeprofit takeprofit { get; set; }

        #endregion

        #region CONDORDER
        [XmlElement(IsNullable = false)]
        public string cond_type { get; set; }
        [XmlIgnore]
        public cond_type? cond_typeValue
        {
            get
            {
                cond_type? res = null;

                if (cond_type != null) res = (cond_type)Enum.Parse(typeof(cond_type), cond_type, true); ;

                return res;
            }
            set
            {
                cond_type = value.ToString();
            }
        }

        [XmlElement(IsNullable = false)]
        public string cond_value { get; set; }
        [XmlIgnore]
        public double? cond_valueValue
        {
            get
            {
                double? res = null;

                if (cond_value != null) res = double.Parse(cond_value); ;

                return res;
            }
            set
            {
                cond_value = value.ToString().ToLower();
            }
        }

        // validafter и validbefore задаются в форме даты, описанном выше. 
        // Для validafter можно передать значение "0", если заявка начинает действовать немедленно. 
        // Для validbefore значение "0" означает, что заявка будет действительна до конца сессии. 
        // Также validbefore может принимать текстовое значение "till_canceled", которое означает, что заявка  должна быть действительна до ее отмены
        [XmlElement(IsNullable = false)]
        public string validafter { get; set; }      //с какого момента времени действительна

        [XmlElement(IsNullable = false)]
        public string validbefore { get; set; }     //до какого времени действует заявка
        [XmlIgnore]
        public validbefore? validbeforeValue
        {
            get
            {
                validbefore? res = null;

                if (validbefore != null) res = (validbefore)Enum.Parse(typeof(validbefore), validbefore, true); ;

                return res;
            }
            set
            {
                validbefore = value.ToString().ToLower();
            }
        }

        //0 – до конца торговой сессии
        //till_canceled – до отмены
        //ДД.ММ.ГГГГ ЧЧ:ММ:СС – до указанной даты и времени
        [XmlElement(IsNullable = false)]
        public string transactionid { get; set; }
        [XmlIgnore]
        public int? transactionidValue 
        {
            get
            {
                int res = 0;

                if (transactionid != null) int.Parse(transactionid);

                return res;
            }
            set
            {
                transactionid = value.ToString();
            }
        }

        #endregion

        #region GET HISTORY DATA
        [XmlElement(IsNullable = false)]
        public string period { get; set; }

        /// <summary>
        /// Возможные значения для period присылаются при установке соединения с 
        ///         сервером в полях candlekinds(поле id).
        /// В period необходимо указать нужное значение поля<id> из структуры
        /// <candlekinds>.
        /// </summary>
        [XmlIgnore]
        public int? periodValue
        {
            get
            {
                int? res = null;

                if (period != null) res = int.Parse(period); ;

                return res;
            }
            set
            {
                period = value.ToString().ToLower();
            }
        }

        [XmlElement(IsNullable = false)]
        public string count { get; set; }
        [XmlIgnore]
        public int? countValue
        {
            get
            {
                int? res = null;

                if (count != null) res = int.Parse(count); ;

                return res;
            }
            set
            {
                count = value.ToString().ToLower();
            }
        }

        [XmlElement(IsNullable = false)]
        public string reset { get; set; }
        [XmlIgnore]
        public bool? resetValue
        {
            get
            {
                bool? res = null;

                if (reset != null) res = bool.Parse(reset); ;

                return res;
            }
            set
            {
                reset = value.ToString().ToLower();
            }
        }
        #endregion

        #region SUBSCRIBE

        [XmlElement(IsNullable = false)]
        public alltrades alltrades { get; set; }

        [XmlElement(IsNullable = false)]
        public command_ns.quotations quotations { get; set; }

        /// <summary>
        /// подписка на изменения «стакана»
        /// </summary>
        [XmlElement(IsNullable = false)]
        public command_ns.quotes quotes { get; set; }
        #endregion
    }
}
