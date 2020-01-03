using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace TtsExec
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = CreateAudio(args[0]).Result;
            Console.Out.Write(Convert.ToBase64String(data));
        }

        public async static Task<byte[]> CreateAudio(string text)
        {
            Task<byte[]> task = Task.Run(() => {

                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                using (MemoryStream streamAudio = new MemoryStream())
                {
                    synth.SetOutputToWaveStream(streamAudio);
                    synth.Rate = -2;


                    foreach (var charc in text)
                    {
                        synth.Speak(charc.ToString());
                    }

                    // Speak a phrase.  
                    streamAudio.Position = 0; 
                    synth.SetOutputToNull();

                        return streamAudio.ToArray();
                }
            }); 
            return await task;
        }
    }
}
