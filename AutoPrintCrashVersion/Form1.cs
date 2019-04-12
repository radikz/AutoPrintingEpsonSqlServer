using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace AutoPrintCrashVersion
{
    public partial class Form1 : Form
    {
       
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        FolderBrowserDialog Fld;
        String server = "192.168.88.23";
        String db = "etracking";
        String username = "sa";
        String pass = "mus282828";
        String fileName = "";
        
        String id;
        public Form1()
        {
            InitializeComponent();
            textBox4.UseSystemPasswordChar = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
          
            //Process p = new Process();
            
            
            /*
            textBox1.Text = server;
            textBox2.Text = db;
            textBox3.Text = username;
            textBox4.Text = pass;

            var proc1 = new ProcessStartInfo();
            String anyCommand = " " + "/k" + '\u0022' + @"C:\Program Files\Adobe\Acrobat 6.0\Acrobat\Acrobat.exe" + '\u0022' + " "+
                '\u002f' + "t" + " "+ '\u0022' + "spj.pdf" + '\u0022' + " " + '\u0022' + "EPSON L310 Series" + '\u0022'; 
            proc1.UseShellExecute = true;

            proc1.WorkingDirectory = @"C:\Windows\System32";

            proc1.FileName = @"cmd.exe";
            //proc1.Verb = "runas";
            proc1.Arguments = "/k " + anyCommand;
            //proc1.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(proc1);
      */
            timer1.Enabled = true;

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;

            

            string Command;
            //proc1.UseShellExecute = true;
            //Command = "\"" + @"lulu" + "\"";
            //proc1.WorkingDirectory = @"C:\Windows\System32";
            //proc1.FileName = @"C:\Windows\System32\cmd.exe";
            /// as admin = proc1.Verb = "runas";
            //proc1.Arguments = "/k " + "\"";
            //proc1.WindowStyle = ProcessWindowStyle.Maximized;
            //Process.Start(proc1);
            

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog Fld = new FolderBrowserDialog();
            Fld = new FolderBrowserDialog();

            // Set initial selected folder
            Fld.SelectedPath = "D:\\Anime" ;

            // Show the "Make new folder" button
            Fld.ShowNewFolderButton = true;

            if (Fld.ShowDialog() == DialogResult.OK)
            {
                // Select successful
                MessageBox.Show("The folder you selected is : " + Fld.SelectedPath);
                
                textBox5.Text = Fld.SelectedPath;
                fileName = textBox5.Text.ToString();
            }
           
        }

        
        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            //bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

            
            String str = String.Concat("server=", server, ";database=", db, ";UID=", username, ";password=", pass);
            // String str = "server=;database=etracking;UID=sa;password=Radikzid29()";
            SqlConnection con = new SqlConnection(str);
            con.Close();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            timer1.Start();
        }



        
        byte[] pdfFile;

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Process p = new Process();
            
            

            textBox1.Text = server;
            textBox2.Text = db;
            textBox3.Text = username;
            textBox4.Text = pass;

            
            try
            {
                String str = String.Concat("server=", server, ";database=", db, ";UID=", username, ";password=", pass);

                /*
                 SqlDependency sqlDependency = new SqlDependency(sqlCommand); // Also sets sqlCommand.Notification.
                 String str = String.Concat("server=", server, ";database=", db, ";UID=", username, ";password=", pass);
                 // String str = "server=;database=etracking;UID=sa;password=Radikzid29()";
                 String query = " insert";
                 //String query = "insert into tp.master_tanggal (id_karyawan) values (9)";
                 SqlConnection con = new SqlConnection(str);
                 SqlCommand cmd = new SqlCommand(query, con);
                 cmd.Notification = null;
                
                 //DataSet ds = new DataSet();
                 MessageBox.Show("connect with sql server");
               
                 //con.Close();
                 con.Open();
                 */
                
                SqlConnection Con = new SqlConnection(str);
                Con.Open();
                SqlCommand command = new SqlCommand("Select top 1 id_spj_berangkat from tp.spj_berangkat order by id_spj_berangkat desc", Con);
                //command.Parameters.AddWithValue("@zip", "india");
                // int result = command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //id = String.Format("{0}", reader["trip"]); 
                        id = String.Format("{0}", reader["id_spj_berangkat"]);
                        //pdfFile = (Byte[]) reader["pdf_upload"];
                        //MessageBox.Show(String.Format("{0}", reader["tujuan"]));
                        //Console.WriteLine(String.Format("{0}", reader["id"]));
                    }
                }
                //MessageBox.Show("berhasil");

                //buat spj.bat
                using (FileStream fs = File.Create(fileName + "\\spj.bat"))
                {
                    String cmd = "\"" + @"C:\Program Files\Adobe\Acrobat 6.0\Acrobat\Acrobat.exe" + "\"" +
                        " /t " + '\u0022' + "spj.pdf" + '\u0022' + " " + '\u0022' + "EPSON L310 Series" + '\u0022';
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(cmd);
                    fs.Write(title, 0, title.Length);
                }

                using (FileStream fs = File.Create(fileName + "\\KillPDF.bat"))
                {
                    String cmd = "timeout 15 && taskkill /im" + @" Acrobat.exe";
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(cmd);
                    fs.Write(title, 0, title.Length);
                }

                
                //File.WriteAllBytes("D:\\Anime\\kun.pdf", pdfFile);

                // Check if file already exists. If yes, delete it.     
               

                // Create a new file
                if (!File.Exists(fileName + "\\test.txt"))
                {
                 //   MessageBox.Show("yy");
                    using (FileStream fs = File.Create(fileName + "\\test.txt"))
                    {
                        // Add some text to file    
                        Byte[] title = new UTF8Encoding(true).GetBytes(id);
                        fs.Write(title, 0, title.Length);
                    }
                    command = new SqlCommand("Select top 1 pdf_upload from tp.spj_berangkat where id_spj_berangkat =@id", Con);
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pdfFile = (Byte[])reader["pdf_upload"];
                            //MessageBox.Show(String.Format("{0}", reader["tujuan"]));
                            //Console.WriteLine(String.Format("{0}", reader["id"]));
                        }
                    }
                    File.WriteAllBytes(Fld.SelectedPath + @"/spj.pdf", pdfFile);
                    MessageBox.Show("data masuk");

                    


                    //string batDir = string.Format(@"D:\");
                    Process proc1 = new Process();
                    proc1.StartInfo.WorkingDirectory = fileName;
                    proc1.StartInfo.FileName = "spj.bat";
                    proc1.StartInfo.CreateNoWindow = true;
                    proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //proc.StartInfo.Arguments = "/c";
                    proc1.Start();

                    Process proc = new Process();
                    proc.StartInfo.WorkingDirectory = fileName;
                    proc.StartInfo.FileName = "killPDF.bat";
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //proc.StartInfo.Arguments = "/c";
                    proc.Start();

                    

                    

                    //String anyCommand = '\u0022' + @"C:/Program" + '\u0022';
                   // proc1.UseShellExecute = true;
                    

                    //proc1.WorkingDirectory = @"C:\Windows\System32";

                    //proc1.FileName = @"cmd.exe";
                    //proc1.Arguments = "/k" + "\"";
                    //MessageBox.Show("\"");
                    //proc1.Verb = "runas";
                    //proc1.Arguments = "/k " + " " + '\u0022' + @"C:\Program Files\Adobe\Acrobat 6.0\Acrobat\Acrobat.exe" + '\u0022' + " " +
                    //        "/t" + " " + '\u0022' + Fld.SelectedPath + "\\spj.pdf" + '\u0022' + " " + '\u0022' + "EPSON L310 Series" + '\u0022';
                    
                    //proc1.WindowStyle = ProcessWindowStyle.Hidden;
                    //Process.Start(proc1);
                }
                else {
              //      MessageBox.Show("nn");
                    //jika file ada dan isi tidak berbeda
                    if (File.ReadAllText(fileName + "\\test.txt", Encoding.UTF8) == id)
                    {
                        //kill pdf
                        
                        //MessageBox.Show(File.ReadAllText(fileName, Encoding.UTF8)); 
                        //MessageBox.Show(tanggal);
                    }

                    else {
                        using (FileStream fs = File.Create(fileName + "\\test.txt"))
                        {
                            // Add some text to file    
                            Byte[] title = new UTF8Encoding(true).GetBytes(id);
                            fs.Write(title, 0, title.Length);

                            
                        }
                        command = new SqlCommand("Select top 1 pdf_upload from tp.spj_berangkat where id_spj_berangkat =@id", Con);
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pdfFile = (Byte[])reader["pdf_upload"];
                                //MessageBox.Show(String.Format("{0}", reader["tujuan"]));
                                //Console.WriteLine(String.Format("{0}", reader["id"]));
                            }
                        }
                        File.WriteAllBytes( Fld.SelectedPath + @"/spj.pdf", pdfFile);
                        MessageBox.Show("data masuk");


                        Process proc1 = new Process();
                        proc1.StartInfo.WorkingDirectory = fileName;
                        proc1.StartInfo.FileName = "spj.bat";
                        proc1.StartInfo.CreateNoWindow = true;
                        proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        //proc.StartInfo.Arguments = "/c";
                        proc1.Start();

                        Process proc = new Process();
                        proc.StartInfo.WorkingDirectory = fileName;
                        proc.StartInfo.FileName = "killPDF.bat";
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        //proc.StartInfo.Arguments = "/c";
                        proc.Start();

                        
                        
                       

                    }
                }
                

                //coba
                // create sql connection object.  Be sure to put a valid connection string

                // create command object with SQL query and link to connection object
                /*
                SqlCommand Cmd = new SqlCommand("insert into tp.spj_berangkat " + "(tujuan) " +
                    "VALUES(@tujuan)",
            Con);

                // create your parameters
                Cmd.Parameters.Add("@tujuan", System.Data.SqlDbType.VarChar);

                // set values to parameters from textboxes
                Cmd.Parameters["@tujuan"].Value = "dada";

                // open sql connection


                // execute the query and return number of rows affected, should be one
                int RowsAffected = Cmd.ExecuteNonQuery();
                */

                // close connection when done
                Con.Close();

              

            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        

        
    }
}
