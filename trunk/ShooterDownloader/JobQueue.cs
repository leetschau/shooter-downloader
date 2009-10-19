/*
 *   Shooter Subtitle Downloader: Automatic Subtitle Downloader for the http://shooter.cn.
 *   Copyright (C) 2009  John Fung
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Affero General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Affero General Public License for more details.
 *
 *   You should have received a copy of the GNU Affero General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace ShooterDownloader
{
    public delegate void AllDoneHandler();

    class JobQueue
    {
        private Queue<IJob> _pendingJobs;
        private int _maxConcurrentJobs = 0;
        private Thread[] _jobHandlers;
        private volatile bool _continue = true;
        private int _jobsCount = 0;

        public JobQueue(int maxJobs)
        {
            _pendingJobs = new Queue<IJob>();
            _maxConcurrentJobs = Math.Min(maxJobs, ShooterConst.MaxConcurrentJobs);
            _maxConcurrentJobs = Math.Max(1, _maxConcurrentJobs);
            _jobHandlers = new Thread[_maxConcurrentJobs];
            for (int i = 0; i < _maxConcurrentJobs; i++)
            {
                _jobHandlers[i] = new Thread(this.JobHandler);
            }
        }

        public bool Running
        {
            get
            {
                if (_jobsCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public event AllDoneHandler AllDone;

        private void FireAllDone()
        {
            if (AllDone != null)
            {
                AllDone();
            }
        }

        public void AddJob(IJob job)
        {
            lock (_pendingJobs)
            {
                _pendingJobs.Enqueue(job);
                _jobsCount++;
                Monitor.Pulse(_pendingJobs);
            }
        }

        public void Start()
        {
            foreach (Thread jobHandler in _jobHandlers)
            {
                jobHandler.Start();
            }
        }

        public void Close()
        {
            lock (_pendingJobs)
            {
                _continue = false;
                Monitor.PulseAll(_pendingJobs);
            }

            foreach (Thread jobHandler in _jobHandlers)
            {
                jobHandler.Join();
            }
        }

        private void JobHandler()
        {
            while (_continue)
            {
                IJob job = null;
                lock (_pendingJobs)
                {
                    if (_continue && _pendingJobs.Count == 0)
                    {
                        //wait until there is a new job.
                        Monitor.Wait(_pendingJobs);
                    }

                    if (_continue && _pendingJobs.Count > 0)
                    {
                        //get the job.
                        job = _pendingJobs.Dequeue();
                    }
                }

                if (_continue && job != null)
                {
                    //do the job.
                    job.Start();

                    //job done 
                    _jobsCount--;

                    if (_jobsCount == 0)
                    {
                        FireAllDone();
                    }
                }
                
            }
        }
    }
}
