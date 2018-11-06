using System.ServiceProcess;

namespace WindowsService
{
    partial class Service : ServiceBase
    {
        private int timerCount;

        public Service()
        {
            InitializeComponent();

            timer.Enabled = true;                 
        }

        protected override void OnStart(string[] args)
        {
            timer.Start();
            // TODO: agregar código aquí para iniciar el servicio.
        }

        protected override void OnStop()
        {
            timer.Stop();
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            timerCount++;

            if (timerCount.Equals(5))
            {
                timerCount = 0;

                /* Código para realizar la Réplica:
                 * 1.- Obtener registros nuevos de base de datos Informix.
                 * 2.- Insertar registros en base de datos Microsoft SQL Server
                   Fin. */
            }
        }
    }
}
