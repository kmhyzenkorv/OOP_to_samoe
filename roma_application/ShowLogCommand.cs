using System;
using System.Collections.Generic;
using System.Text;

namespace roma_application
{
    public class ShowLogCommand : Command
    {
        private Logger logger;

        public ShowLogCommand(Logger l)
        {
            logger = l;
        }

        public void execute()
        {
            logger.show();
        }
    }
}
