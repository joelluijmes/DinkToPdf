using System;
using System.Runtime.InteropServices;
using System.Text;
using DinkToPdf.Callbacks;
using DinkToPdf.Native;

namespace DinkToPdf
{
    public sealed class WkHtmlToPdfWrapper : IDisposable, IWkHtmlToPdfWrapper
    {
        private bool _disposed;
        private bool _initialized;

        public void Initialize()
        {
            if (_initialized)
                throw new InvalidOperationException("WkHtmlToPdf alreay has been initialized");

            if (Pinvoke.wkhtmltopdf_init(0) != 1)
                throw new InvalidOperationException("Failed to initialize wkhtmltopdf");

            _initialized = true;
        }

        public bool ExtendedQt()
        {
            return Pinvoke.wkhtmltopdf_extended_qt() == 1;
        }

        public string GetLibraryVersion()
        {
            return Marshal.PtrToStringAnsi(Pinvoke.wkhtmltopdf_version());
        }

        public IntPtr CreateGlobalSettings()
        {
            return Pinvoke.wkhtmltopdf_create_global_settings();
        }

        public int SetGlobalSetting(IntPtr settings, string name, string value)
        {
            return Pinvoke.wkhtmltopdf_set_global_setting(settings, name, value);
        }

        public unsafe string GetGlobalSetting(IntPtr settings, string name)
        {
            //default const char * size is 2048 bytes 
            var buffer = new byte[2048];

            fixed (byte* tempBuffer = buffer)
            {
                Pinvoke.wkhtmltopdf_get_global_setting(settings, name, tempBuffer, buffer.Length);
            }

            return GetString(buffer);
        }

        public void DestroyGlobalSetting(IntPtr settings)
        {
            Pinvoke.wkhtmltopdf_destroy_global_settings(settings);
        }

        public IntPtr CreateObjectSettings()
        {
            return Pinvoke.wkhtmltopdf_create_object_settings();
        }

        public int SetObjectSetting(IntPtr settings, string name, string value)
        {
            return Pinvoke.wkhtmltopdf_set_object_setting(settings, name, value);
        }

        public unsafe string GetObjectSetting(IntPtr settings, string name)
        {
            //default const char * size is 2048 bytes 
            var buffer = new byte[2048];

            fixed (byte* tempBuffer = buffer)
            {
                Pinvoke.wkhtmltopdf_get_object_setting(settings, name, tempBuffer, buffer.Length);
            }

            return GetString(buffer);
        }

        public void DestroyObjectSetting(IntPtr settings)
        {
            Pinvoke.wkhtmltopdf_destroy_object_settings(settings);
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            return Pinvoke.wkhtmltopdf_create_converter(globalSettings);
        }

        public void AddObject(IntPtr converter, IntPtr objectSettings, byte[] data)
        {
            Pinvoke.wkhtmltopdf_add_object(converter, objectSettings, data);
        }

        public void AddObject(IntPtr converter, IntPtr objectSettings, string data)
        {
            Pinvoke.wkhtmltopdf_add_object(converter, objectSettings, data);
        }

        public bool DoConversion(IntPtr converter)
        {
            return Pinvoke.wkhtmltopdf_convert(converter);
        }

        public void DestroyConverter(IntPtr converter)
        {
            Pinvoke.wkhtmltopdf_destroy_converter(converter);
        }

        public byte[] GetConversionResult(IntPtr converter)
        {
            var length = Pinvoke.wkhtmltopdf_get_output(converter, out var data);
            var result = new byte[length];
            Marshal.Copy(data, result, 0, length);

            return result;
        }

        public int SetPhaseChangedCallback(IntPtr converter, VoidCallback callback)
        {
            return Pinvoke.wkhtmltopdf_set_phase_changed_callback(converter, callback);
        }

        public int SetProgressChangedCallback(IntPtr converter, VoidCallback callback)
        {
            return Pinvoke.wkhtmltopdf_set_progress_changed_callback(converter, callback);
        }

