using System;
using System.Collections.Generic;
using System.Text;

namespace Pigi.Captcha
{
    internal class ImgResources
    {
        private static string audio = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAADfSURBVDhPzZIxDgFBFIZHo6FScAbR6ZxAK8QliBOgouUELiGRcBMSOnqcgO+f3bfZXXYlCvEnX/Lmzbx/570d99dqQTsIE1K+EYTZ0qE79P0qqQ2coOxXb2TFD4gbKN+BKtxgCi+y4jWkDRZwhQrM4AwF8I4HUIFQcTGMZSDTEejK+vIQmqD9OrhuuNDhHqhYstwYLkqgLaygBNr3Q/5kMAEz2IEMdJvIQC3sw4T41MIAEi3ElTfEJdgQ5xANMS0zSRsor99YA91Es8mUmcQNTHpIR9AgcyWTr5/yr+TcE6KcPMM9mUxsAAAAAElFTkSuQmCC";
        private static string refresh = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAADRSURBVDhPzdI7DgFRFIfxadDZgEqEdXl0HoXleMQC7MOjQaVgF56VhO/LzCRi7jQK8U9+MbnkmHvOif4q5eSzgB5WOOOKNfooIjdLNLDDDVO00cQ4OfO7CoJ54oEDah58pIo9tgi+iQXkq6fX+YxF7vCKmczfDD3IyQT+ydexLzY3kwta8eN3cVSj+DEYr/V+zUwGcFQ2KhQb693TZmdSgnN2VKEidRzhqIMFjEtiEUdlt+1JB7PkbAMLuXS5cUlcW39kY09YoAvX3OTtyc8TRS/xzTJRMFY69gAAAABJRU5ErkJggg==";
        private static byte[] _audioBytes;
        private static byte[] _refreshBytes;
        
        internal static byte[] GetAudioImg
        {
            get
            {
                if (_audioBytes == null)
                    _audioBytes = Convert.FromBase64String(audio);
                return _audioBytes;
            }
        }
        
        internal static byte[] GetRefreshImg
        {
            get
            {
                if (_refreshBytes == null)
                    _refreshBytes = Convert.FromBase64String(refresh);
                return _refreshBytes;
            }
        }

        
    }
}
