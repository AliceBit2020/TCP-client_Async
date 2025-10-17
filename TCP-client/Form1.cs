using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace TCP_client
{
    public partial class Form1 : Form
    {
        Socket sock;
        public Form1()
        {
            InitializeComponent();
        }

        private async void ConnectAsync()
        {
            await Task.Run(() =>
            {
                // соединяемся с удаленным устройством
                try
                {

                    IPAddress ipAddr = IPAddress.Parse(ip_address.Text);
                    // устанавливаем удаленную конечную точку для сокета
                    // уникальный адрес для обслуживания TCP/IP определяется комбинацией IP-адреса хоста с номером порта обслуживания
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr /* IP-адрес */, 49200 /* порт */);

                    // создаем потоковый сокет
                    sock = new Socket(AddressFamily.InterNetwork /*схема адресации*/, SocketType.Stream /*тип сокета*/, ProtocolType.Tcp /*протокол*/);
                    /* Значение InterNetwork указывает на то, что при подключении объекта Socket к конечной точке предполагается использование IPv4-адреса.
                      SocketType.Stream поддерживает надежные двусторонние байтовые потоки в режиме с установлением подключения, без дублирования данных и 
                      без сохранения границ данных. Объект Socket этого типа взаимодействует с одним узлом и требует предварительного установления подключения 
                      к удаленному узлу перед началом обмена данными. Тип Stream использует протокол Tcp и схему адресации AddressFamily.
                    */

                    // соединяем сокет с удаленной конечной точкой
                    sock.Connect(ipEndPoint);
                    byte[] msg = Encoding.Default.GetBytes(Dns.GetHostName() /* имя узла локального компьютера */);// конвертируем строку, содержащую имя хоста, в маock.Send(msg); ссив байтов
                    int bytesSent = sock.Send(msg);// отправляем серверу сообщение через сокет
                    MessageBox.Show("Клиент " + Dns.GetHostName() + " установил соединение с " + sock.RemoteEndPoint.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Клиент: " + ex.Message);
                }
            }); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectAsync();            
        }

        private async Task ExchangeAsync()
        {
           await Task.Run( () =>
            {
                try
                {
                    ////
                    string theMessage = textBox1.Text; // получим текст сообщения, введенный в текстовое поле
                    byte[] msg = Encoding.Default.GetBytes(theMessage); // конвертируем строку, содержащую сообщение, в массив байтов
                    Thread.Sleep(5000);
                    int bytesSent = sock.Send(msg); // отправляем серверу сообщение через сокет
                   
                    if (theMessage.IndexOf("<end>") > -1) // если клиент отправил эту команду, то принимаем сообщение от сервера
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = sock.Receive(bytes); // принимаем данные, переданные сервером. Если данных нет, поток блокируется
                        MessageBox.Show("Сервер (" + sock.RemoteEndPoint.ToString() + ") ответил: " + Encoding.Default.GetString(bytes, 0, bytesRec) /*конвертируем массив байтов в строку*/);
                        sock.Shutdown(SocketShutdown.Both); // Блокируем передачу и получение данных для объекта Socket.
                        sock.Close(); // закрываем сокет
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Клиент: " + ex.Message);
                }
            });

         

         
        }

        private async Task<int> ExchangeAsyncInt()
        {
            await Task.Run(() =>
            {
                try
                {
                    ////
                    string theMessage = textBox1.Text; // получим текст сообщения, введенный в текстовое поле
                    byte[] msg = Encoding.Default.GetBytes(theMessage); // конвертируем строку, содержащую сообщение, в массив байтов
                    Thread.Sleep(5000);
                    int bytesSent = sock.Send(msg); // отправляем серверу сообщение через сокет

                    if (theMessage.IndexOf("<end>") > -1) // если клиент отправил эту команду, то принимаем сообщение от сервера
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = sock.Receive(bytes); // принимаем данные, переданные сервером. Если данных нет, поток блокируется
                        MessageBox.Show("Сервер (" + sock.RemoteEndPoint.ToString() + ") ответил: " + Encoding.Default.GetString(bytes, 0, bytesRec) /*конвертируем массив байтов в строку*/);
                        sock.Shutdown(SocketShutdown.Both); // Блокируем передачу и получение данных для объекта Socket.
                        sock.Close(); // закрываем сокет
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Клиент: " + ex.Message);
                }
            });
            return 1;




        }
        private async Task<int> ExchangeAsyncTaskInt()
        {
            int th_id = Thread.CurrentThread.ManagedThreadId;
            MessageBox.Show(th_id.ToString());
            var result= await Task.Run<int>(() =>
            {
                try
                {
                    ////
                    string theMessage = textBox1.Text; // получим текст сообщения, введенный в текстовое поле
                    byte[] msg = Encoding.Default.GetBytes(theMessage); // конвертируем строку, содержащую сообщение, в массив байтов
                    Thread.Sleep(5000);
                    int bytesSent = sock.Send(msg); // отправляем серверу сообщение через сокет

                    if (theMessage.IndexOf("<end>") > -1) // если клиент отправил эту команду, то принимаем сообщение от сервера
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = sock.Receive(bytes); // принимаем данные, переданные сервером. Если данных нет, поток блокируется
                        MessageBox.Show("Сервер (" + sock.RemoteEndPoint.ToString() + ") ответил: " + Encoding.Default.GetString(bytes, 0, bytesRec) /*конвертируем массив байтов в строку*/);
                        sock.Shutdown(SocketShutdown.Both); // Блокируем передачу и получение данных для объекта Socket.
                        sock.Close(); // закрываем сокет
                    }
                    return bytesSent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Клиент: " + ex.Message);
                    return 0;
                }
            });
            th_id = Thread.CurrentThread.ManagedThreadId;
            MessageBox.Show(th_id.ToString());
            return result;




        }

        //private  void button3_Click(object sender, EventArgs e)
        //{
        //    ExchangeAsync();
            
        //}
        //private async void button4_Click(object sender, EventArgs e)
        //{
        //  int res=await ExchangeAsyncInt();
        //    MessageBox.Show(res.ToString());
        //}
        private async void button3_Click(object sender, EventArgs e)
        {

            int th_id = Thread.CurrentThread.ManagedThreadId;
            MessageBox.Show("Click before await"+th_id.ToString());

            int res = await ExchangeAsyncTaskInt();
            MessageBox.Show(res.ToString());

          th_id = Thread.CurrentThread.ManagedThreadId;
            MessageBox.Show("Click after await"+th_id.ToString());


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                sock.Shutdown(SocketShutdown.Both); // Блокируем передачу и получение данных для объекта Socket.
                sock.Close(); // закрываем сокет
            }
            catch (Exception ex)
            {
                MessageBox.Show("Клиент: " + ex.Message);
            }
        }
    }
}
