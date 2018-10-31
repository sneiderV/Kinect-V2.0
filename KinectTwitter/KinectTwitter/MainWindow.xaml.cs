﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using Tweetinvi;


namespace KinectTwitter
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /*Twitter*/
        const string CONSUMER_KEY = "a2mVCms5rkDNxgwUEFdy4oOfE";
        const string CONSUMER_SECRET = "ahwcSlg5ENtjmIP820NptGP1QItCJJHYY0SxcvXOIkHEsZmBfd";
        const string ACCESS_TOKEN = "889371522648993792-33Ol8hQUocGSUV8hYEPKv4GHULn15Dw";
        const string ACCESS_TOKEN_SECRET = "WQfH4NZYdyEK9D11R0u0ibKnfQ9m5NPXM1IobkLPl0nJh";

        KinectSensor miKinect;

        Stack pilaTweets;
        ArrayList listaTweets;

        public MainWindow()
        {
            InitializeComponent();

            miKinect = KinectSensor.GetDefault();

            //especifico mi region 
            KinectRegion.SetKinectRegion(this, mikinectRegion);
            App app = ((App)Application.Current);

            //asigno mi kinect a mi region
            mikinectRegion.KinectSensor = miKinect;

            //autenticación con twitter
            Auth.SetUserCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);

            //Ejecutando el streaming de tweets en background
            BackgroundWorker streamBack = new BackgroundWorker();
            streamBack.DoWork += streamingTweets;
            //streamBack.RunWorkerCompleted += mostrarTweets();
            streamBack.RunWorkerAsync();

            pilaTweets = new Stack();
            listaTweets = new ArrayList();
                        
        }

        /// <summary>
        /// Se ejecuta cuando se carga la aplicación 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pintarTweets(listaTweets);
        }

        

        public void pintarTweets( ArrayList tweets)
        {
            tweets.Reverse();

            if (tweets.Count >= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(tweets[i]);
                    var t = new TweetK("ISIS", "@ISIS", "50 años", "1 Nov");
                    miScrollContent.Children.Add(t);
                }
            }
            else
            {
                for (int i = 0; i < 5 ; i++)
                {
                    var t = new TweetK("UniandesISIS", "@UniandesISIS", "Celebrando los 50 años", "1 Nov");
                    miScrollContent.Children.Add(t);
                }
            }
            
        }

        /// <summary>
        /// Hace el streaming de los tweets con el hashtag #KinectUniandes
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        private void streamingTweets(object o, DoWorkEventArgs args)
        {
            ArrayList twetts = new ArrayList();
            //strean
            var stream = Tweetinvi.Stream.CreateFilteredStream();
            //palabra a seguir
            String hashtag = "#KinectUniandes";
            stream.AddTrack(hashtag);
            //manejo de tweets
            Console.WriteLine("::-----------> Escuchando tweets para "+hashtag);
            stream.MatchingTweetReceived += (sender, arg) =>
            {
                Console.WriteLine(arg.Tweet.Text);
                String nom = arg.Tweet.CreatedAt.DayOfWeek.ToString();
                String usId = arg.Tweet.CreatedBy.Name;
                String cont = "@" + arg.Tweet.CreatedBy.ScreenName;
                String dat = arg.Tweet.FullText;
               // var t = new TweetK(nom,usId,cont,dat);

                pilaTweets.Push(arg.Tweet);

                twetts.Add(arg.Tweet);
                listaTweets = twetts;
                listaTweets.Add(arg.Tweet);

            };
            stream.StartStreamMatchingAllConditions();
        }

        
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            pintarTweets(listaTweets);
        }

       
    }
}
/*
fecha: arg.Tweet.CreatedAt.DayOfWeek (int)
nombre: arg.Tweet.CreatedBy.Name (String)
userId: arg.Tweet.CreatedBy.ScreenName (String)+@
content: arg.Tweet.FullText (String)
 */
