using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;


namespace หัดเรียน4
{
    public partial class Form1 : Form
    {
        OpenFileDialog op = new OpenFileDialog();
        Image<Bgr, byte> Image1 = null;
        Image<Gray, byte> image2 = null;
        int ku = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (op.ShowDialog() == DialogResult.OK)
            {
                //----รับภาพ--------------------------------------------------
                Image1 = new Image<Bgr, byte>(op.FileName);
                imageBox1.Image = Image1;

            }
        } //โหลด
        private void imageBox2_Click(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (Image1 != null)
            {

                //---แปลงเป็น hsv------------------------------------------------
                Image<Hsv, byte> hsv = Image1.Convert<Hsv, byte>();


                //แปลงเป็น ภาพเทา แบบ อาเรย์-Create result image----------------------------------------
                Image<Gray, byte>[] channels = hsv.Split();

                CvInvoke.InRange(channels[0], new ScalarArray(new MCvScalar(20)), new ScalarArray(new MCvScalar(160)), channels[0]);
                //อันแรกคือ H อันสอง คือ S

                channels[0]._Not();
                channels[1]._ThresholdBinary(new Gray(100), new Gray(255));

                //อันนี้ คือV new Gray(10)


                IInputArray mask = null;
                CvInvoke.BitwiseAnd(channels[0], channels[1], channels[0], mask);

                image2 = channels[0].Canny(150,0);


                //-------------------------วาดกรอบโดยการใช้contour---------------------------------
                
                Emgu.CV.Util.VectorOfVectorOfPoint contour = new Emgu.CV.Util.VectorOfVectorOfPoint();
                // คือการประกาศตัวแปรcountor 

                    Mat hier = new Mat();
                CvInvoke.FindContours(image2, contour, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                //คือการค้นหา contours 


         
                //---------------------------------------------------------------------------
                //ทำการวนลูป 
                for(int i =0; i < contour.Size;i++ )
                {
                    double paramiter = CvInvoke.ArcLength(contour[i], true);
                    Emgu.CV.Util.VectorOfPoint approx = new Emgu.CV.Util.VectorOfPoint();
                    CvInvoke.ApproxPolyDP(contour[i],approx,0.04*paramiter,true);

                    CvInvoke.DrawContours(Image1, contour, i, new MCvScalar(0, 0, 0), 6); // i บอกจำนวนที่วาด
                    ku++;

                    //-------------หาศูนย์กลาง--------------
                    var moment = CvInvoke.Moments(contour[i]);
                    int x = (int)(moment.M10 / moment.M00);
                    int y = (int)(moment.M01/moment.M00);

                    if(approx.Size == 3 )
                    {
                        CvInvoke.PutText(Image1, ". ", new Point(x,y),Emgu.CV.CvEnum.FontFace.HersheyScriptSimplex,1.5,new MCvScalar(0,0,0),10);
                    }
                    if (approx.Size == 5)
                    {
                        CvInvoke.PutText(Image1, ". ", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheyScriptSimplex, 1.5, new MCvScalar(0, 0, 0), 10);
                    }
                    if (approx.Size == 6)
                    {
                        CvInvoke.PutText(Image1, ". ", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheyScriptSimplex, 1.5, new MCvScalar(0, 0, 0), 10);
                    }
                    if (approx.Size > 6)
                    {
                        CvInvoke.PutText(Image1, ". ", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheyScriptSimplex, 1.5, new MCvScalar(0, 0, 0), 10);
                    }
                    imageBox2.Image = Image1;
                }



                textBox1.Text = ku.ToString();

               ku = 0;
            }
        } //แดง


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
