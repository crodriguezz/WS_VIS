﻿using IBM.WMQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQLibrary
{
    public class Metodos
    {

        /// <summary>
        /// Variables
        ///</summary>
        private MQQueueManager queueManager;
        private MQQueue queue;
        private Hashtable properties;
        private MQMessage message;
        /// <summary>
        /// Permite enviar un mensaje directo a la Cola del MQ
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="trama"></param>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public bool PutMessages(string trama, string messageID, out string mensaje)
        {
            bool response = false;
            try
            {
                // mq properties
                properties = new Hashtable();
                properties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED);
                properties.Add(MQC.HOST_NAME_PROPERTY, Properties.Resources.hostName);
                properties.Add(MQC.PORT_PROPERTY, Properties.Resources.port);
                properties.Add(MQC.CHANNEL_PROPERTY, Properties.Resources.channelName);

                // create connection
                queueManager = new MQQueueManager(Properties.Resources.queueManagerName, properties);
                // accessing queue
                queue = queueManager.AccessQueue(Properties.Resources.queueName_Put, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);

                // creating a message object
                message = new MQMessage();
                message.Format = MQC.MQFMT_STRING;
                message.CharacterSet = 437;
                message.Encoding = 546;
                message.WriteString(trama);


                message.MessageId = Encoding.ASCII.GetBytes(messageID);

                queue.Put(message);
                mensaje = "";
                response = true;


                // closing queue
                queue.Close();
                // disconnecting queue manager
                queueManager.Disconnect();
            }

            catch (MQException mqe)
            {
                //Console.WriteLine("");
                // Console.WriteLine("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message);
                //Console.WriteLine(mqe.StackTrace);
                mensaje = "No fue posible realizar la Consulta, intente nuevamente";
                //mensaje = string.Format("MQException caught: {0} - {1} - {2} - {3}", mqe.ReasonCode,mqe.Message,mqe.Reason, mqe.ToString());
                response = false;
            }
            return response;
        }

        /// <summary>
        /// Permite obtener un mensaje directo de la Cola
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        public string GetMessages()
        {
            string response = string.Empty;
            try
            {
                // mq properties
                properties = new Hashtable
                {
                    { MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED },
                    { MQC.HOST_NAME_PROPERTY, Properties.Resources.hostName },
                    { MQC.PORT_PROPERTY, Properties.Resources.port },
                    { MQC.CHANNEL_PROPERTY, Properties.Resources.channelName }
                };

                // create connection
                queueManager = new MQQueueManager(Properties.Resources.queueManagerName, properties);
                // accessing queue
                queue = queueManager.AccessQueue(Properties.Resources.queueName_Get, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);

                // getting messages continuously
                //for (int i = 1; i <=  Int32.Parse(Properties.Resources.numberOfMsgs); i++)
                //{
                // creating a message object
                message = new MQMessage();


                try
                {
                    queue.Get(message);
                    response = message.ReadString(message.MessageLength);
                    //Console.WriteLine("Message " + i + " got = " + message.ReadString(message.MessageLength));
                    message.ClearMessage();
                }
                catch (MQException mqe)
                {
                    if (mqe.ReasonCode == 2033)
                    {
                        Console.WriteLine("No message available");
                    }
                    else
                    {
                        Console.WriteLine("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message);
                    }

                    response = "-1";
                }
                //}

                // closing queue
                queue.Close();
                // disconnecting queue manager
                queueManager.Disconnect();
            }

            catch (MQException mqe)
            {
                Console.WriteLine("");
                Console.WriteLine("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message);
                Console.WriteLine(mqe.StackTrace);
                response = "-2";
            }

            return response;
        }
    }
}
