using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_ServiceBus_POC_02
{
    internal class Program
    {
        static async Task Main(string[] args)
        {           
            string connectionString = "";
            string topicName = "topic_poc_01";
            string subscription = "subscription_poc_02";
            
            ServiceBusClient client = new ServiceBusClient(connectionString);

            try
            {
                // Enviar un mensaje
                await SendMessageAsync(client, topicName, "POC Ejemplo Service Bus");

        
                // Recibir y eliminar un mensaje
                await ReceiveAndDeleteMessageAsync(client, topicName, subscription);
            }
            finally
            {
                // Cerrar el cliente
                 await client.DisposeAsync();
            }
     
        }


        static async Task SendMessageAsync(ServiceBusClient client, string topicName, string messageBody)
        {
            // Crear un emisor para el tópico
            ServiceBusSender sender = client.CreateSender(topicName);
 
            // Crear un mensaje con la propiedad personalizada
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            message.ApplicationProperties.Add("tipo", "orden");

            try
            {
                // Enviar el mensaje
                await sender.SendMessageAsync(message);

               
                Console.WriteLine("Mensaje enviado correctamente.");
            }
            finally
            {
                // Cerrar el emisor
                await sender.DisposeAsync();
            }
        }

        static async Task ReceiveAndDeleteMessageAsync(ServiceBusClient client, string topicName, string subscription)
        {
            // Crear un receptor para el tópico
            ServiceBusReceiver receiver = client.CreateReceiver(topicName, subscription);

            try
            {
                // Recibir un mensaje
                ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

                if (receivedMessage != null)
                {
                    Console.WriteLine("Mensaje recibido: " + receivedMessage.Body.ToString());
                    await receiver.CompleteMessageAsync(receivedMessage);
                    Console.WriteLine("Mensaje eliminado correctamente.");
                }
                else
                {
                    Console.WriteLine("No se encontraron mensajes.");
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Cerrar el receptor
                await receiver.DisposeAsync();
            }
        }


    }
}
