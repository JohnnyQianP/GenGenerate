using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenGenerate.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenGenerate.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenPackController : ControllerBase
    {
        [HttpGet]
        public long Get()
        {
            return SnowflakeIdMaker.PackGenId();
        }

        [HttpGet("QueryValidateMuitySnowId")]
        public bool QueryValidateMuitySnowId(int loop) 
        {
            ConcurrentDictionary<long, int> concurrentDictionary = new ConcurrentDictionary<long, int>();
            Task[] tasks = new Task[loop];
            for (int ij = 0; ij < loop; ij++)
            {
                Task task = new Task(() =>
                {
                    var threadid = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"当前线程id:{threadid}");
                    var id = SnowflakeIdMaker.PackGenId();
                    var ret = concurrentDictionary.TryAdd(id, ij);
                    if (!ret)
                    {
                        var val = 0;
                        concurrentDictionary.TryGetValue(id, out val);
                        Console.WriteLine($"error:{id}_{ij}; exist:{val}");
                    }
                });
                tasks[ij] = task;
                task.Start();
            }
            Task.WaitAll(tasks);
            return concurrentDictionary.Count==loop;
        }
    }
}
