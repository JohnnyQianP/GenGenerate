using System;

namespace GenGenerate.Core.Infrastructure
{
    public class SnowflakeIdMaker
    {
        //private volatile static SnowflakeIdMaker instance;

        //public static SnowflakeIdMaker GetInstance() 
        //{
        //    if (instance == null)
        //    {
        //        lock (typeof(SnowflakeIdMaker)) {
        //            if (instance==null) 
        //            {
        //                instance = new SnowflakeIdMaker();
        //            }
        //        }
        //    }
        //    return instance;
        //}
        readonly static object _lock = new Object();
        public static long PackGenId()
        {
            lock (_lock)
            {
                return GenId();
            }
        }


        public static long GenId()
        {
            var currentTimeStamp = TimeStamp();

            var bl = 0;
            //如果是同一时间生成的，则进行毫秒内序列
            if (SnowflakeUtils.Last_Time_Stamp == currentTimeStamp)
            {
                bl = 1;
                SnowflakeUtils.Last_Seq = (SnowflakeUtils.Last_Seq + 1) & SnowflakeUtils.SEQ_MAX_NUM;
                //毫秒内序列溢出
                if (SnowflakeUtils.Last_Seq == 0)
                {
                    //阻塞到下一个毫秒,获得新的时间戳
                    currentTimeStamp = tilNextMillis(SnowflakeUtils.Last_Time_Stamp);
                }
            }
            //时间戳改变，毫秒内序列重置
            else
            {
                SnowflakeUtils.Last_Seq = 0L;
            }
            //上次生成ID的时间截
            SnowflakeUtils.Last_Time_Stamp = currentTimeStamp;

            var time = currentTimeStamp << SnowflakeUtils.Time_Left_Bit;
            var work = SnowflakeUtils.Work_Id << SnowflakeUtils.Work_Left_Bit;
            var id = time | work | SnowflakeUtils.Last_Seq;
            //Console.WriteLine($"{SnowflakeUtils.Last_Seq}_{id}_{bl}");
            //SnowflakeUtils.Last_Seq++;
            return id;
        }

        /**
     * 阻塞到下一个毫秒，直到获得新的时间戳
     * @param lastTimestamp 上次生成ID的时间截
     * @return 当前时间戳
     */
        private static long tilNextMillis(long lastTimestamp)
        {
            long timestamp = TimeStamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeStamp();
            }
            return timestamp;
        }


        private static long TimeStamp(long lastTimestamp = 0L)
        {
            var current = (DateTime.Now.Ticks - SnowflakeUtils.Start_Time) / 10000;
            if (lastTimestamp == current)
            {
                return TimeStamp(lastTimestamp);
            }

            return current;
        }


    }
}
