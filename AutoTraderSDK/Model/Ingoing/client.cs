using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTraderSDK.Model.Ingoing
{
    //Данные сообщения для каждого из клиентских счетов передаются сразу
    //после коннекта. Кроме того, это сообщение передается в случае
    //добавления/удаления администратором TRANSAQ доступа к клиентскому
    //счету во время текущей сессии.
    public class client
    {
        [XmlAttribute]
        public string id { get; set; }


        //Параметр remove говорит о том, добавился ли клиент или удалился. В случае
        //удаления клиента никаких свойств клиента передано не будет.
        [XmlAttribute]
        public bool remove { get; set; }


        //Возможные типы клиента: spot (кассовый), leverage (плечевой), margin_level
        //(маржинальный).
        public string type { get; set; }

        //Валюта фондового портфеля currency может принимать следующие
        //значения: NA (если клиент не имеет фондового портфеля), RUB, EUR, USD.
        public string currency { get; set; }

        //market – идентификатор рынка, на котором разрешено работать данному
        //клиенту (значение id из структуры markets).
        public int market { get; set; }

        //Union – код Единого Портфеля, в который включен данный клиент. Если
        //клиент не включён в юнион, то элемент <union> в структуре client
        //отсутствует.
        public string union { get; set; }

        //forts_acc - счет FORTS клиента. Если клиент не имеет счета FORTS, то
        //элемент не передается.
        public string forts_acc { get; set; }
    }
}


                
                
                
                
                
                
                