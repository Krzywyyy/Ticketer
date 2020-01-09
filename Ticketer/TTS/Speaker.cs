using Microsoft.Speech.Synthesis;
using System;

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

        public void SystemSpeak(string message)
        {
            Console.WriteLine(message);
            synthesizer.Speak(message);
        }

        public void EmployeeSpeak(string message, string options)
        {
            Console.WriteLine("Pracownik: " + message + " " + options);
            synthesizer.Speak(message);
        }

        public void CustomerSpeak(string message)
        {
            Console.WriteLine("Ty: " + message);
        }

        public void AskForRepeat()
        {
            synthesizer.Speak("Nie do końca zrozumiałam. Czy możesz powtórzyć?");
        }

        public void SayGoodbye()
        {
            string goodbyeMessage = "W takim razie dziękuję i zapraszamy ponownie.";
            Console.WriteLine("Pracownik: " + goodbyeMessage);
            synthesizer.Speak(goodbyeMessage);
            Console.Clear();
        }
    }
}
