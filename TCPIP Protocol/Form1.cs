using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Globalization;

namespace TCPIP_Protocol
{
    public partial class Form1 : Form
    {
        // ------------------------------------------------------------------------
        // Constants for access
        private const byte fctReadCoil = 1;
        private const byte fctReadDiscreteInputs = 2;
        private const byte fctReadHoldingRegister = 3;
        private const byte fctReadInputRegister = 4;
        private const byte fctWriteSingleCoil = 5;
        private const byte fctWriteSingleRegister = 6;
        private const byte fctWriteMultipleCoils = 15;
        private const byte fctWriteMultipleRegister = 16;
        private const byte fctReadWriteMultipleRegister = 23;

        /// <summary>Constant for exception illegal function.</summary>
        public const byte excIllegalFunction = 1;
        /// <summary>Constant for exception illegal data address.</summary>
        public const byte excIllegalDataAdr = 2;
        /// <summary>Constant for exception illegal data value.</summary>
        public const byte excIllegalDataVal = 3;
        /// <summary>Constant for exception slave device failure.</summary>
        public const byte excSlaveDeviceFailure = 4;
        /// <summary>Constant for exception acknowledge. This is triggered if a write request is executed while the watchdog has expired.</summary>
        public const byte excAck = 5;
        /// <summary>Constant for exception slave is busy/booting up.</summary>
        public const byte excSlaveIsBusy = 6;
        /// <summary>Constant for exception gate path unavailable.</summary>
        public const byte excGatePathUnavailable = 10;
        /// <summary>Constant for exception not connected.</summary>
        public const byte excExceptionNotConnected = 253;
        /// <summary>Constant for exception connection lost.</summary>
        public const byte excExceptionConnectionLost = 254;
        /// <summary>Constant for exception response timeout.</summary>
        public const byte excExceptionTimeout = 255;
        /// <summary>Constant for exception wrong offset.</summary>
        private const byte excExceptionOffset = 128;
        /// <summary>Constant for exception send failt.</summary>
        private const byte excSendFailt = 100;

        // ------------------------------------------------------------------------
        // Private declarations
        private static ushort _timeout = 500;//arsath changed//500
        private static ushort _refresh = 10;
        private static bool _connected = false;
        private static bool _no_sync_connection = false;
        private static string data_in_hex;//arsath declared
        public int x;
        public string x1;
        public string x2;
        public string x_val ="";
        public decimal d_val;
        decimal[] all_params = new decimal[16];
        byte[] data = new byte[12];

        private Socket tcpAsyCl;
        private byte[] tcpAsyClBuffer = new byte[2048];

        public byte[] res_val = new byte[2048];

        private Socket tcpSynCl;
        private byte[] tcpSynClBuffer = new byte[2048];

        // ------------------------------------------------------------------------
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public delegate void ResponseData(ushort id, byte unit, byte function, byte[] data);
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public event ResponseData OnResponseData;
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public delegate void ExceptionData(ushort id, byte unit, byte function, byte exception);
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public event ExceptionData OnException;

