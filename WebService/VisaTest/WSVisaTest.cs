using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSPagoServicio;
using WSPagoServicio.Clases;

namespace VisaTest
{
    [TestClass]
    class WSVisaTest
    {
        [TestMethod]
        public void AutorizacionTest()
        {
            PagoServicio wsvisa = new PagoServicio();
            string user = "";
            string pass = "";

            //RespuestaAutenticacion token = wsvisa.AutenticarUsuario();
        }
    }
}
