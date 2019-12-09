using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeef.Menu
{
    public interface IMenuSelect
    {
        /// <summary>
        /// Given a list options, the user will return one of their choice.
        /// </summary>
        Task<object> GetSelectionAsync(Func<bool> isCancelled = null);

        /// <summary>
        /// Close the Menu Select.
        /// </summary>
        void Close();
    }
}
