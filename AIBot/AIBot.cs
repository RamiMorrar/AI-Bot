using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace AI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SpeechSynthesizer s = new SpeechSynthesizer();
        public bool search = false;
        Choices Index = new Choices();
        Choices list = new Choices();
        string temp;
        string condition;
        public MainWindow()
        {

            Index.Add(new String[] { "hello, How are you", "what Time is it", "what do you like to do\n" +
                 "restart", "update", "what is today", "what can you do", "open google chrome", "wake", "sleep", "open steam", "tell me a joke ",
           "whats the weather like", "whats the temperature", "minimize window", "maximize window", " open youtube", "mute","play","pause",  "hello al" });

            Grammar g = new Grammar(new GrammarBuilder(Index));

            list.Add(File.ReadAllLines(@"C:\Users\exewi\Documents\Search Commands.txt"));


            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(g);
                rec.SpeechRecognized += rec_SpeachRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { return; }

            s.SelectVoiceByHints(VoiceGender.Neutral);
            
            

            InitializeComponent();


            
        }

        public String GetWeather(String input)
        {
            String query = String.Format("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='san Leandro, ca')&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
            XmlDocument wData = new XmlDocument();
            wData.Load(query);
            try
            {
                wData.Load(query);
            }
            catch
            {
                MessageBox.Show("No internet Connection");
                return "No internet";
            }
            XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
            manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");
            XmlNodeList nodes = wData.SelectNodes("query/results/channel");
            try
            {
                int rawtemp = int.Parse(channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value);
                temp = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
                condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;

                if (input == "temp")
                {
                    return temp;
                }

                if (input == "cond")
                {
                    return condition;
                }
            }
            catch
            {
                return "Error Recieving data";
            }
            return "error";
        }
        
        public void say (string h)
        {
            s.Speak(h);
            
        }
        private void rec_SpeachRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string r = e.Result.Text;
            //What to Say
            
            if (r=="hello al")
            {
                say("Hi there.");
            }
                if (r=="maximize window")
                {
                    this.WindowState = WindowState.Maximized;
                }
                if(r=="minimize window")
                {
                    this.WindowState = WindowState.Minimized;
                }

                if (r == "hello")
                {
                    say("Hi");
                    say(greetings_action());
                }
                if (r == "what is today")
                {
                    say(DateTime.Now.ToString("M/d/yyyy"));
                }
                if (r == "open steam")
                {
                    Process.Start(@"C:\Program Files (x86)\Steam\Steam.exe");
                }
                if (r == "what can you do")
                {
                    say("I can open up applications, do google search and tell the weather.");
                }
                
                if (r=="whats the weather like")
                {
                    say("todays weather conditions are " + GetWeather("cond")+".");
                }

                if (r == "mute")
                {

                }
                if (r == "pause")
                {

                }
                if (r == "play")
                {
                     
                }
                if (r == "search for")
                {
                search = true;
                }
               

                if  (search)
                {
                    Process.Start("https://www.google.com/#q=" + r);
                    search = false;
                }
                
                if (r == "whats the temperature")
                {
                    say("the temperature today is " + GetWeather("temp") + "degrees.");
                }
                if (r == "open discord")
                {
                    Process.Start(@"");
                }
                if (r == "")
                if (r == "open gooogle chrome")
                {
                    Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
                }
                if (r == "open twitter")
                {
                    Process.Start("http://Twitter.com");
                    say("now opening Twitter.");
                }
                if (r == "what time is it?")
                {
                    say("h:mm tt");
                }
                if (r == "hello, how are you")
                {
                    say("Fantastic! You?");
                }
                if (r == "tell me a joke")
                {
                    say("Why tell a joke when the joke is right infront of me?");
                }
                if ( r == "open youtube")
                {
                    Process.Start("https://www.youtube.com/");
                    say("Now opening youtube. You can pause, play or mute videos by saying pause, play, or mute.");
                }
                
            
        }

        String[] greetings = new String[4] { "hi", "yo, what's up", "hi there!", "hey, how's it going dude" }; 

        public String greetings_action()
        {
            Random r = new Random();
            return greetings[r.Next(4)];
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("http://twitter.com/RamiMorrar97");
        }
    }
}
