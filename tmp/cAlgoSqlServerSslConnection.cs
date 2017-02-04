using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
// SQL Server
using System.Data.SqlClient;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.Internet | AccessRights.FileSystem | AccessRights.FullAccess)]
    public class _aTest : Robot
    {
        [Parameter(DefaultValue = 0.0)]
        public double Parameter { get; set; }

        protected override void OnStart()
        {
            try
            {
                SqlConnection Conn = new SqlConnection("user id=sa;" + "password=SQL888;server=localhost;" + "Trusted_Connection=yes;" + "database=breakermind; " + "connection timeout=30");

                Conn.Open();
                SqlCommand Cmd = new SqlCommand("INSERT INTO OpenSignal(id,symbol,volume) VALUES('1','GBPJPY','10000')", Conn);
                Cmd.ExecuteNonQuery();


                // Read data from tables
                SqlDataReader Reader = null;
                Cmd = new SqlCommand("select * from OpenSignal", Conn);
                Reader = Cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Print(Reader["id"].ToString());
                    Print(Reader["symbol"].ToString());
                    Print(Reader["volume"].ToString());
                }

            } catch (Exception eee)
            {
                Print(eee);
            }
        }

        protected override void OnTick()
        {
            // StopLevelPrice(StopEquity)
            // StopLevelPrice(10);
        }

        protected override void OnStop()
        {
            Print("cBot has been stoped!");
        }

        //=========================================================================
        public void StopLevelPrice(double StopEquity = 0, bool StopCbot = true)
        {
            ChartObjects.DrawText("Equity/StopLossEquity", "Equity: " + Account.Equity.ToString() + " / StopLossEquity: " + StopEquity, StaticPosition.BottomRight, Colors.Yellow);
            if (StopCbot)
            {
                CloseAllPositionsStop();
            }
            else
            {
                CloseAllPositions();
            }
        }

        public void CloseAllPositionsStop()
        {
            foreach (var position in Positions)
            {
                if (position != null)
                {
                    ClosePosition(position);
                }
                if (Positions.Count == 0)
                {
                    Stop();
                }
            }
        }

        public void CloseAllPositions()
        {
            foreach (var position in Positions)
            {
                if (position != null)
                {
                    ClosePosition(position);
                }
            }
        }
        //=========================================================================
    }
}
