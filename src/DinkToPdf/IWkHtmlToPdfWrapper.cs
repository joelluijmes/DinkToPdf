using System;
using DinkToPdf.Callbacks;

namespace DinkToPdf
{
    public interface IWkHtmlToPdfWrapper
    {
        void AddObject(IntPtr converter, IntPtr objectSettings, byte[] data);
        void AddObject(IntPtr converter, IntPtr objectSettings, string data);
        IntPtr CreateConverter(IntPtr globalSettings);
        IntPtr CreateGlobalSettings();
        IntPtr CreateObjectSettings();
        void DestroyConverter(IntPtr converter);
        void DestroyGlobalSetting(IntPtr settings);
        void DestroyObjectSetting(IntPtr settings);
        void Dispose();
        bool DoConversion(IntPtr converter);
        bool ExtendedQt();
        byte[] GetConversionResult(IntPtr converter);
        int GetCurrentPhase(IntPtr converter);
        string GetGlobalSetting(IntPtr settings, string name);
        string GetLibraryVersion();
        string GetObjectSetting(IntPtr settings, string name);
        int GetPhaseCount(IntPtr converter);
        string GetPhaseDescription(IntPtr converter, int phase);
        string GetProgressString(IntPtr converter);
        void Initialize();
        int SetErrorCallback(IntPtr converter, StringCallback callback);
        int SetFinishedCallback(IntPtr converter, IntCallback callback);
        int SetGlobalSetting(IntPtr settings, string name, string value);
        int SetObjectSetting(IntPtr settings, string name, string value);
        int SetPhaseChangedCallback(IntPtr converter, VoidCallback callback);
        int SetProgressChangedCallback(IntPtr converter, VoidCallback callback);
        int SetWarningCallback(IntPtr converter, StringCallback callback);
    }
}