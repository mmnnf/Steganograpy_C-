using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;



namespace Steganography
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string dosyayolu = "";
        Bitmap bmp;
        string gizledilecek_data = "";
        int [] mm;
        int [] nn;
        static String evvel = "";
        static String sonra = "";
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            dosyayolu = openFileDialog1.FileName;
            bmp = new Bitmap(dosyayolu);
            pictureBox1.Image = bmp;
             byte[] mmb = takergba(bmp);
             mm = new int[mmb.Length];
             for (int i = 0; i < mm.Length; i++)
             {
                 mm[i] = mmb[i];
             }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "jpeg files(*.jpg)|*.jpg";
            if (DialogResult.OK == sfd.ShowDialog())
            {
                this.pictureBox2.Image.Save(sfd.FileName);
            }
        }
       string gizledilecekdata_binnary = "";
       string txt1 = "";
       Bitmap bmp2;
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            dosyayolu = openFileDialog1.FileName;
            string fileType = Path.GetExtension(dosyayolu);
            if (fileType==".txt")
            {
                StreamReader read = new StreamReader(File.OpenRead(dosyayolu));
                gizledilecek_data = read.ReadToEnd();
                textBox1.Text = gizledilecek_data;
                read.Dispose();
                int[] nn = new int[gizledilecek_data.Length];
                for (int i = 0; i < gizledilecek_data.Length; i++)
                {
                    nn[i]=(int)gizledilecek_data[i];
                }
            }
            else if (fileType == ".jpg")
            {
                  bmp2 = new Bitmap(dosyayolu);
                pictureBox1.Image = bmp;
                //byte [] nnb=takergba(bmp); //@@@@@@ @@@@@@   @@@@@@ @@@@@@  @2@@@
                byte []nnb = takergba(bmp2);
                nn = new int[nnb.Length];
                for (int i = 0; i < nn.Length; i++)
                {
                    nn[i] = nnb[i];
                } 
            } 
        }
        static byte[] oobyte;
        private void button3_Click(object sender, EventArgs e)
        { 
            int[] oo;
            if ((bmp2.Width)*bmp2.Height*8+4>bmp.Height*bmp.Width)
            {
                MessageBox.Show("yerlesmir");
            }
            else
            {
                Stopwatch ww = new Stopwatch();
                ww.Start();
               
            oo = tamSTEGOimageLIST (mm, nn, bmp2.Width, bmp2.Height);
            ww.Stop();

            oobyte = new byte[oo.Length];
            int g = 0;
            for (int i = 0; i < oo.Length; i++)
            {
                oobyte[i] = Convert.ToByte(oo[i]);
                if (oobyte[i]!=oo[i])
                {
                    MessageBox.Show("alinmir");   
                }
            }
            
            Bitmap resim = bmp;
            Bitmap yeniResim = new Bitmap(resim.Width, resim.Height, PixelFormat.Format24bppRgb);

            int dizisayisi;
            IntPtr baslangicy;  
            Rectangle rcty;
            BitmapData bmdatay;
            rcty = new Rectangle(0, 0, resim.Width, resim.Height);
            bmdatay = yeniResim.LockBits(rcty, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            baslangicy = bmdatay.Scan0;
            dizisayisi = oobyte.Length;
            Marshal.Copy(oobyte, 0, baslangicy, dizisayisi);
            yeniResim.UnlockBits(bmdatay);
            pictureBox2.Image = yeniResim;
            MessageBox.Show("gizledildi");
                Bitmap kk=new Bitmap(yeniResim);
                ee = takergba(kk);
                ee = oobyte;

            }
        }
        static byte[] ee;
        //////////////////////////////////////////////////////////////////
        public static Byte[] takergba(Bitmap resim)
        { 
            int dizisayisi;
            IntPtr baslangic;
            byte[] rgbdeger;
            Rectangle rct;
            BitmapData bmdata;
            rct = new Rectangle(0, 0, resim.Width, resim.Height);
            bmdata = resim.LockBits(rct, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            baslangic = bmdata.Scan0;
            dizisayisi = bmdata.Stride * resim.Height;
            rgbdeger = new byte[dizisayisi];
            Marshal.Copy(baslangic, rgbdeger, 0, dizisayisi);
            resim.UnlockBits(bmdata);
            //pictureBox1.Image = yeniResim;
            return rgbdeger; 
        }
        public static StringBuilder ff = new StringBuilder();
         
        public static int[] tamTERSSTEGO_LIST(int[] mm)
        {
            int[] orginal = { };
            List<int> tersmetabinarystm = new List<int>();
            List<int> tersmetabinarystn = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                tersmetabinarystm.Add(mm[i] % 2);
            }

            //int tersmetaintm = Convert.ToInt32(tersmetabinarystm.ToString(), 2);
            int tersmetaintm = binnary_to_decimal(tersmetabinarystm);
            for (int i = 16; i < 32; i++)
            {
                tersmetabinarystn.Add(mm[i] % 2);
            }  
            int tersmetaintn = binnary_to_decimal(tersmetabinarystn); 

            List<int> tersbinary_STB = new List<int>();
            string tersbinary = "";
            //MessageBox.Show(tersmetaintm+"--||--"+tersmetaintn);
            //MessageBox.Show(tersmetaintm * tersmetaintn * 8 * 4 + ">>>>>>>>>>>>>>>>>" + mm.Length);
            int size = tersmetaintm * tersmetaintn * 8 * 3;
            for (int i = 32; i < 32 + tersmetaintm * tersmetaintn * 8 * 3; i++)
            {

                tersbinary_STB.Add(mm[i] % 2);

            }
            ///////////########BURA NIYE ISLEMIR
            // sonra = tersbinary_STB.ToString();
            List<int> ededstr_STB = new List<int>();
            orginal = new int[((tersbinary_STB.Count) / 8) + 2];
            for (int i = 0; i < tersbinary_STB.Count; i++)
            {
                ededstr_STB.Add(tersbinary_STB[i]);
                if ((i + 1) % 8 == 0)
                {
                   // orginal[i / 8] = Convert.ToInt32(ededstr_STB.ToString(), 2);
                    orginal[i / 8] = binnary_to_decimal(ededstr_STB);
                    ededstr_STB.Clear();
                }
            }
            //MessageBox.Show(tersmetaintm + "))))" + tersmetaintn);
            orginal[orginal.Length - 2] = tersmetaintm;
            orginal[orginal.Length - 1] = tersmetaintn;
            return orginal;

        }



        //static StringBuilder sb;
        //static StringBuilder sb2;
        private void button4_Click(object sender, EventArgs e)
        {
            
            int[] pp;
            byte[] r = takergba(new Bitmap(pictureBox2.Image));  //stego seklin byte acilisi
            //byte[] v = takergba(new Bitmap(pictureBox1.Image));   //ana seklin stego acilisi
            int[] yy=new int [r.Length];
            for (int i = 0; i < yy.Length; i++)
            {
                yy[i] = r[i];
                //yy[i] = oobyte[i];
                //yy[i] = ee[i];
            }
                 //sb=new StringBuilder();
                 //sb2=new StringBuilder();
            //int f = 0; 
            //for (int i = 0; i < r.Length; i++)           //qapanisa geden stego sekil(oobyte) ile acilisa gelen stego seklin muqaisesi(r)
            //{
            //                          ///----MARAQLI NIYE ALINMIR
            //   if (r[i] == oobyte[i])
            //    {
            //        f++;
            //    }
            //   //sb.Append(oobyte[i]+" ");
            //   //sb2.Append(r[i]+" ");
            //}
            //if (f==r.Length)
            //{
            //    MessageBox.Show("heyyoooo");
            //}
            //else
            //{
            //    MessageBox.Show("nooooo"+f);
            //}


            //File.WriteAllText(@"D:\oobyte.txt", sb.ToString());
            //File.WriteAllText(@"D:\r.txt", sb2.ToString());
            pp = tamTERSSTEGO_LIST(yy);
            byte[] ppbyte = new byte[pp.Length-2];

            for (int i = 0; i < pp.Length-2; i++)
            {
                ppbyte[i] = Convert.ToByte(pp[i]);
            }
            ////////ASAGINI METODA SALMAQ LAZIMDIR:
             
            Bitmap yeniResim = new Bitmap(pp[pp.Length - 2], pp[pp.Length - 1], PixelFormat.Format24bppRgb);

            int dizisayisi;
            IntPtr baslangicy;
            Rectangle rcty;
            BitmapData bmdatay;
            rcty = new Rectangle(0, 0, pp[pp.Length-2] ,pp[pp.Length-1]);
            bmdatay = yeniResim.LockBits(rcty, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            baslangicy = bmdatay.Scan0;
            dizisayisi = ppbyte.Length;
            Marshal.Copy(ppbyte, 0, baslangicy, dizisayisi);
            yeniResim.UnlockBits(bmdatay);
            pictureBox2.Image = yeniResim;

        } 
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public static List<int> convert_number_tobinary(int n)
        {
            List<int> aa = new List<int>();
            List<int> bb = new List<int>();
            //aa.Clear();
            //bb.Clear();
            for (int i = 0; n > 0; i++)
            {
                aa.Add(n % 2);
                n = n / 2;
            }
            for (int i = 0; i < aa.Count; i++)
            {
                bb.Add(aa[aa.Count - 1 - i]);
            }
            return bb;
        }
        public static List<int> convert_tobinary_N_lik(List<int> a, int n)
        {
            List<int> zeros = new List<int>(); 
            int s = a.Count;
            while (s < n)
            {
                zeros.Add(0);
                s++;
            }
            zeros.AddRange(a);
            return zeros;
        }
        public static List<int> convert_tobinary_tam(int n, int size)
        {
            return convert_tobinary_N_lik(convert_number_tobinary(n), size);
        }
        public static List<int> cevirikiliyeListt(int[] nn, int maxdigits)
        { 
            List<int> binarytaml = new List<int>();
            List<int> binarytaml8llik = new List<int>();
            for (int i = 0; i < nn.Length; i++)
            {
                binarytaml.AddRange(convert_tobinary_tam(nn[i], maxdigits));
            }
            return binarytaml;
        }
         public static int binnary_to_decimal(List<int> mm)
        { 
            int decimalValue = 0; 
            int base1 = 1;
            int s = mm.Count-1;
            while (s >=0)
            {
                int reminder = mm[s];
                decimalValue += reminder * base1;
                base1 = base1 * 2;
                s--;
            }
            return decimalValue;
        }
         public static int[] tamSTEGOimageLIST(int[] mm, int[] nn, int m, int n)
         {

             int[] cc = { m };
             int[] dd = { n }; 
             List<int> sm = cevirikiliyeListt(cc, 16);
             List<int> sn = cevirikiliyeListt(dd, 16); 
             List<int> binary32metadata = new List<int>();
             binary32metadata.AddRange(sm);
             binary32metadata.AddRange(sn);
             mm = stegoSTB(binary32metadata, 0, mm); 
             List<int> binarytam;   
             if (nn.Length * 8 + 32 <= mm.Length)
             {
                 binarytam = cevirikiliyeListt(nn, 8);
                 mm = stegoSTB(binarytam, 32, mm); 
             }

             else
             {
                 Console.WriteLine("massivin uzunlugu catmir");
             } 
             return mm;
         }
         public static int[] stegoSTB(List<int> binarytam, int idx, int[] mm)
         {
             for (int i = 0; i < binarytam.Count; i++)
             {

                 if (binarytam[i] == 0)
                 {

                     if (mm[i + idx] % 2 == 1)
                     {
                         mm[i + idx] -= 1;
                     } 
                 }
                 else
                 {
                     if (mm[i + idx] % 2 != 1)
                     {
                         mm[i + idx] += 1;
                     } 
                 } 
             } 
             return mm;
         }  

         private void button9_Click(object sender, EventArgs e)
         {
             openFileDialog1.ShowDialog();
             dosyayolu = openFileDialog1.FileName;
             bmp = new Bitmap(dosyayolu);
             pictureBox2.Image = bmp;
             
         }
    }
}
