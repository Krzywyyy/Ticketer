using Microsoft.Speech.Recognition;
using Ticketer.ASR.Validators;
using Ticketer.MyProperties;
using Ticketer.TTS;

namespace Ticketer.ASR
{
    class InitEngine
    {
        private SpeechRecognitionEngine pSRE;
        private Speaker speaker;
        public InitEngine()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            PrepareGrammar(pSRE);
            speaker = new Speaker();
        }

        public void GetAnswer()
        {
            RecognitionResult recognition;
            do
            {
                recognition = pSRE.Recognize();
            } while (!RecognitionValidator.CorrectlyRecognized(recognition));
        }

        private void PrepareGrammar(SpeechRecognitionEngine engine)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            Choices choices = new Choices();
            choices.Add("Rozpocznij");
            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }
    }
}
