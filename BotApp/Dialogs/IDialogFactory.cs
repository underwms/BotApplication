using System.Collections.Generic;

namespace Bot_Application1.Dialogs
{
    public interface IDialogFactory
    {
        T Create<T>();

        T Create<T, U>(U parameter);

        T Create<T>(IDictionary<string, object> parameters);
    }
}
