using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Homework10_5
{
    class TelegramMessageClient
    {
        private MainWindow w;
        public bool saveLog;

        private TelegramBotClient bot;
        // Коллекция сообщений пользователя
        public ObservableCollection<MessageLog> BotMessageLog { get; set; }
        public TelegramMessageClient(MainWindow W, string PathToken = @"token.txt")
        {
            this.BotMessageLog = new ObservableCollection<MessageLog>();
            this.w = W;

            bot = new TelegramBotClient(File.ReadAllText(PathToken));

            bot.OnMessage += MessageListener;

            bot.StartReceiving();
        }
        /// <summary>
        /// Метод, обрабатывающий запрос пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageListener(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string url;
            string cityName;
            string messageText = "";
            string response;
            if (e.Message.Text == null)
                return;
            else
                cityName = e.Message.Text;
            url = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&appid=0452bfbba226fdd6b3dc9df0e173a75f";
            try
            {// Передача http запроса
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                //WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
                WeatherResponse weatherResponse = new WeatherResponse(
                JObject.Parse(response)["name"].ToString(),
                (float)Convert.ToDouble(JObject.Parse(response)["main"]["temp"].ToString()),
                (float)Convert.ToDouble(JObject.Parse(response)["main"]["feels_like"].ToString()),
                (float)Convert.ToDouble(JObject.Parse(response)["main"]["pressure"].ToString()),
                (float)Convert.ToDouble(JObject.Parse(response)["main"]["humidity"].ToString()),
                (float)Convert.ToDouble(JObject.Parse(response)["wind"]["speed"].ToString()),
                Convert.ToInt32(JObject.Parse(response)["clouds"]["all"].ToString())
                );
                messageText = $"{weatherResponse.Name}:\n" +
                $"Temperature: {weatherResponse.Temp} °C\n" +
                $"Feels like: {weatherResponse.Feels_like} °C\n" +
                $"Pressure: {weatherResponse.Pressure} hPa\n" +
                $"Humidity: {weatherResponse.Humidity} %\n" +
                $"Wind speed: {weatherResponse.WindSpeed} m/sec\n" +
                $"Cloudiness: {weatherResponse.Clouds} %";
            }// Обработка ошибки запроса
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ProtocolError)
                    messageText = "City name is wrong!";
            }// Передача ответа пользователю
            bot.SendTextMessageAsync(e.Message.Chat.Id, messageText);
            
            
            w.Dispatcher.Invoke(() =>
            {
                MessageLog messageLog = new MessageLog(
                    DateTime.Now.ToLongTimeString(), cityName, e.Message.Chat.FirstName, e.Message.Chat.Id);
                BotMessageLog.Add(messageLog);
                if (saveLog)
                    AddLog(messageLog);
            });
        }
        private void AddLog(MessageLog messageLog)
        {
            File.AppendAllText(@"messageLog.json", JsonConvert.SerializeObject(messageLog) + "\n");
        }

        /// <summary>
        /// Метод, передающий сообщение пользователю
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Id"></param>
        public void SendMessage(string Text, string Id)
        {
            long id = Convert.ToInt64(Id);
            bot.SendTextMessageAsync(id, Text);
        }
    }
}
