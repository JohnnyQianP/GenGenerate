using System;
using System.Collections.Generic;
using System.Text;

namespace GenGenerate.Core.Infrastructure
{
    public class SnowflakeUtils
    {
        //1,41,5 5,12
        private static int Time_Len = 41;
        //private static int Data_Len = 5;

        private static int Work_Len = 10;

        /// <summary>
        /// sequence id
        /// </summary>
        private static int Seq_Len = 12;

        //起始
        public static long Start_Time = 1577808000;
        //上次生成id的时间戳
        public static long Last_Time_Stamp = -1;

        //时间部分左移22位
        public static int Time_Left_Bit = 22;

        //private static int Data_Left_Bit = 17;

        public static int Work_Left_Bit = 12;


        private static int WORK_MAX_NUM = ~(-1 << Work_Len);

        //生成序列的掩码 4095
        public static long SEQ_MAX_NUM = ~(-1 << Seq_Len);
        private static long SEQ_MAX_NUM1 = -1 ^ (-1 << Seq_Len);

        //private static long Data_Id = 0;
        public static long Work_Id = getWorkId();

        public static long Last_Seq = 0;


        public static int getWorkId()
        {
            try
            {
                var host = System.Net.Dns.GetHostName();
                //HttpContext.Connection.LocalIpAddress.MapToIPv4()?.ToString();

                return getHostId(host, WORK_MAX_NUM);
            }
            catch (Exception e)
            {
                return new Random().Next(WORK_MAX_NUM);
            }
        }

        private static int getHostId(string s, int max)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            int sums = 0;
            foreach (var item in bytes)
            {
                sums += item;
            }

            return sums % (max + 1);
        }
    }
}
