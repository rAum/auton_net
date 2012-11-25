using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CarVision
{
    public class ResultReadyEventArgs
    {
        public object Result { get; private set; }
        public ResultReadyEventArgs(object value)
        {
            Result = value;
        }
    }

    abstract class Supplier<ResultType>
    {
        public virtual ResultType LastResult { get; protected set; }
        public delegate void ResultReadyEventHandler(object sender, ResultReadyEventArgs e);
        public event ResultReadyEventHandler ResultReady;

        protected void OnResultReady(ResultReadyEventArgs e)
        {
            if (ResultReady != null)
                ResultReady(this, e);
        }
    }

    abstract class ThreadSupplier<MaterialType, ResultType> : Supplier<ResultType>
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
                OnResultReady(new ResultReadyEventArgs(LastResult));
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

        private void PostProcess()
        {
            if (!working)
            {
                working = true;
                is_pending = false;
                new Thread(() => { Process(pending); }).Start();
            }
        }

        protected void MaterialReady(object sender, ResultReadyEventArgs e)
        {
            if (e.Result is MaterialType)
            {
                is_pending = true;
                pending = (MaterialType)e.Result;
            }
            else
                throw new InvalidCastException("MaterialReady called with wrong material type");
            PostProcess();
        }
    }
}
