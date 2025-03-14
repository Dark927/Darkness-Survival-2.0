

using System;

namespace Settings.Global
{
    public interface IEventListener
    {
        public void Listen(object sender, EventArgs e);
    }
}
