using System;
using System.Collections;
using System.Windows.Forms;

namespace OLSR.Configuration
{
    public delegate void MethodCallInvoker(object[] o);

    /// <summary>
    /// 
    /// Created by  APIF Moviquity S.A.
    /// 
    /// http://www.moviquity.com/
    /// 
    /// Developers:
    /// 
    ///     Alberto Martinez Garcia
    ///     Francisco Abril Bucero 
    ///     Jose Manuel Lopez Garcia
    /// 
    /// Clase para invocar a la actualizacion del formulario principal
    /// desde un hilo secundario de la aplicacion
    ///
    /// Version: 1.1
    ///
    /// </summary>
    public class ControlInvoker
    {
        
        private class MethodCall
        {
            public MethodCallInvoker invoker;
            public object[] arguments;

            public MethodCall(MethodCallInvoker invoker, object[] arguments)
            {
                this.invoker = invoker;
                this.arguments = arguments;
            }
        }

        private Control control;
        private Queue argumentInvokeList = new Queue();

        public ControlInvoker()
        {
        }

        public ControlInvoker(Control control)
        {
            this.control = control;
        }

        public void SetControl(Control pControl)
        {
            control = pControl;
        }  

        public void Invoke(MethodCallInvoker invoker, params object[] arguments)
        {
            argumentInvokeList.Enqueue(new MethodCall(invoker, arguments));
            control.Invoke(new EventHandler(ControlInvoke));
        }

        private void ControlInvoke(object sender, EventArgs e)
        {
            var methodCall = (MethodCall)argumentInvokeList.Dequeue();
            methodCall.invoker(methodCall.arguments);
        }


    }
}