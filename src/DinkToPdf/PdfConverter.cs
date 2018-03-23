using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DinkToPdf.Document;
using DinkToPdf.EventArgs;

namespace DinkToPdf
{
    public sealed class PdfConverter : IPdfConverter
    {
        private readonly IWkHtmlToPdfWrapper _wkHtmlToPdf;

        private IDocument _processingDocument;

        public PdfConverter(IWkHtmlToPdfWrapper wkHtmlToPdf)
        {
            _wkHtmlToPdf = wkHtmlToPdf;
        }

        public event EventHandler<PhaseChangedArgs> PhaseChanged;

        public event EventHandler<ProgressChangedArgs> ProgressChanged;

        public event EventHandler<FinishedArgs> Finished;

        public event EventHandler<ErrorArgs> Error;

        public event EventHandler<WarningArgs> Warning;

        public byte[] Convert(IDocument document)
        {
            if (!document.GetObjects().Any())
                throw new ArgumentException("No objects is defined in document that was passed. At least one object must be defined.");

            _processingDocument = document;

            var result = new byte[0];
            _wkHtmlToPdf.Initialize();

            var converter = CreateConverter(document);

            //register events
            _wkHtmlToPdf.SetPhaseChangedCallback(converter, OnPhaseChanged);
            _wkHtmlToPdf.SetProgressChangedCallback(converter, OnProgressChanged);
            _wkHtmlToPdf.SetFinishedCallback(converter, OnFinished);
            _wkHtmlToPdf.SetWarningCallback(converter, OnWarning);
            _wkHtmlToPdf.SetErrorCallback(converter, OnError);

            var converted = _wkHtmlToPdf.DoConversion(converter);

            if (converted)
                result = _wkHtmlToPdf.GetConversionResult(converter);

            _wkHtmlToPdf.DestroyConverter(converter);

            return result;
        }

        private void OnPhaseChanged(IntPtr converter)
        {
            var currentPhase = _wkHtmlToPdf.GetCurrentPhase(converter);
            var eventArgs = new PhaseChangedArgs
            {
                Document = _processingDocument,
                PhaseCount = _wkHtmlToPdf.GetPhaseCount(converter),
                CurrentPhase = currentPhase,
                Description = _wkHtmlToPdf.GetPhaseDescription(converter, currentPhase)
            };

            PhaseChanged?.Invoke(this, eventArgs);
        }

        private void OnProgressChanged(IntPtr converter)
        {
            var eventArgs = new ProgressChangedArgs
            {
                Document = _processingDocument,
                Description = _wkHtmlToPdf.GetProgressString(converter)
            };

            ProgressChanged?.Invoke(this, eventArgs);
        }

        private void OnFinished(IntPtr converter, int success)
        {
            var eventArgs = new FinishedArgs
            {
                Document = _processingDocument,
                Success = success == 1
            };

            Finished?.Invoke(this, eventArgs);
        }

        private void OnError(IntPtr converter, string message)
        {
            var eventArgs = new ErrorArgs
            {
                Document = _processingDocument,
                Message = message
            };

            Error?.Invoke(this, eventArgs);
        }

        private void OnWarning(IntPtr converter, string message)
        {
            var eventArgs = new WarningArgs
            {
                Document = _processingDocument,
                Message = message
            };

            Warning?.Invoke(this, eventArgs);
        }

        private IntPtr CreateConverter(IDocument document)
        {
            IntPtr converter;

            var settings = _wkHtmlToPdf.CreateGlobalSettings();

            ApplyConfig(settings, document, true);

            converter = _wkHtmlToPdf.CreateConverter(settings);
            foreach (var obj in document.GetObjects())
            {
                if (obj == null)
                    continue;

                settings = _wkHtmlToPdf.CreateObjectSettings();

                ApplyConfig(settings, obj, false);
                _wkHtmlToPdf.AddObject(converter, settings, obj.GetContent());
            }

            return converter;
        }

        private void ApplyConfig(IntPtr config, IPdfObject settings, bool isGlobal)
        {
            if (settings == null)
                return;

            var properties = settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var propertyInfo in properties)
            {
                var attrs = propertyInfo.GetCustomAttributes().ToArray();
                var propValue = propertyInfo.GetValue(settings);

                if (propValue == null)
                    continue;

                if (attrs.Length > 0 && attrs[0] is WkHtmlAttribute)
                {
                    var attr = (WkHtmlAttribute) attrs[0];

                    Apply(config, attr.Name, propValue, isGlobal);
                }
                else if (propValue is IPdfObject)
                {
                    ApplyConfig(config, propValue as IPdfObject, isGlobal);
                }
            }
        }

        private void Apply(IntPtr config, string name, object value, bool isGlobal)
        {
            void ApplySetting(string strValue, string settingName = null)
            {
                if (isGlobal)
                    _wkHtmlToPdf.SetGlobalSetting(config, settingName ?? name, strValue);
                else
                    _wkHtmlToPdf.SetObjectSetting(config, settingName ?? name, strValue);
            }

            switch (value)
            {
            case bool boolValue:
                ApplySetting(boolValue.ToString());
                break;

            case double doubleValue:
                ApplySetting(doubleValue.ToString("0.##", CultureInfo.InvariantCulture));
                break;

            case Dictionary<string, string> dictionary:
                var index = 0;

                foreach (var pair in dictionary)
                {
                    if (pair.Key == null || pair.Value == null)
                        continue;

                    //https://github.com/wkhtmltopdf/wkhtmltopdf/blob/c754e38b074a75a51327df36c4a53f8962020510/src/lib/reflect.hh#L192
                    ApplySetting(null, name + ".append");
                    ApplySetting($"{pair.Key}\n{pair.Value}", $"{name}[{index}]");

                    index++;
                }

                break;

            default:
                ApplySetting(value.ToString());
                break;
            }
        }
    }
}