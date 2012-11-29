using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Auton.CarVision.Video
{
    public class ResultReadyEventArgs<ResultType>
    {
        public ResultType Result { get; private set; }
        public ResultReadyEventArgs(ResultType value)
        {
            Result = value;
        }
    }

    public abstract class Supplier<ResultType>
    {
        public virtual ResultType LastResult { get; protected set; }
        public delegate void ResultReadyEventHandler(object sender, ResultReadyEventArgs<ResultType> e);
        public event ResultReadyEventHandler ResultReady;

        protected void OnResultReady(ResultReadyEventArgs<ResultType> e)
        {
            if (ResultReady != null)
                ResultReady(this, e);
        }
    }

    public abstract class ThreadSupplier<MaterialType, ResultType> : Supplier<ResultType>
    {
        protected delegate void MaterialProcessor(MaterialType value);
        protected MaterialProcessor Process;

        private ResultType result;
        public override ResultType LastResult 
        { 
            get { return result; } 
            protected set 
            {
                result = value; 
                OnResultReady(new ResultReadyEventArgs<ResultType>(LastResult));
            }
        }

        bool is_pending;
        MaterialType pending;
        bool working = false;

        protected void PostComplete()
        {
            working = false;
            if (is_pending)
                PostProcess();
        }

        protected void PostFailed()
        {
            working = false;
        }

        public void PostProcess()
        {
            if (!working)
            {
                working = true;
                is_pending = false;
                new Thread(() => { Process(pending); }).Start();
            }
        }

        protected void MaterialReady(object sender, ResultReadyEventArgs<MaterialType> e)
        {
            is_pending = true;
            pending = e.Result;
            
            PostProcess();
        }
    }
}