        //Used to store the datas
        string meter_status = "";
        //TcpClient clientsocket = new TcpClient();
        //NetworkStream serverstream = default(NetworkStream);
        //string readdata = null;
        //byte trans_id = 1;
        //byte proto_id = 0;
        //byte data_len = 6;
        //byte st_add = 0;
        //byte no_ele = 64;
        //byte[] tcp_cmd = new byte[12];
        //byte[] bytes = new byte[1024];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //clientsocket.Connect(textBox1.Text, Int32.Parse(textBox2.Text));
            //Thread ctthread = new Thread(getmessage);
            //ctthread.Start();
            //tcp_data();
            //tcp_protocol();
           // string ipad = textBox1.Text;
            //ushort portno = Convert.ToUInt16(textBox2.Text);
            Master("192.168.1.177", 502, false);
           // Master(ipad, portno, false);
            ReadHoldingRegister(31, 1, 1920, 64, ref res_val);//id,unit,startaddress,noofele,resval
            disconnect();
            //getdatatogrid(data_in_hex);

        }
        //public void tcp_protocol()
        //{
        //    try
        //    {
        //        const string ipadd = "192.168.1.177";
        //        const int portadd = 502;
        //        TcpClient client = new TcpClient();
        //        client.Connect(ipadd, portadd);
                
               
        //        tcp_cmd[0] = Convert.ToByte(trans_id / 256);
        //        tcp_cmd[1] = Convert.ToByte(trans_id % 256);
        //        tcp_cmd[2] = Convert.ToByte(proto_id / 256);
        //        tcp_cmd[3] = Convert.ToByte(proto_id % 256);
        //        tcp_cmd[4] = Convert.ToByte(data_len / 256);
        //        tcp_cmd[5] = Convert.ToByte(data_len % 256);
        //        tcp_cmd[6] = 1;
        //        tcp_cmd[7] = 3;
        //        tcp_cmd[8] = Convert.ToByte(st_add / 256);
        //        tcp_cmd[9] = Convert.ToByte(st_add % 256);
        //        tcp_cmd[10] = Convert.ToByte(no_ele / 256);
        //        tcp_cmd[11] = Convert.ToByte(no_ele % 256);

        //        var tcpcmd ="";

        //        for (int i = 0; i < 11; i++)
        //        {
        //            tcpcmd += tcp_cmd[i];
        //        }

        //        string hex_1 = BitConverter.ToString(tcp_cmd).Replace("-", string.Empty);

        //        //string hexa_value = ByteArrayToString(tcp_cmd);

        //        byte[] data = Encoding.Default.GetBytes(hex_1);

        //        NetworkStream stream = client.GetStream();

        //        stream.Write(data, 0, data.Length);

        //        data = new byte[256];

        //        string responsedata = string.Empty;

        //        while (stream.DataAvailable)
        //        {
        //            Int32 bytes = stream.Read(data, 0, data.Length);
        //            responsedata = Encoding.ASCII.GetString(data, 0, bytes);                    
        //       }

        //        stream.Close();
        //        client.Close();
        //    }
        //    catch
        //    {

        //    }
        //}

        //public static string ByteArrayToString(byte[] ba)
        //{
        //    StringBuilder hex = new StringBuilder(ba.Length * 2);
        //    foreach (byte b in ba)
        //        hex.AppendFormat("{0:x2}", b);
        //    return hex.ToString();
        //}


        //public void tcp_data()
        //{
        //    // Data buffer for incoming data.  
        //    byte[] bytes = new byte[1024];
        //    var tcpcmd = "";
        //    // Connect to a remote device.  
        //    try
        //    {
        //        // Establish the remote endpoint for the socket.  
        //        // This example uses port 11000 on the local computer.  
        //        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //        //IPAddress ipAddress = ipHostInfo.AddressList[0];                
        //        IPAddress ipAddress = IPAddress.Parse("192.168.1.177");
        //        IPEndPoint remoteEP = new IPEndPoint(ipAddress, 502);

        //        // Create a TCP/IP  socket.  
        //        Socket sender = new Socket(ipAddress.AddressFamily,
        //            SocketType.Stream, ProtocolType.Tcp);

        //        // Connect the socket to the remote endpoint. Catch any errors.  
        //        try
        //        {
        //            sender.Connect(remoteEP);

        //            MessageBox.Show("Socket connected to {0}", sender.RemoteEndPoint.ToString());
        //            //Console.WriteLine("Socket connected to {0}",sender.RemoteEndPoint.ToString());

