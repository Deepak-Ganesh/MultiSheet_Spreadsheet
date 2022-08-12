using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
//using System.Windows.Forms;

namespace SS
{
    /// <summary>
    /// class used to test basic functionality of the client controller. will eventually be replaced by the gui when
    ///  the gui begins to have functionality to communicate via the client controller.
    /// </summary>
    public class TestClient
    {
        ClientController cc;
        
        // there should be a methodinvoker in form2 to let it know the connection has arrived
        //  however, this class cannot link up to a windows form. cannot test here.
        //MethodInvoker 

        public TestClient(ClientController ctl)
        {
            cc = ctl;
            cc.RegisterHandlers(ConnectionHandler, UpdateHandler, SpreadsheetListHandler, ErrorHandler);
        }

        public void test()
        {
            // hard coded the server stuff for the test client, for now
            cc.ConnectToServer("lab1-34.eng.utah.edu");

            System.Threading.Thread.Sleep(2000);

            List<string> list = new List<string>();
            list.Add("A2"); 
            list.Add("B1");
            EditMessage etest = new EditMessage("A1", "=A2+B1", list);

            OpenMessage rtest = new OpenMessage("my_spreadsheet.sprd", "username", "password");
            //OpenMessage rtest = new OpenMessage("testsprd.sprd", "username", "password");\

            Dictionary<string, dynamic> st = new Dictionary<string, dynamic>();
            st.Add("A1", "=2*A1+1");
            st.Add("B3", "42");
            st.Add("B6", 42);
            st.Add("C12", "");
            FullSendMessage fsmtest = new FullSendMessage(st);
            fsmtest.ToString();

            cc.SendData(rtest.ToString());
            
        }
        
        public void ConnectionHandler()
        {
            //stub
        }

        public void UpdateHandler(object fsm)
        {
            // fullsend needs to be a class / object of its own, not a string
            //stub
        }

        public void SpreadsheetListHandler(SpreadsheetListMessage sslm)
        {
            //stub
        }

        public void ErrorHandler(ErrorMessage EM)
        {
            //stub
        }
        
    }



    //static class Program
    //{

    //    [STAThread]
    //    static void Main()
    //    {
    //        TestClient.testClient();

    //    }
    //}
}



//SpreadsheetListMessage sslmtest = SpreadsheetListMessage.DeserializeSSLM("sadfwrb43/n");
//string etestJSON = etest.ToString();

////Parse the received data
//string[] stest = new string[2];
//stest[0] = "sprd.sprd";
//stest[1] = "test.sprd";
//SpreadsheetListMessage stestmessage = new SpreadsheetListMessage(stest);
//dynamic msgObj = JObject.Parse(stestmessage.ToString());

////Grab the JSON element type which will be used to check the type of object
//JToken Type = msgObj["type"];
//string testy = Type.ToString();
//if (testy != "list")
//    //if (Type != null)
//    {

//    throw new FormatException();
//}
//else
//{
//    throw new InvalidTimeZoneException();
//}