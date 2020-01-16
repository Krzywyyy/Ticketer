using Microsoft.Speech.Synthesis;
using System;
using System.Threading;

namespace Ticketer.TTS
{
    class Speaker
    {
        private SpeechSynthesizer synthesizer;

        public Speaker()
        {
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
        }

        public string SystemSpeak(string message)
        {
            Speak(message);
            return message;
        }

        public string EmployeeSpeak(string message, string options)
        {
            Speak(message);
            return "Pracownik: " + message + " " + options;
        }

        public string CustomerSpeak(string message)
        {
            return ("Ty: " + message);
        }

        public void AskForRepeat()
        {
            Speak("Nie do końca zrozumiałam. Czy możesz powtórzyć?");
        }

        public void SayGoodbye()
        {
            Speak("W takim razie dziękuję i zapraszamy ponownie.");
        }

        private void Speak(string message)
        {
            Thread thread = new Thread(() => synthesizer.Speak(message));
            thread.Start();
        }
    }
}