        //            // Encode the data string into a byte array.  
        //            tcp_cmd[0] = Convert.ToByte(trans_id / 256);
        //            tcp_cmd[1] = Convert.ToByte(trans_id % 256);
        //            tcp_cmd[2] = Convert.ToByte(proto_id / 256);
        //            tcp_cmd[3] = Convert.ToByte(proto_id % 256);
        //            tcp_cmd[4] = Convert.ToByte(data_len / 256);
        //            tcp_cmd[5] = Convert.ToByte(data_len % 256);
        //            tcp_cmd[6] = 1;
        //            tcp_cmd[7] = 3;
        //            tcp_cmd[8] = Convert.ToByte(st_add / 256);
        //            tcp_cmd[9] = Convert.ToByte(st_add % 256);
        //            tcp_cmd[10] = Convert.ToByte(no_ele / 256);
        //            tcp_cmd[11] = Convert.ToByte(no_ele % 256);

        //            for (int i = 0; i < 11; i++)
        //            {
        //                tcpcmd += tcp_cmd[i];
        //            }

        //           string gethex = ByteArrayToString(tcp_cmd);


        //            byte msg = Convert.ToByte(gethex);

        //            byte[] msg1 = addByteToArray()

        //            // Send the data through the socket.  
        //            int bytesSent = sender.Send(msg);

        //            // Receive the response from the remote device.  
        //            int bytesRec = sender.Receive(bytes);
        //            //MessageBox.Show("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
        //            //Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

        //            // Release the socket.  
        //            //sender.Shutdown(SocketShutdown.Both);
        //            //sender.Close();

        //        }
        //        catch (ArgumentNullException ane)
        //        {
        //            Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
        //        }
        //        catch (SocketException se)
        //        {
        //            Console.WriteLine("SocketException : {0}", se.ToString());
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Unexpected exception : {0}", e.ToString());
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        //public static string ByteArrayToString(byte[] ba)
        //{
        //    StringBuilder hex = new StringBuilder(ba.Length * 2);
        //    foreach (byte b in ba)
        //        hex.AppendFormat("{0:x2}", b);
        //    return hex.ToString();
        //}

        //public byte[] addByteToArray(byte[] bArray, byte newByte)
        //{
        //    byte[] newArray = new byte[bArray.Length + 1];
        //    bArray.CopyTo(newArray, 1);
        //    newArray[0] = newByte;
        //    return newArray;
        //}


        //public void getmessage()
        //{
        //    string returndata;
        //    var tcpcmd = "";


        //    while (true)
        //    {
        //        serverstream = clientsocket.GetStream();
        //        var buffersize = clientsocket.ReceiveBufferSize;
        //        byte[] instream = new byte[buffersize];


        //        //serverstream.Read(instream, 0, buffersize);
        //        tcp_cmd[0] = Convert.ToByte(trans_id / 256);
        //        tcp_cmd[1] = Convert.ToByte(trans_id % 256);
        //        tcp_cmd[2] = Convert.ToByte(proto_id / 256);
        //        tcp_cmd[3] = Convert.ToByte(proto_id % 256);
        //        tcp_cmd[4] = Convert.ToByte(data_len / 256);
        //        tcp_cmd[5] = Convert.ToByte(data_len % 256);
        //        tcp_cmd[6] = 1;
        //        tcp_cmd[7] = 3;
        //        tcp_cmd[8] = Convert.ToByte(st_add / 256);
        //        tcp_cmd[9] = Convert.ToByte(st_add % 256);
        //        tcp_cmd[10] = Convert.ToByte(no_ele / 256);
        //        tcp_cmd[11] = Convert.ToByte(no_ele % 256);

        //        for (int i = 0; i < 11; i++)
        //        {
        //            tcpcmd += tcp_cmd[i];
        //        }               

        //            //returndata = System.Text.Encoding.ASCII.GetString(instream);
        //        returndata = System.Text.Encoding.ASCII.GetString(tcp_cmd);

        //        readdata = returndata;
        //        msg();
        //    }
        //}

