using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Assistment.Service
{
    public class Distributor
    {
        public delegate void Job(int i);

        public Job job;
        public string process;
        public int n = 8;

        public Distributor(Job job, string process)
        {
            this.job = job;
            this.process = process;
        }

        public void handle(string[] args)
        {
            if (args == null || args.Length < 1)
                for (int i = 0; i < n; i++)
                    Process.Start(process, i + "");
            else
                job(int.Parse(args[0]));
        }
    }
}