        public int SetFinishedCallback(IntPtr converter, IntCallback callback)
        {
            return Pinvoke.wkhtmltopdf_set_finished_callback(converter, callback);
        }

        public int SetWarningCallback(IntPtr converter, StringCallback callback)
        {
            return Pinvoke.wkhtmltopdf_set_warning_callback(converter, callback);
        }

        public int SetErrorCallback(IntPtr converter, StringCallback callback)
        {
            return Pinvoke.wkhtmltopdf_set_error_callback(converter, callback);
        }

        public int GetPhaseCount(IntPtr converter)
        {
            return Pinvoke.wkhtmltopdf_phase_count(converter);
        }

        public int GetCurrentPhase(IntPtr converter)
        {
            return Pinvoke.wkhtmltopdf_current_phase(converter);
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            return Marshal.PtrToStringAnsi(Pinvoke.wkhtmltopdf_phase_description(converter, phase));
        }

        public string GetProgressString(IntPtr converter)
        {
            return Marshal.PtrToStringAnsi(Pinvoke.wkhtmltopdf_progress_string(converter));
        }

        private static string GetString(byte[] buffer)
        {
            var nullchar = Array.IndexOf(buffer, 0);
            if (nullchar < 0)
                nullchar = 0;

            return Encoding.UTF8.GetString(buffer, 0, nullchar);
        }

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException("Object already has been disposed");

            Pinvoke.wkhtmltopdf_deinit();
            _disposed = true;
        }


        private static unsafe class Pinvoke
        {
            private const string DLLNAME = "libwkhtmltox";

            private const CharSet CHARSET = CharSet.Unicode;

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_extended_qt();

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wkhtmltopdf_version();

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_init(int useGraphics);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_deinit();

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wkhtmltopdf_create_global_settings();

            [DllImport(DLLNAME, CharSet = CHARSET)]
            public static extern int wkhtmltopdf_set_global_setting(IntPtr settings,
                [MarshalAs((short) CustomUnmanagedType.LPUTF8Str)]
                string name,
                [MarshalAs((short) CustomUnmanagedType.LPUTF8Str)]
                string value);


            [DllImport(DLLNAME, CharSet = CHARSET)]
            public static extern int wkhtmltopdf_get_global_setting(IntPtr settings,
                [MarshalAs((short) CustomUnmanagedType.LPUTF8Str)]
                string name,
                byte* value,
                int valueSize);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_destroy_global_settings(IntPtr settings);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wkhtmltopdf_create_object_settings();

            [DllImport(DLLNAME, CharSet = CHARSET)]
            public static extern int wkhtmltopdf_set_object_setting(IntPtr settings,
                [MarshalAs((short) CustomUnmanagedType.LPUTF8Str)]
                string name,
                [MarshalAs((short) CustomUnmanagedType.LPUTF8Str)]
                string value);

            [DllImport(DLLNAME, CharSet = CHARSET)]
            public static extern int wkhtmltopdf_get_object_setting(IntPtr settings,
                [MarshalAs((short) CustomUnmanagedType.LPUTF8Str)]
                string name,
                byte* value,
                int valueSize);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_destroy_object_settings(IntPtr settings);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wkhtmltopdf_create_converter(IntPtr globalSettings);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern void wkhtmltopdf_add_object(IntPtr converter,
                IntPtr objectSettings,
                byte[] data);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern void wkhtmltopdf_add_object(IntPtr converter,
                IntPtr objectSettings,
                [MarshalAs((short) CustomUnmanagedType.LPUTF8Str)]
                string data);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool wkhtmltopdf_convert(IntPtr converter);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern void wkhtmltopdf_destroy_converter(IntPtr converter);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_get_output(IntPtr converter, out IntPtr data);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_phase_count(IntPtr converter);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_current_phase(IntPtr converter);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wkhtmltopdf_phase_description(IntPtr converter, int phase);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wkhtmltopdf_progress_string(IntPtr converter);

            [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
            public static extern int wkhtmltopdf_http_error_code(IntPtr converter);

        }
    }
}