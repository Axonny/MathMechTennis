using System;
using System.Threading.Tasks;

namespace App
{
    public interface IUi
    {
        Task ShowMessage(string text);
    }
}