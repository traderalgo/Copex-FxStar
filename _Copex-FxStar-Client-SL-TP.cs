//
// @Autor: Marcin Łukaszewski
// @Url: https://fxstar.eu
//
// Download and install Mysql Connector/.NET from:
// https://dev.mysql.com/downloads/connector/net/6.9.html
//
using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
using System.Collections.Generic;
// run browser
using System.Diagnostics;
using System.IO;
// Threading
using System.Threading;
//Add MySql Library
using MySql.Data.MySqlClient;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.Internet | AccessRights.FileSystem | AccessRights.FullAccess)]
    public class FxStarEu_Client : Robot
    {
        [Parameter(DefaultValue = 1)]
        public int TimerSeconds { get; set; }

        [Parameter(DefaultValue = 300, MinValue = 1, MaxValue = 1000)]
        public int DelayMillisecond { get; set; }

        [Parameter(DefaultValue = 9975139)]
        public int CopyAccountID { get; set; }

        [Parameter(DefaultValue = 1, MinValue = 0.1, MaxValue = 100)]
        public double Multiply { get; set; }

        [Parameter(DefaultValue = 0, MinValue = 0)]
        public int StopLossPips { get; set; }

        [Parameter(DefaultValue = 0, MinValue = 0)]
        public int TakeProfitPips { get; set; }

        [Parameter(DefaultValue = 500, MinValue = 100)]
        public int MaxPositions { get; set; }

        [Parameter(DefaultValue = "root")]
        public string User { get; set; }

        [Parameter(DefaultValue = "toor")]
        public string Password { get; set; }

        [Parameter(DefaultValue = "localhost")]
        public string ServerHost { get; set; }

        // cBot folder for opened position list
        public string path;

        // Mysql Connection variables
        public MySqlConnection connection;
        private string server;
        private string database;
        private double sleep;

        protected override void OnStart()
        {
            // show my webpage
            WebPage();

            // Set timer
            Timer.Start(TimerSeconds);

            // Create robot folder
            //path = "c:\\cBot" + Account.Number;
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\cBotID" + Account.Number;
            System.IO.Directory.CreateDirectory(path);

            //mysql server hostname            
            server = ServerHost;

            // database name for each cTrader trader account
            database = "breakermind_" + CopyAccountID;
            //database = "breakermind_9975139";

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + User + ";" + "PASSWORD=" + Password + ";";
            connection = new MySqlConnection(connectionString);
        }
        protected override void OnTimer()
        {
            try
            {
                connection.Open();
            } catch (MySqlException ex)
            {

                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.

                switch (ex.Number)
                {
                    case 0:
                        Print("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Print("Invalid username/password, please try again");
                        break;
                    default:
                        Print("Connected");
                        break;
                }

            }

            // Loop in one second interval 1s / DelayMillisecond (1-1000) = position refresh time (milliseconds)
            while (true)
            {
                // Copy open positions signals
                SelectOpenPositions();

                // Close positions signals
                SelectClosePositions();

                // Delay thread
                Thread.Sleep(DelayMillisecond);

                double sleep1 = sleep - Convert.ToDouble(DateTime.UtcNow.Second + "." + DateTime.UtcNow.Millisecond.ToString());
                Print("Position update loop (delay) : " + sleep1);
                sleep = Convert.ToDouble(DateTime.UtcNow.Second + "." + DateTime.UtcNow.Millisecond.ToString());
            }

            // close connection
            connection.Close();

            // Print time
            Print("Timer loop (UTC) " + DateTime.UtcNow.ToString("h:mm:ss tt"));
        }


        public bool CreateFile(string fileName = "0")
        {
            string path1 = "";
            path1 = path + "\\" + fileName + ".POS";
            if (!System.IO.File.Exists(path1))
            {
                try
                {
                    System.IO.FileStream fs = System.IO.File.Create(path1);
                } catch (DirectoryNotFoundException e)
                {
                    Print(e);
                }
                return false;
            }
            else
            {
                //Print("Position file \"{0}\" already exists. Don't copy.", fileName);
                return true;
            }
        }


        public bool DeleteFile(string fileName = "0")
        {
            string path1 = "";
            path1 = path + "\\" + fileName + ".POS";
            if (System.IO.File.Exists(path1))
            {
                try
                {
                    System.IO.File.Delete(path1);
                } catch (DirectoryNotFoundException e)
                {
                    Print(e);
                }
                return false;
            }
            else
            {
                //Print("Position file \"{0}\" already exists. Don't copy.", fileName);
                return true;
            }
        }


        public void WebPage(string web = "fxstar.eu")
        {
            // RUN WEB BROWSER with my web page
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "IExplore.exe";
            psi.Arguments = web;
            Process.Start(psi);
        }

        //Select statement
        public List<string>[] SelectOpenPositions()
        {
            Int32 TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string query = "SELECT * FROM opensignal where time >= NOW() - INTERVAL 6 HOUR ORDER BY id DESC LIMIT " + MaxPositions;

            //Create a list to store the result
            List<string>[] list = new List<string>[9];
            string[][] arr = new string[MaxPositions][];
            for (int x = 0; x < arr.Length; x++)
            {
                arr[x] = new string[8];
            }

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            int nr = 0;
            while (dataReader.Read())
            {
                // Console.WriteLine("\t{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                /// SET POSITION HERE DONT INSERT IN ARRAY
                /// create file with position or check is exist position with label in history or open positions

                arr[nr][0] = "" + dataReader["id"];
                arr[nr][1] = "" + dataReader["symbol"];
                arr[nr][2] = "" + dataReader["volume"];
                arr[nr][3] = "" + dataReader["type"];
                arr[nr][4] = "" + dataReader["opent"];
                arr[nr][5] = "" + dataReader["openp"];
                arr[nr][6] = "" + dataReader["sl"];
                arr[nr][7] = "" + dataReader["tp"];

                if (!CreateFile("" + dataReader["id"]))
                {
                    Symbol sym = MarketData.GetSymbol("" + dataReader["symbol"]);
                    //ExecuteMarketOrder(TradeType.Buy, sym, (Int32)dataReader["volume"], (string)dataReader["id"]);
                    if (arr[nr][3] == "BUY")
                    {
                        double vol = Int32.Parse(arr[nr][2].ToString()) * Multiply;
                        var result = ExecuteMarketOrder(TradeType.Buy, sym, (Int32)vol, (string)dataReader["id"] + "CopexFxStar");
                        if (result.IsSuccessful)
                        {
                            Print("=========> BUY Marker Order Open " + dataReader["id"] + " " + dataReader["symbol"] + " " + vol + " " + dataReader["type"]);
                            // Add StopLossPips for position
                            if (StopLossPips > 10)
                            {
                                var position = result.Position;
                                Print("Position SL price is {0}", position.StopLoss);
                                var stopLoss = position.EntryPrice - StopLossPips * Symbol.PipSize;
                                ModifyPosition(position, stopLoss, position.TakeProfit);
                                Print("New Position {1} SL price is {0}", position.StopLoss, position.Id);
                            }
                            // Add TakeProfitPips for position
                            if (TakeProfitPips > 10)
                            {
                                var position = result.Position;
                                Print("Position TP price is {0}", position.TakeProfit);
                                var takeProfit = position.EntryPrice + TakeProfitPips * Symbol.PipSize;
                                ModifyPosition(position, position.StopLoss, takeProfit);
                                Print("New Position {1} TP price is {0}", position.TakeProfit, position.Id);
                            }
                        }
                        // if open position error delete position file
                        if (!result.IsSuccessful)
                        {
                            DeleteFile("" + dataReader["id"]);
                        }
                    }

                    if (arr[nr][3] == "SELL")
                    {
                        double vol = Int32.Parse(arr[nr][2].ToString()) * Multiply;
                        var result = ExecuteMarketOrder(TradeType.Sell, sym, (Int32)vol, (string)dataReader["id"] + "CopexFxStar");
                        if (result.IsSuccessful)
                        {
                            Print("=========> SELL Marker Order Open " + dataReader["id"] + " " + dataReader["symbol"] + " " + vol + " " + dataReader["type"]);
                            // Add StopLossPips for position
                            if (StopLossPips > 10)
                            {
                                var position = result.Position;
                                Print("Position SL price is {0}", position.StopLoss);
                                var stopLoss = position.EntryPrice + StopLossPips * Symbol.PipSize;
                                ModifyPosition(position, stopLoss, position.TakeProfit);
                                Print("New Position {1} SL price is {0}", position.StopLoss, position.Id);
                            }
                            // Add TakeProfitPips for position
                            if (TakeProfitPips > 10)
                            {
                                var position = result.Position;
                                Print("Position TP price is {0}", position.TakeProfit);
                                var takeProfit = position.EntryPrice - TakeProfitPips * Symbol.PipSize;
                                ModifyPosition(position, position.StopLoss, takeProfit);
                                Print("New Position {1} TP price is {0}", position.TakeProfit, position.Id);
                            }
                        }
                        // if open position error delete position file
                        if (!result.IsSuccessful)
                        {
                            DeleteFile("" + dataReader["id"]);
                        }
                    }

                }
                nr++;
            }
            dataReader.Close();

            return list;
        }


        //Select statement
        public List<string>[] SelectClosePositions()
        {
            string query = "SELECT * FROM closesignal where time >= NOW() - INTERVAL 6 HOUR ORDER BY id DESC LIMIT " + MaxPositions;

            //Create a list to store the result
            List<string>[] list = new List<string>[9];
            string[][] arr = new string[MaxPositions][];
            for (int x = 0; x < arr.Length; x++)
            {
                arr[x] = new string[8];
            }

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            int nr = 0;
            while (dataReader.Read())
            {
                arr[nr][0] = "" + dataReader["id"];
                arr[nr][1] = "" + dataReader["closet"];
                arr[nr][2] = "" + dataReader["closep"];
                arr[nr][3] = "" + dataReader["profit"];
                arr[nr][4] = "" + dataReader["pips"];
                arr[nr][5] = "" + dataReader["account"];

                //Symbol sym = MarketData.GetSymbol("" + dataReader["symbol"]);
                string id = arr[nr][0];
                string label = id + "CopexFxStar";
                var Pos = Positions.Find(label);
                if (Pos != null)
                {
                    Print("Close ID " + label + " Pos " + Pos);
                    ClosePosition(Pos);
                    Print("!!! Close position Label: " + label + "  ID" + Pos);
                }
                nr++;
            }
            dataReader.Close();
            return list;
        }

    }
}

/*
    // directory files
    string[] picList = Directory.GetFiles(sourceDir, "*.jpg");
    Arrays :
    https://msdn.microsoft.com/en-us/library/aa288453%28v=vs.71%29.aspx
*/