        //public void msg()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new MethodInvoker(msg));
        //    }
        //    else
        //    {
        //        richTextBox2.Text = readdata;
        //    }
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            //byte[] outstream = Encoding.ASCII.GetBytes(richTextBox1.Text);
            //serverstream.Write(outstream, 0, outstream.Length);
            //serverstream.Flush();
        }

        private void getdatatogrid(string hexval)
        {
            x_val = hexval;
                     
            //start 
            x_val = x_val.Substring(0, 4);
            x = int.Parse(x_val, NumberStyles.HexNumber);
            int tag_no = x;//meter_id
            
            
            x_val = hexval.Remove(0, 4);
            //meter_status----start
            x_val = x_val.Substring(0, 4);
            x = int.Parse(x_val, NumberStyles.HexNumber);            
            switch (x)
            {
                case 0:
                    meter_status = "NC";
                    break;
                case 1:
                    meter_status = "Good";
                    break;
                case 2:
                    meter_status = "Fail";
                    break;
                default:
                    meter_status = "NC";
                    break;
            }           
            //meter_status-----end
            //r_voltage
            //x_val = hexval.Remove(0, 8);
            //x_val = x_val.Substring(0, 4);
            //x = int.Parse(x_val, NumberStyles.HexNumber);
            //d_val = Convert.ToDecimal(x * 0.1);
            

            int start_val = 8;
            for (int i = 0; i < 12; i++)//line_volt..phase_volt...Current...kw...kva...kvar
            {                           
                x_val = hexval.Remove(0, start_val);
                x_val = x_val.Substring(0, 4);
                x = int.Parse(x_val, NumberStyles.HexNumber);
                d_val = Convert.ToDecimal(x * 0.1);
                all_params[i] = d_val;
                start_val = start_val + 4;
            }
            //start pf
            x_val = hexval.Remove(0, start_val);
            x_val = x_val.Substring(0, 4);
            x = int.Parse(x_val, NumberStyles.HexNumber);
            d_val = call_pf(Convert.ToDecimal(x * 0.1));
            all_params[12] = d_val;
            start_val = start_val + 4;
            //end pf
            //start hz
            x_val = hexval.Remove(0, start_val);
            x_val = x_val.Substring(0, 4);
            x = int.Parse(x_val, NumberStyles.HexNumber);
            d_val = x / 100;
            all_params[13] = d_val;
            start_val = start_val + 4;
            //end hz
            //kwh start
            if (meter_status == "Good")
            {
                x_val = hexval.Remove(0, start_val);
                x1 = x_val.Substring(0, 4);
                x2 = x_val.Substring(4, 4);
                int kwh1, kwh2;
                kwh1 = int.Parse(x1, NumberStyles.HexNumber);
                kwh2 = int.Parse(x2, NumberStyles.HexNumber);
                x = (kwh1 * 65536) + kwh2;
                d_val = Convert.ToDecimal(x * 0.1);
                all_params[14] = d_val;
                start_val = start_val + 10;
            }
            else
            {
                all_params[14] = 0;
                start_val = start_val + 10;
            }
            //kwh end
            //run_hr start
            if (meter_status == "Good")
            {
                x_val = hexval.Remove(0, start_val);
                x1 = x_val.Substring(0, 2);
                x2 = x_val.Substring(2, 4);
                int runhr1, runhr2;
                runhr1 = int.Parse(x1, NumberStyles.HexNumber);
                runhr2 = int.Parse(x2, NumberStyles.HexNumber);
                x = (runhr1 * 65536) + runhr2;//1525
                decimal runhr3, runhr4;
                runhr3 = x / 100;//15
                runhr4 = x % 100;//25
                string len1 = runhr_len(runhr3.ToString());
                string len2 = runhr_len(runhr4.ToString());
                string final_len = len1 + "." + len2;
                decimal runhr_res = Convert.ToDecimal(final_len);                
                all_params[15] = runhr_res;
                start_val = start_val + 8;
            }
            else
            {
                all_params[15] = 0;
                start_val = start_val + 8;
            }
            //run_hr end
            DataTable dt = new DataTable();
            dt.Columns.Add("Parameters");
            dt.Columns.Add("Datas");
            dt.Rows.Add("Tag_no");
            dt.Rows.Add("Meter_status");
            dt.Rows.Add("R_voltage");
            dt.Rows.Add("Y_voltage");
            dt.Rows.Add("B_voltage");
            dt.Rows.Add("RY_phase");
            dt.Rows.Add("YB_phase");
            dt.Rows.Add("BR_phase");
            dt.Rows.Add("R_current");
            dt.Rows.Add("Y_current");
            dt.Rows.Add("B_current");
            dt.Rows.Add("KVA");
            dt.Rows.Add("KW");
            dt.Rows.Add("KVAR");
            dt.Rows.Add("PF");
            dt.Rows.Add("HZ");
            dt.Rows.Add("KWH");
            dt.Rows.Add("RUN_HR");

            

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (j == 0)
                {
                    dt.Rows[j][1] = tag_no;
                }
                else if (j == 1)
                {
                    dt.Rows[j][1] = meter_status;
                }
                else
                {
                    dt.Rows[j][1] = all_params[j-2];
                }
                
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

        public string runhr_len(string val)
        {
            int len_val = val.Length;

            switch (len_val)
            {
                case 0:
                    val = "00";
                    break;
                case 1:
                    val = "0" + val;
                    break;
                case 2:
                    val = val;
                    break;
                default:
                    val = val;
                    break;
            }
            return val;
        }

        //private string run_hr(decimal runhr)
        //{
        //    string run_hrr;
        //    run_hrr = assignleadzero(runhr / 100).ToString();
        //    run_hrr += "." + assignleadzero(runhr % 100).ToString();
        //    return run_hrr;
        //}

        //private decimal assignleadzero(string getval)
        //{
        //    switch (getval.Length)
        //    {
        //        case 0:

        //    }
        //}

        private decimal call_pf(decimal pf)
        {
            decimal res_val;
            if (pf < 1000)
            {
                res_val = 1 * (pf * Convert.ToDecimal(0.001));
            }
            else if (pf > 1000)
            {
                pf = pf - 1000;
                res_val = 1 * (pf * Convert.ToDecimal(0.001));
            }
            else if (pf == 1000)
            {
                res_val = pf / 1000;
            }
            else
            {
                res_val = pf;
            }
            return res_val;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        ///         
        /// <summary>Read holding registers from slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        //public void ReadHoldingRegister(ushort id, byte unit, ushort startAddress, ushort numInputs)
        //{
        //    if (numInputs > 125)
        //    {
        //        CallException(id, unit, fctReadHoldingRegister, excIllegalDataVal);
        //        return;
        //    }
        //    WriteAsyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadHoldingRegister), id);
        //}
        
        /// <summary>Read holding registers from slave synchronous.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        /// <param name="values">Contains the result of function.</param>
        public void ReadHoldingRegister(ushort id, byte unit, ushort startAddress, ushort numInputs, ref byte[] values)
        {
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadHoldingRegister, excIllegalDataVal);
                return;
            }

            //byte[] wri_data = CreateReadHeader(id, unit, startAddress, numInputs, fctReadHoldingRegister);
            //Thread.Sleep(5000);
            values = WriteSyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadHoldingRegister), id);

            data_in_hex = BitConverter.ToString(values).Replace("-", string.Empty);

            getdatatogrid(data_in_hex);
        }
        
        /// <summary>Response timeout. If the slave didn't answers within in this time an exception is called.</summary>
        /// <value>The default value is 500ms.</value>
        public ushort timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        
        /// <summary>Refresh timer for slave answer. The class is polling for answer every X ms.</summary>
        /// <value>The default value is 10ms.</value>
        public ushort refresh
        {
            get { return _refresh; }
            set { _refresh = value; }
        }
        
        /// <summary>Displays the state of the synchronous channel</summary>
        /// <value>True if channel was diabled during connection.</value>
        public bool NoSyncConnection
        {
            get { return _no_sync_connection; }
        }
        
        /// <summary>Shows if a connection is active.</summary>
        public bool connected
        {
            get { return _connected; }
        }
        
        /// <summary>Create master instance with parameters.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        //public void Master(string ip, ushort port)
        //{
        //    connect(ip, port, false);
        //}
        
        /// <summary>Create master instance with parameters.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        /// <param name="no_sync_connection">Disable second connection for synchronous requests</param>
        public void Master(string ip, ushort port, bool no_sync_connection)
        {
            connect(ip, port, no_sync_connection);
        }
        
        /// <summary>Start connection to slave.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        /// <param name="no_sync_connection">Disable sencond connection for synchronous requests</param>
        public void connect(string ip, ushort port, bool no_sync_connection)
        {
            try
            {
                IPAddress _ip;
                _no_sync_connection = no_sync_connection;
                if (IPAddress.TryParse(ip, out _ip) == false)
                {
                    IPHostEntry hst = Dns.GetHostEntry(ip);
                    ip = hst.AddressList[0].ToString();
                }                
                //// Connect asynchronous client
                //tcpAsyCl = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //tcpAsyCl.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                //tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _timeout);
                //tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _timeout);
                //tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);                
                // Connect synchronous client
                if (!_no_sync_connection)
                {
                    tcpSynCl = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    tcpSynCl.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _timeout);
                    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _timeout);
                    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);
                }
                _connected = true;              
            }
            catch (System.IO.IOException error)
            {
                _connected = false;
                throw (error);
            }
        }
        
        /// <summary>Stop connection to slave.</summary>
        public void disconnect()
        {
            Dispose_con();
        }
        
        ///// <summary>Destroy master instance.</summary>
        //~Master()
        //{
        //    Dispose();
        //}
        
        /// <summary>Destroy master instance</summary>
        public void Dispose_con()
        {
            //if (tcpAsyCl != null)
            //{
            //    if (tcpAsyCl.Connected)
            //    {
            //        try { tcpAsyCl.Shutdown(SocketShutdown.Both); }
            //        catch { }
            //        tcpAsyCl.Close();
            //    }
            //    tcpAsyCl = null;
            //}
            if (tcpSynCl != null)
            {
                if (tcpSynCl.Connected)
                {
                    try { tcpSynCl.Shutdown(SocketShutdown.Both); }
                    catch { }
                    tcpSynCl.Close();
                }
                tcpSynCl = null;
            }
        }
          
        // Create modbus header for read action
        private byte[] CreateReadHeader(ushort id, byte unit, ushort startAddress, ushort length, byte function)
        {
           

            byte[] _id = BitConverter.GetBytes((short)id);
            data[0] = _id[1];			    // Slave id high byte
            data[1] = _id[0];				// Slave id low byte
            data[5] = 6;					// Message size
            data[6] = unit;					// Slave address
            data[7] = function;				// Function code
            byte[] _adr = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startAddress));
            data[8] = _adr[0];				// Start address
            data[9] = _adr[1];				// Start address
            byte[] _length = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)length));
            data[10] = _length[0];			// Number of data to read
            data[11] = _length[1];			// Number of data to read
            return data;
        }

         private byte[] WriteSyncData(byte[] write_data, ushort id)
        {
            if (tcpSynCl.Connected)
            {
                try
                {
                    tcpSynCl.Send(write_data, 0, write_data.Length, SocketFlags.None);
                    Thread.Sleep(10000);
                    int result = tcpSynCl.Receive(tcpSynClBuffer, 0, tcpSynClBuffer.Length, SocketFlags.None);
                    

                    byte unit = tcpSynClBuffer[6];
                    byte function = tcpSynClBuffer[7];
                    byte[] data;

                    if (result == 0)
                    { 
                        CallException(id, unit, write_data[7], excExceptionConnectionLost); 
                    }

                    // Response data is slave exception
                    if (function > excExceptionOffset)
                    {
                        function -= excExceptionOffset;
                        CallException(id, unit, function, tcpSynClBuffer[8]);
                        return null;
                    }

                    //// Write response data
                    //else if ((function >= fctWriteSingleCoil) && (function != fctReadWriteMultipleRegister))
                    //{
                    //    data = new byte[2];
                    //    Array.Copy(tcpSynClBuffer, 10, data, 0, 2);
                    //}

                    // Read response data
                    else
                    {                        
                        data = new byte[tcpSynClBuffer[8]];
                        Array.Copy(tcpSynClBuffer, 9, data, 0, tcpSynClBuffer[8]);
                    }
                    return data;
                }
                catch (SystemException)
                {
                    CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
                }
            }
            else
            {
                CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
            }

            return null;
        }

        // private void WriteAsyncData(byte[] write_data, ushort id)
        //{
        //    if ((tcpAsyCl != null) && (tcpAsyCl.Connected))
        //    {
        //        try
        //        {
        //            tcpAsyCl.BeginSend(write_data, 0, write_data.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
        //        }
        //        catch (SystemException)
        //        {
        //            CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
        //        }
        //    }
        //    else
        //    {
        //        CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
        //    }
        //}

        //private void OnSend(System.IAsyncResult result)
        //{
        //    Int32 size = tcpAsyCl.EndSend(result);
        //    if (result.IsCompleted == false)
        //    {
        //        CallException(0xFFFF, 0xFF, 0xFF, excSendFailt);
        //    }
        //    else
        //    {
        //        tcpAsyCl.BeginReceive(tcpAsyClBuffer, 0, tcpAsyClBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), tcpAsyCl);
        //    }
        //}

        
        // Write asynchronous data response
        //private void OnReceive(System.IAsyncResult result)
        //{
        //    if (tcpAsyCl == null)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        tcpAsyCl.EndReceive(result);
        //        if (result.IsCompleted == false) CallException(0xFF, 0xFF, 0xFF, excExceptionConnectionLost);
        //    }
        //    catch (Exception) { 
        //    }

        //    ushort id = SwapUInt16(BitConverter.ToUInt16(tcpAsyClBuffer, 0));
        //    byte unit = tcpAsyClBuffer[6];
        //    byte function = tcpAsyClBuffer[7];
        //    byte[] data;
            
        //    // Write response data
        //    if ((function >= fctWriteSingleCoil) && (function != fctReadWriteMultipleRegister))
        //    {
        //        data = new byte[2];
        //        Array.Copy(tcpAsyClBuffer, 10, data, 0, 2);
        //    }
            
        //    // Read response data
        //    else
        //    {
        //        data = new byte[tcpAsyClBuffer[8]];               
        //        Array.Copy(tcpAsyClBuffer, 9, data, 0, tcpAsyClBuffer[8]); 
        //    }
            
        //    // Response data is slave exception
        //    if (function > excExceptionOffset)
        //    {
        //        function -= excExceptionOffset;
        //        CallException(id, unit, function, tcpAsyClBuffer[8]);
        //    }
            
        //    // Response data is regular data
        //    else if (OnResponseData != null)
        //    {
        //        OnResponseData(id, unit, function, data);
        //    }
        //}

        // public static UInt16 SwapUInt16(UInt16 inValue)//I changes internal to public
        //{
        //    return (UInt16)(((inValue & 0xff00) >> 8) |
        //             ((inValue & 0x00ff) << 8));
        //}

         public void CallException(ushort id, byte unit, byte function, byte exception)//I changed internal keyword to public
         {
             if ((tcpAsyCl == null) || (tcpSynCl == null && !_no_sync_connection))
             {
                 return;
             }
             if (exception == excExceptionConnectionLost)
             {
                 tcpSynCl = null;
                 tcpAsyCl = null;
             }
             if (OnException != null)
             {
                 OnException(id, unit, function, exception);
             }
         }
    }
}
