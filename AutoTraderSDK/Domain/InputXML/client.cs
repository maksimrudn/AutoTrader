using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Domain.InputXML
{
    public class client
    {
        public string id { get; set; }

        public string remove { get; set; }

        public string type { get; set; }

        public string currency { get; set; }

        public int market { get; set; }

        public string union { get; set; }

        public string forts_acc { get; set; }
    }
}


                //Данные сообщения для каждого из клиентских счетов передаются сразу
                //после коннекта. Кроме того, это сообщение передается в случае
                //добавления/удаления администратором TRANSAQ доступа к клиентскому
                //счету во время текущей сессии.
                //Возможные типы клиента: spot (кассовый), leverage (плечевой), margin_level
                //(маржинальный).
                //Параметр remove говорит о том, добавился ли клиент или удалился. В случае
                //удаления клиента никаких свойств клиента передано не будет.
                //Валюта фондового портфеля currency может принимать следующие
                //значения: NA (если клиент не имеет фондового портфеля), RUB, EUR, USD.
                //Union – код Единого Портфеля, в который включен данный клиент. Если
                //клиент не включён в юнион, то элемент <union> в структуре client
                //отсутствует.
                //market – идентификатор рынка, на котором разрешено работать данному
                //клиенту (значение id из структуры markets).
                //forts_acc - счет FORTS клиента. Если клиент не имеет счета FORTS, то
                //элемент не передается.